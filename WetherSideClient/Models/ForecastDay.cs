using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetherSideClient.Models
{
    public class ForecastDay
    {
        public DateTime Date { get; set; }
        public string? Summary { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Icon { get; set; }

        public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png";

    }
}
