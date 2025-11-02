using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetherSideClient.Models
{
    public class ForecastResponse
    {
        public HistoryCity? HistoryCity { get; set; }
        public List<ForecastDay> Days { get; set; } = new List<ForecastDay>();
    }
}
