using Microsoft.AspNetCore.Mvc;
using WeatherSideServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherSideServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET api/<WeatherController>/5
        [HttpGet("{nameCity}")]
        public async Task<IActionResult> GetForecast(string nameCity)
        {
            var forecast = await _weatherService.GetForecast(nameCity);
            return Ok(forecast);
        }

    }
}
