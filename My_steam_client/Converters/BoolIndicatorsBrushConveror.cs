using My_steam_client.Templates;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace My_steam_client.Converters
{
    public class BoolIndicatorsBrushConveror : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Brushes.Gray; // Изменено на Brush вместо SolidColorBrush

            bool isActive = values[0] is bool b && b;
            var slider = values[1] as Slider;

            Brush GetBrush(Brush baseBrush)
            {
                // Возвращаем базовый Brush, который может быть как SolidColorBrush, так и GradientBrush и т.д.
                return baseBrush ?? Brushes.Transparent;
            }

            if (slider == null)
                return isActive ? GetBrush(Brushes.DodgerBlue) : GetBrush(Brushes.LightGray);

            // Используем Brushes из Slider, если они заданы, иначе используем стандартные цвета
            return isActive
                ? GetBrush(slider.IndicatorsActiveBrush ?? Brushes.DodgerBlue)
                : GetBrush(slider.IndicatorsInactiveBrush ?? Brushes.LightGray);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
