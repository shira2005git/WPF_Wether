using WeatherSideServer.Models;

namespace WeatherSideServer.Services
{
    public interface IWeatherService
    {
        Task<ForecastResponse> GetForecast(string nameCity);
    }
}
