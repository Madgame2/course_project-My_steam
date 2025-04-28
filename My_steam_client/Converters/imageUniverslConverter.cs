using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace My_steam_client.Converters
{
    public class ImageUniversalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string path || string.IsNullOrEmpty(path))
                return null;

            bool isWebPath = path.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                             || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

            if (isWebPath)
            {
                // Это сетевой путь
                return LoadBitmapImage(path, isWeb: true);
            }
            else
            {
                // Это локальный путь
                path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "resources",
                    path);

                if (!File.Exists(path))
                    return null;

                string extension = System.IO.Path.GetExtension(path)?.ToLowerInvariant();

                if (extension == ".svg")
                {
                    return LoadSvgImage(path);
                }
                else
                {
                    return LoadBitmapImage(path, isWeb: false);
                }
            }
        }

        private BitmapImage LoadBitmapImage(string path, bool isWeb)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;

                if (isWeb)
                {
                    bitmap.UriSource = new Uri(path, UriKind.Absolute);
                }
                else
                {
                    bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                }

                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        private DrawingImage LoadSvgImage(string path)
        {
            try
            {
                var settings = new WpfDrawingSettings();
                var reader = new FileSvgReader(settings);

                var drawing = reader.Read(path);
                if (drawing != null)
                {
                    var drawingImage = new DrawingImage(drawing);
                    drawingImage.Freeze();
                    return drawingImage;
                }
            }
            catch
            {
                // Ошибка чтения SVG
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
