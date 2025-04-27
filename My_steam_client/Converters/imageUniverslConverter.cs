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
    public class imageUniverslConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string path || string.IsNullOrEmpty(path))
                return null;

            path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "resources",
                path);

            if (!File.Exists(path))
                return null;

            string extension = System.IO.Path.GetExtension(path)?.ToLowerInvariant();

            if(extension == ".svg")
            {
                return LoadSvgImage(path);

            }
            else
            {
                return LoadBitmapImage(path);

            }
        }

        private BitmapImage LoadBitmapImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
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

                if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                {
                    // Абсолютный путь к SVG
                    var drawing = reader.Read(path);
                    if (drawing != null)
                    {
                        var drawingImage = new DrawingImage(drawing);
                        drawingImage.Freeze();
                        return drawingImage;
                    }
                }
                else
                {
                    // Относительный путь
                    var basePath = AppDomain.CurrentDomain.BaseDirectory;
                    var fullPath = System.IO.Path.Combine(basePath, path);

                    if (File.Exists(fullPath))
                    {
                        // Если файл существует по абсолютному пути
                        var drawing = reader.Read(fullPath);
                        if (drawing != null)
                        {
                            var drawingImage = new DrawingImage(drawing);
                            drawingImage.Freeze();
                            return drawingImage;
                        }
                    }
                    else
                    {
                        // Если файла на диске нет, пытаемся открыть через поток ресурсов

                        var resourceUri = new Uri($"pack://application:,,,/{path}", UriKind.Absolute);
                        var streamResourceInfo = Application.GetResourceStream(resourceUri);

                        if (streamResourceInfo != null)
                        {
                            using (var stream = streamResourceInfo.Stream)
                            {
                                var drawing = reader.Read(stream);
                                if (drawing != null)
                                {
                                    var drawingImage = new DrawingImage(drawing);
                                    drawingImage.Freeze();
                                    return drawingImage;
                                }
                            }
                        }
                    }
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
