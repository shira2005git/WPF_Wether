using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetherSideClient.Models;
using WetherSideClient.View;

namespace WetherSideClient.Services
{
    public class CityService : ICityService
    {
        private readonly DataContext _db;
        public CityService(DataContext db)
        {
            _db = db;
            _db.Database.EnsureCreated();
        }

      
        public async Task<List<HistoryCity>> GetHistory()
        {
            return await _db.HistoryCities
                .OrderByDescending(c => c.LastSearchedAt)
                .Take(25)
                .ToListAsync();
        }

        public async Task AddCityToHistory(HistoryCity city)
        {
            var existing = await _db.HistoryCities
                .FirstOrDefaultAsync(c => c.Name == city.Name);

            if (existing != null)
            {
                existing.LastSearchedAt = DateTime.Now;
            }
            else
            {
                city.LastSearchedAt = DateTime.Now;
                await _db.HistoryCities.AddAsync(city);
            }

            await _db.SaveChangesAsync();
            await RemoveOldHistory();
        }

        public async Task RemoveOldHistory()
        {
            var all = await _db.HistoryCities
                .OrderByDescending(c => c.LastSearchedAt)
                .ToListAsync();

            if (all.Count > 25)
            {
                var toRemove = all.Skip(25).ToList();
                _db.HistoryCities.RemoveRange(toRemove);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<HistoryCity?> GetHistoryCityByName(string name)
        {
            return await _db.HistoryCities.FirstOrDefaultAsync(c => c.Name == name);
        }

       
        public async Task<List<FavoriteCity>> GetFavorites()
        {
            return await _db.FavoriteCities.ToListAsync();
        }

        public async Task AddFavorite(string cityName)
        {
            var favorites = await _db.FavoriteCities.ToListAsync();

            if (favorites.Any(f => f.Name == cityName))
                return;

            if (favorites.Count >= 8)
            {
                var oldest = favorites.First();
                _db.FavoriteCities.Remove(oldest);
            }

            await _db.FavoriteCities.AddAsync(new FavoriteCity { Name = cityName });
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFavorite(int id)
        {
            var fav = await _db.FavoriteCities.FindAsync(id);
            if (fav != null)
            {
                _db.FavoriteCities.Remove(fav);
                await _db.SaveChangesAsync();
            }
        }
    }
}
