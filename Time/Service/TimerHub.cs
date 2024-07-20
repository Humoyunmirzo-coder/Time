using Microsoft.AspNetCore.SignalR;

namespace Time.Service
{
    public class TimerHub : Hub
    {
        public async Task SendTimerData(DateTime startTime, DateTime endTime)
        {
            await Clients.All.SendAsync("ReceiveTimerData", startTime, endTime);
        }
    }
}
