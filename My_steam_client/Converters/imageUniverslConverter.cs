using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Net;

namespace My_steam_client.Converters
{
    public class ImageUniversalConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string path || string.IsNullOrWhiteSpace(path))
                return null;

            try
            {
                bool isWebPath = path.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                              || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

                if (isWebPath)
                {
                    return LoadBitmapImage(path, isWeb: true);
                }
                else
                {
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", path);

                    if (!File.Exists(fullPath))
                    {
                        Debug.WriteLine($"[ImageConverter] Файл не найден: {fullPath}");
                        return null;
                    }

                    string extension = Path.GetExtension(fullPath)?.ToLowerInvariant();

                    return extension switch
                    {
                        ".svg" => LoadSvgImage(fullPath),
                        ".gif" => LoadGifImage(fullPath),
                        _ => LoadBitmapImage(fullPath, isWeb: false)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ImageConverter] Ошибка при конвертации: {ex.Message}");
                return null;
            }
        }

        private BitmapImage? LoadBitmapImage(string path, bool isWeb)
        {
            try
            {
                if (isWeb)
                {
                    using var webClient = new WebClient();
                    byte[] imageBytes = webClient.DownloadData(path);

                    using var stream = new MemoryStream(imageBytes);
                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();

                    if (bitmap.CanFreeze)
                        bitmap.Freeze();

                    return bitmap;
                }
                else
                {
                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(path, UriKind.Absolute);
                    bitmap.EndInit();

                    if (bitmap.CanFreeze)
                        bitmap.Freeze();

                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ImageConverter] Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }

        private DrawingImage? LoadSvgImage(string path)
        {
            try
            {
                var settings = new WpfDrawingSettings
                {
                    IncludeRuntime = false,
                    TextAsGeometry = false
                };

                var reader = new FileSvgReader(settings);
                var drawing = reader.Read(path);

                if (drawing != null)
                {
                    var image = new DrawingImage(drawing);
                    if (image.CanFreeze)
                        image.Freeze();

                    return image;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ImageConverter] Ошибка загрузки SVG: {ex.Message}");
            }

            return null;
        }

        private ImageSource? LoadGifImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Важно, чтобы поток не блокировался
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ImageConverter] Ошибка загрузки GIF: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

