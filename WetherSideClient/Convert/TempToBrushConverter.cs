using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WetherSideClient.Convert
{
    public class TempToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Brushes.LightGray;

            double max = 0, min = 0;

            if (values[0] != null && double.TryParse(values[0].ToString(), out double parsedMax))
                max = parsedMax;

            if (values[1] != null && double.TryParse(values[1].ToString(), out double parsedMin))
                min = parsedMin;

            double avg = (max + min) / 2;

            if (avg <= 0) return Brushes.LightBlue;
            if (avg <= 10) return Brushes.Cyan;
            if (avg <= 15) return Brushes.LightGreen;
            if (avg <= 20) return Brushes.YellowGreen;
            if (avg <= 25) return Brushes.Gold;
            return Brushes.OrangeRed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
