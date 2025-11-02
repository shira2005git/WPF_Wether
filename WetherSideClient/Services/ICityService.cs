using System.Collections.Generic;
using System.Threading.Tasks;
using WetherSideClient.Models;

namespace WetherSideClient.Services
{
    public interface ICityService
    {
        // היסטוריית ערים
        Task<List<HistoryCity>> GetHistory();
        Task AddCityToHistory(HistoryCity city);
        Task RemoveOldHistory(); // לשמירה על 25 אחרונות

        // ערים אהובות
        Task<List<FavoriteCity>> GetFavorites();
        Task AddFavorite(string cityName);
        Task RemoveFavorite(int id);

        // בדיקה אם עיר כבר קיימת בהיסטוריה
        Task<HistoryCity?> GetHistoryCityByName(string name);
    }
}
