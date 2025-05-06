using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace My_steam_client.Converters
{
    class DateTimeToSting : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date) { 
                var curentDate= DateTime.Now;

                if (date.Year != curentDate.Year)
                {
                    return date.ToString("MMM, dd, yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    return date.ToString("MMM, dd", CultureInfo.InvariantCulture);
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
