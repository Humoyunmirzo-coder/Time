using Microsoft.EntityFrameworkCore;

namespace Time.TimeDbcontext
{
    public class TimerContext : DbContext
    {
        public TimerContext(DbContextOptions<TimerContext> options) : base(options) { }

        public DbSet<TimerRecord> TimerRecords { get; set; }
    }

    public class TimerRecord
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
