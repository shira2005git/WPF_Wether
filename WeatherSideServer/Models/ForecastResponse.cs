namespace WeatherSideServer.Models
{
    public class ForecastResponse
    {
        public City City { get; set; }
        public List<ForecastDay> Days { get; set; } = new List<ForecastDay>();
    }
}
