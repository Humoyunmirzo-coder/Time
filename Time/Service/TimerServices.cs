using Stl.CommandR.Configuration;
using Stl.CommandR;
using Stl.Fusion;
using System.Reactive;

namespace Time.Service
{
    public class TimerServices
    {
        public record TimerDurationCons(int timer) : ICommand<Unit>;

        public class TimerService : IComputeService, IHostedService
        {
            public int TimerDuration;
            ICollection<Data.Timer> timerData = new List<Data.Timer>();

            private int remainingTime;
            private int elapsedTime;
            private DateTime startTime;
            private DateTime endTime;
            private bool isRunning = false;
            private int elapsedPercentage;
            public int isLoad = 0;

            private readonly object _lock = new();

            [CommandHandler]
            public virtual Task TimeDurationStart(TimerDurationCons timerD, CancellationToken cancellationToken = default)
            {
                var tmd = timerD.timer;
                if (Computed.IsInvalidating())
                {
                    _ = Get();
                    return Task.CompletedTask;
                }

                lock (_lock)
                {
                    TimerDuration = tmd;
                    remainingTime = tmd;
                    isRunning = true;
                    isLoad = 1;
                }
                return Task.CompletedTask;
            }

            [ComputeMethod]
            public virtual Task<(int, DateTime, DateTime)> Get()
            {
                lock (_lock)
                {
                    var _startTimer = DateTime.Now;
                    var _endTimer = _startTimer.AddSeconds(TimerDuration);
                    startTime = _startTimer;
                    endTime = _endTimer;
                    return Task.FromResult((TimerDuration, _startTimer, _endTimer));
                }
            }

            [ComputeMethod]
            public virtual Task<ICollection<Data.Timer>> GetData()
            {
                return Task.FromResult(timerData);
            }

            public Task Start(int timerDuration)
            {
                return Task.CompletedTask;
            }

            [ComputeMethod(AutoInvalidationDelay = 1)]
            public virtual Task<(bool, int, int, int, bool)> Update()
            {
                lock (_lock)
                {
                    if (isRunning)
                    {
                        if (remainingTime <= 0)
                        {
                            TimerDown();
                            if (isLoad == 2)
                                SaveData(startTime, endTime);
                        }
                        else
                        {
                            elapsedPercentage = (elapsedTime * 100) / TimerDuration;
                            remainingTime--;
                            elapsedTime++;
                        }
                    }

                    return Task.FromResult((isRunning, remainingTime, elapsedTime, elapsedPercentage, isPause));
                }
            }

            public Task SaveData(DateTime st, DateTime end)
            {
                lock (_lock)
                {
                    Data.Timer timerr = new Data.Timer()
                    {
                        Id = Guid.NewGuid(),
                        StartTime = st,
                        EndTime = end,
                    };
                    isLoad = 0;
                    timerData.Add(timerr);
                }
                return Task.CompletedTask;
            }

            bool isPause = false;

            public bool PauseHandler()
            {
                if (!isPause)
                {
                    isRunning = false;
                    isPause = true;
                }
                else
                {
                    isRunning = true;
                    isPause = false;
                }
                return isPause;
            }

            public void TimerDown()
            {
                isRunning = false;
                remainingTime = 0;
                elapsedTime = 0;
                elapsedPercentage = 0;
                isLoad = isLoad == 1 ? 2 : 0;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                await Console.Out.WriteLineAsync("Timer boshlandi...");
            }

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        }
    }
}
