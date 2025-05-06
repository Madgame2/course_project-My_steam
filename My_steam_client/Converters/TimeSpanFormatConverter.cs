using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace My_steam_client.Converters
{
    class TimeSpanFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TimeSpan ts)
            {
                if (ts.TotalMinutes <= 60)
                {
                    return $"{(int)ts.TotalMinutes} minutes";
                }
                else
                {
                    return $"{ts.TotalHours:F1} hours";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
