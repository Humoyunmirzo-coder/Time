using Time.Data;
using Time.TimeDbcontext;

namespace Time.Repository
{
    public class TimerRepository
    {
        private readonly  TimerContext  _timerContext;

        public TimerRepository(TimerContext timerContext)
        {
            _timerContext = timerContext;
        }

        public async Task AddData(TimerEntity timer, CancellationToken cancellationToken = default)
        {
            await _timerContext.Timer.AddAsync(timer);
            await _timerContext.SaveChangesAsync(cancellationToken);
        }

        public Task<ICollection<TimerEntity>> GetData()
        {
            ICollection<TimerEntity> result = _timerContext.Timer.ToList();
            return Task.FromResult(result);
        }
    }
}
