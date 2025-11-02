using Microsoft.EntityFrameworkCore;
using System.IO;
using WetherSideClient.Models;

namespace WetherSideClient.View
{
    public class DataContext : DbContext
    {
        public DbSet<HistoryCity> HistoryCities { get; set; }
        public DbSet<FavoriteCity> FavoriteCities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cities.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}