using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;
using WeatherSideServer.Data;
using WeatherSideServer.Models;
using Microsoft.EntityFrameworkCore;

namespace WeatherSideServer.Services
{
    public class WetherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string API_KEY;
        private readonly DBContext _context;
        public WetherService(HttpClient httpClient, IMemoryCache cache, IConfiguration configuration, DBContext context)
        {
            _httpClient = httpClient;
            _cache = cache;
            API_KEY = configuration["WeatherApi:ApiKey"];
            _context = context;
        }

        public async Task<ForecastResponse> GetForecast(string nameCity)
        {

            if (_cache.TryGetValue(nameCity, out ForecastResponse cachedResponse))
            {
                System.Diagnostics.Debug.WriteLine($"✅ נשלף מה-Cache עבור {nameCity}");
                return cachedResponse;
            }

            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={nameCity}&appid={API_KEY}&units=metric&lang=he";

            var response = await _httpClient.GetStringAsync(url);

            var json = JObject.Parse(response);

            if (json["city"] == null)
                throw new Exception($"City '{nameCity}' not found.");

            var ExistCity = await _context.Cities.FirstOrDefaultAsync(c => c.Name == nameCity);

            if (ExistCity == null)
            {
                ExistCity = new City
                {
                    Name = json["city"]["name"].ToString(),
                    Latitude = (double)json["city"]["coord"]["lat"],
                    Longitude = (double)json["city"]["coord"]["lon"]
                };

                _context.Cities.Add(ExistCity);
                await _context.SaveChangesAsync();
            }

            var forecast = new ForecastResponse
            {
                City = ExistCity,
            };

            var dailyForecasts = json["list"]
            .GroupBy(item => DateTime.Parse(item["dt_txt"].ToString()).Date)
            .Take(5);

            foreach (var dayGroup in dailyForecasts)
            {
                var date = dayGroup.Key;
                var minTemp = dayGroup.Min(i => (double)i["main"]["temp_min"]);
                var maxTemp = dayGroup.Max(i => (double)i["main"]["temp_max"]);
                var summary = dayGroup.First()["weather"][0]["description"].ToString();
                var icon = dayGroup.First()["weather"][0]["icon"].ToString();

                var exists = await _context.ForecastDays
                    .FirstOrDefaultAsync(f => f.CityId == ExistCity.Id && f.Date.Date == date);

                if (exists != null)
                {
                    exists.MinTemp = minTemp;
                    exists.MaxTemp = maxTemp;
                    exists.Summary = summary;
                    exists.Icon = icon;
                    forecast.Days.Add(exists);
                }
                else
                {
                    var day = new ForecastDay
                    {
                        Date = date,
                        MinTemp = minTemp,
                        MaxTemp = maxTemp,
                        Summary = summary,
                        Icon = icon,
                        CityId = ExistCity.Id
                    };
                    forecast.Days.Add(day);
                    _context.ForecastDays.Add(day);
                }
            }
            await _context.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine($"🧭 {ExistCity.Name} - Lat: {ExistCity.Latitude}, Lon: {ExistCity.Longitude}");

            _cache.Set(nameCity, forecast, TimeSpan.FromHours(4));
            return forecast;
        }
    }
}
