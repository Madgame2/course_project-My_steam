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
            if (values.Length < 2 || values[0] is not string imagePath)
                return null;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", imagePath);

            if (!File.Exists(fullPath))
                return null;

            string extension = Path.GetExtension(fullPath).ToLower();

            if (extension == ".svg")
            {
                var settings = new WpfDrawingSettings();
                var converter = new FileSvgReader(settings);
                DrawingGroup drawing = converter.Read(fullPath);

                if (drawing != null)
                {
                    if (values[1] is Brush newColor && newColor!= null)
                    {
                        SetColor(drawing, newColor);
                    }

                    
                    return new DrawingImage(drawing);
                }
            }
            else
            {
                return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        private void SetColor(DrawingGroup drawing, Brush color)
        {
            foreach (var child in drawing.Children)
            {
                if (child is GeometryDrawing geometryDrawing)
                {
                    if (geometryDrawing.Pen != null)
                    {
                        geometryDrawing.Pen.Brush = color;
                    }

                    if (geometryDrawing.Brush != null)
                    {
                        geometryDrawing.Brush = color;
                    }
                }
                else if (child is DrawingGroup group)
                {
                    SetColor(group, color);
                }
            }
        }
    }

}
