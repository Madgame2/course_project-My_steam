using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace My_steam_client.Converters
{
    public class ImageSourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что передали всё, что нужно
            if (values.Length < 6 || values[0] is not string imagePath)
                return null;

            // Берём цвета и флаг
            var normalBrush = values[1] as Brush;
            var hoverBrush = values[2] as Brush;
            var ActiveBrush = values[3] as Brush;
            var isOver = values[4] as bool? == true;
            var isChecked = values[5] as bool? == true;

            // Выбираем нужный цвет
            var colorToApply = isOver
                ? hoverBrush
                : normalBrush;

            colorToApply = isChecked ? ActiveBrush : colorToApply;

            // Собираем полный путь к ресурсу
            string fullPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "resources",
                imagePath);

            if (!File.Exists(fullPath))
                return null;

            string ext = Path.GetExtension(fullPath).ToLowerInvariant();
            if (ext == ".svg")
            {
                // Читаем SVG
                var settings = new WpfDrawingSettings();
                var reader = new FileSvgReader(settings);
                DrawingGroup drawing = reader.Read(fullPath);
                if (drawing != null && colorToApply != null)
                {
                    // Меняем цвет всех фигур
                    ApplyColor(drawing, colorToApply);
                }
                return drawing != null
                    ? new DrawingImage(drawing)
                    : null;
            }
            else
            {
                // Обычный растровый формат
                return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        private void ApplyColor(DrawingGroup group, Brush brush)
        {
            foreach (var child in group.Children)
            {
                if (child is GeometryDrawing gd)
                {
                    if (gd.Pen != null) gd.Pen.Brush = brush;
                    if (gd.Brush != null) gd.Brush = brush;
                }
                else if (child is DrawingGroup dg)
                {
                    ApplyColor(dg, brush);
                }
            }
        }
    }

}
