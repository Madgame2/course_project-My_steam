using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace My_steam_client.Converters
{
    public class RectToClipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 3 &&
                values[0] is double width &&
                values[1] is double height &&
                values[2] is CornerRadius corner)
            {
                return new RectangleGeometry
                {
                    Rect = new Rect(0, 0, width, height),
                    RadiusX = corner.TopLeft,
                    RadiusY = corner.TopLeft
                };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
