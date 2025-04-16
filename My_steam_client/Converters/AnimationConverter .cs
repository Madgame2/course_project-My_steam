using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace My_steam_client.Converters
{
    public class AnimationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если анимация передана, используем её
            if (value is Storyboard storyboard && storyboard != null)
            {
                return storyboard;
            }

            // Если анимации нет, возвращаем стандартную
            return GetDefaultAnimation();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Storyboard GetDefaultAnimation()
        {
            // Создаём стандартную анимацию
            var animation = new Storyboard();
            var colorAnimation = new ColorAnimation
            {
                From = Colors.Gray,
                To = Colors.LightGray,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            animation.Children.Add(colorAnimation);

            return animation;
        }
    }

}
