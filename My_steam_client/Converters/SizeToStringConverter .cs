using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace My_steam_client.Converters
{
    class SizeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long size)
            {
                if (size >= 1_073_741_824)
                    return $"{size / 1_073_741_824.0:F2} GB";
                if (size >= 1_048_576)
                    return $"{size / 1_048_576.0:F2} MB";
                if (size >= 1024)
                    return $"{size / 1024.0:F2} KB";
                return $"{size} B";
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
