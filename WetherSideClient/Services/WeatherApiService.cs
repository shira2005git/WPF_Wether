using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WetherSideClient.Models;

namespace WetherSideClient.Services
{
    public class WeatherApiService
    {
        private readonly HttpClient httpClient;

        public WeatherApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:7057/");
        }

        public async Task<ForecastResponse> GetForecast(string cityName)
        {
            return await httpClient.GetFromJsonAsync<ForecastResponse>($"api/weather/{cityName}");
        }

    }
}
