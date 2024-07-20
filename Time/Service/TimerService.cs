using System;
using System.Timers;
using System.Threading.Tasks;

public class TimerService
{
    private System.Timers.Timer _timer;
    private DateTime _startTime;
    private DateTime _endTime;
    private TimeSpan _remainingTime;
    private bool _isPaused;
    private TimeSpan _elapsedTime;

    public event Action OnTick;
    public event Action OnCompleted;

    public void Start(TimeSpan duration)
    {
        _startTime = DateTime.Now;
        _endTime = _startTime.Add(duration);
        _remainingTime = duration;
        _isPaused = false;
        _elapsedTime = TimeSpan.Zero;

        _timer = new System.Timers.Timer(1000); // Timer interval set to 1 second
        _timer.Elapsed += TimerElapsed;
        _timer.Start();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (_isPaused)
        {
            return;
        }

        _elapsedTime = DateTime.Now - _startTime;
        _remainingTime = _endTime - DateTime.Now;

        if (_remainingTime <= TimeSpan.Zero)
        {
            _remainingTime = TimeSpan.Zero;
            _timer.Stop();
            OnCompleted?.Invoke();
        }
        else
        {
            OnTick?.Invoke();
        }
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    public DateTime GetStartTime() => _startTime;
    public DateTime GetEndTime() => _endTime;
    public TimeSpan GetRemainingTime() => _remainingTime;
    public TimeSpan GetElapsedTime() => _elapsedTime;
}
