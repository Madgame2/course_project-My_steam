using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace My_steam_client.Converters
{
    class BrushAnimation:AnimationTimeline
    {

        public override Type TargetPropertyType => typeof(Brush);

        public Brush From
        {
            get => (Brush)GetValue(FromProperty);
            set => SetValue(FromProperty,value);
        }

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(Brush), typeof(BrushAnimation));


        public Brush To
        {
            get => (Brush)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(Brush), typeof(BrushAnimation));


        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (From is SolidColorBrush fromBrush && To is SolidColorBrush toBrush && animationClock.CurrentProgress.HasValue)
            {
                double progress = animationClock.CurrentProgress.Value;

                Color fromColor = fromBrush.Color;
                Color toColor = toBrush.Color;

                byte a = (byte)(fromColor.A + (toColor.A - fromColor.A) * progress);
                byte r = (byte)(fromColor.R + (toColor.R - fromColor.R) * progress);
                byte g = (byte)(fromColor.G + (toColor.G - fromColor.G) * progress);
                byte b = (byte)(fromColor.B + (toColor.B - fromColor.B) * progress);

                return new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }

            return From;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BrushAnimation();
        }
    }
}
