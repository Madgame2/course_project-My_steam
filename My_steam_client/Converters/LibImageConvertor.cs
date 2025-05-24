using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace My_steam_client.Converters
{
    public class LibImageConvertor : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string path || string.IsNullOrWhiteSpace(path))
                return null;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Common/LidResources", path);

            if (!File.Exists(fullPath))
            {
                Debug.WriteLine($"[ImageConverter] Файл не найден: {fullPath}");
                return null;
            }

            return LoadBitmapImage(fullPath);
        }

        private BitmapImage? LoadBitmapImage(string path)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"[ImageConverter] Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
