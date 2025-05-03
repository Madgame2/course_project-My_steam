using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Game_Net_DTOLib;

namespace My_steam_client.Converters
{
    public class RatingEnum_to_styles : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 8 || values[0] is not Ratinng rating)
                return null;

            var styles = values.Skip(1).OfType<Style>().ToArray();

            return rating switch
            {
                Ratinng.Extrimly_positive => styles.ElementAtOrDefault(0),
                Ratinng.Very_positive => styles.ElementAtOrDefault(1),
                Ratinng.Positive => styles.ElementAtOrDefault(2),
                Ratinng.Mixed => styles.ElementAtOrDefault(3),
                Ratinng.Mostly_negative => styles.ElementAtOrDefault(4),
                Ratinng.Very_negative => styles.ElementAtOrDefault(5),
                Ratinng.Extrimly_negative => styles.ElementAtOrDefault(6),
                _ => null
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
