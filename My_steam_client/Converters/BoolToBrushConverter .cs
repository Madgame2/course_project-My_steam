using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace My_steam_client.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush ActiveBrush { get; set; } = Brushes.White;
        public Brush InactiveBrush { get; set; } = Brushes.Gray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? ActiveBrush : InactiveBrush;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
