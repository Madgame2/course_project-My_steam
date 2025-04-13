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
            if (values.Length < 2) return Brushes.Gray;

            bool isActive = values[0] is bool b && b;
            var slider = values[1] as Slider;

            if (slider == null)
                return isActive ? Brushes.DodgerBlue : Brushes.LightGray;

            return isActive
                ? slider.IndicatorsActiveBrush ?? Brushes.DodgerBlue
                : slider.IndicatorsInactiveBrush ?? Brushes.LightGray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
