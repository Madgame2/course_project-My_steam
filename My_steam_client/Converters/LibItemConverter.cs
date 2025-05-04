using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace My_steam_client.Converters
{
    public class LibItemConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4
                || values[0] is not bool isActive
                || values[1] is not bool isMouseOver
                || values[2] is not double value1
                || values[3] is not double value2)
            {
                return null;
            }

            double result = 0;
            result += isActive ? value1 : 0;
            result += isMouseOver ? value2 : 0;

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
