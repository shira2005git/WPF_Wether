using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherSideServer.Models
{
    public class ForecastDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

    }
}
