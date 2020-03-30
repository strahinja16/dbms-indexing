using GeneratorApp.Model;
using Microsoft.EntityFrameworkCore;

namespace GeneratorApp.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WarehouseStatistic> Statistics { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
        }
    }
}
