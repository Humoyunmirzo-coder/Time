using Microsoft.EntityFrameworkCore;
using Time.Data;

namespace Time.TimeDbcontext
{
    public class TimerContext : DbContext
    {
        public TimerContext(DbContextOptions<TimerContext> options)
                  : base(options)
        {
        }

        public DbSet<TimerEntity> Timer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp");
                    }
                }
            }
        }
    }

  
}
