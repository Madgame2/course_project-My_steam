using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для Showcase.xaml
    /// </summary>
    public partial class Showcase : UserControl
    {
        public static readonly DependencyProperty ElementHeightProperty =
            DependencyProperty.Register(nameof(ElementHeight),typeof(double),typeof(Showcase),new PropertyMetadata(150.0));
        public static readonly DependencyProperty ElementMarginProperty =
            DependencyProperty.Register(nameof(ElementMargin),typeof(Thickness),typeof(Showcase), new PropertyMetadata(new Thickness(0)));
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.Register(nameof(BorderRadius),typeof(CornerRadius),typeof(Showcase),new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty ShowCaseElementBackGroundProperty =
            DependencyProperty.Register(nameof(ShowCaseElementBackGround), typeof(Color),typeof(Showcase), new PropertyMetadata(Colors.Gray));


        public static readonly DependencyProperty HoverAnimationProperty =
            DependencyProperty.Register(nameof(HoverAnimation),typeof(Storyboard),typeof(Showcase), new PropertyMetadata(null));

        public Storyboard HoverAnimation
        {
            get => (Storyboard)GetValue(HoverAnimationProperty);
            set => SetValue(HoverAnimationProperty, value);
        }

        public Color ShowCaseElementBackGround
        {
            get => (Color)GetValue(ShowCaseElementBackGroundProperty);
            set => SetValue(ShowCaseElementBackGroundProperty, value);
        }
        public CornerRadius BorderRadius
        {
            get => (CornerRadius)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }
        public Thickness ElementMargin
        {
            get => (Thickness)GetValue(ElementMarginProperty);
            set => SetValue(ElementMarginProperty, value);
        }
        public double ElementHeight
        {
            get => (double)GetValue(ElementHeightProperty);
            set => SetValue(ElementHeightProperty, value);
        }

        public ObservableCollection<ShowCaseObject> _items { get; } = new();

        public Showcase()
        {
            InitializeComponent();
        }

        public void addObject(ShowCaseObject obj) { 
          _items.Add(obj);
        }


        public void addObject(IEnumerable<ShowCaseObject> objects) {
            foreach (var obj in objects) { 
                _items.Add((ShowCaseObject)obj);
            }
        }



        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && HoverAnimation is Storyboard templateSb)
            {
                if (border.Background is SolidColorBrush scb)
                {
                    // Убедимся, что кисть не заморожена
                    if (scb.IsFrozen)
                    {
                        scb = scb.Clone();
                        border.Background = scb;
                    }

                    var originalColor = scb.Color;

                    var sb = templateSb.Clone();
                    foreach (var tl in sb.Children)
                        Storyboard.SetTarget(tl, border);

                    sb.Begin(border, true);

                    border.Tag = new BorderHoverInfo
                    {
                        HoverStoryboard = sb,
                        OriginalColor = originalColor
                    };
                }
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Tag is BorderHoverInfo info)
            {
                info.HoverStoryboard?.Stop(border);

                // Достаём UnhoverAnimation из ресурсов
                if (TryFindResource("UnhoverAnimation") is Storyboard unhoverSb)
                {
                    var sb = unhoverSb.Clone();

                    // Устанавливаем оригинальный цвет как цель To
                    if (sb.Children[0] is ColorAnimation colorAnim)
                    {
                        colorAnim.To = info.OriginalColor;
                    }

                    Storyboard.SetTarget(sb.Children[0], border);
                    sb.Begin(border, true);
                }

                border.Tag = null;
            }
        }

    }
}
