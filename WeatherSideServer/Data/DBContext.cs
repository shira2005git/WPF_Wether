using Microsoft.EntityFrameworkCore;
using WeatherSideServer.Models;

namespace WeatherSideServer.Data
{
    public class DBContext:DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<ForecastDay> ForecastDays { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
        }

    }
}
