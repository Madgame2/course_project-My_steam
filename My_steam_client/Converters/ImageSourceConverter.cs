using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
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
    class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath)
            {
                // Формируем полный путь к файлу
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", imagePath);

                if (File.Exists(fullPath))
                {
                    string extension = Path.GetExtension(fullPath).ToLower();

                    if (extension == ".svg")
                    {
                        var settings = new WpfDrawingSettings();
                        var converter = new FileSvgReader(settings);
                        DrawingGroup drawing = converter.Read(fullPath);

                        if (drawing != null)
                        {
                            return new DrawingImage(drawing);
                        }
                    }
                    else
                    {
                        return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                    }
                }
            }

            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
