using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace My_steam_client.Converters
{
    public class PrecentSizeConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4 ||
                values[0] == DependencyProperty.UnsetValue ||
                values[1] == DependencyProperty.UnsetValue ||
                values[2] == DependencyProperty.UnsetValue ||
                values[3] == DependencyProperty.UnsetValue)
                return null;

            try
            {
                double parentWidth = System.Convert.ToDouble(values[0]);
                double parentHeight = System.Convert.ToDouble(values[1]);
                double widthPercent = System.Convert.ToDouble(values[2]);
                double heightPercent = System.Convert.ToDouble(values[3]);

                string mode = parameter?.ToString()?.ToLower() ?? "";

                return mode switch
                {
                    "width" => parentWidth * (widthPercent / 100),
                    "height" => parentHeight * (heightPercent / 100),
                    _ => null
                };
            }
            catch
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
