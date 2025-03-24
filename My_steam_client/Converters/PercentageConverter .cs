using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace My_steam_client.Converters
{
    public class PercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 ||
                !(values[0] is double parentHeight) ||
                !IsNumber(values[1]))
                return DependencyProperty.UnsetValue;

            double percent = System.Convert.ToDouble(values[1]);
            return parentHeight * (percent / 100.0);
        }

        private bool IsNumber(object value)
        {
            return value is int || value is double || value is float || value is decimal;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
