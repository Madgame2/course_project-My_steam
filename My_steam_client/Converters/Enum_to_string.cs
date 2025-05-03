using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Game_Net_DTOLib;

namespace My_steam_client.Converters
{
    public class Enum_to_string : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Ratinng enumValue)
            {
                var raw = enumValue.ToString();

                string result = Regex.Replace(raw, @"([a-z])([A-Z])", "$1 $2")
                                .Replace("_", " ")
                                .ToLowerInvariant();

                return char.ToUpper(result[0])+result.Substring(1);
            }

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
