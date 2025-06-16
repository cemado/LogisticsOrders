using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LogisticsOrders.Infrastructure.Data
{
    public class LogisticsDbContext : DbContext
    {
        public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.OwnsOne(o => o.Origin);
                builder.OwnsOne(o => o.Destination);
            });
        }
    }
}
