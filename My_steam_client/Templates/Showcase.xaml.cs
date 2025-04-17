using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            DependencyProperty.Register(nameof(ShowCaseElementBackGround), typeof(Brush),typeof(Showcase), new PropertyMetadata(Brushes.Gray));


        public static readonly DependencyProperty HoverAnimationProperty =
            DependencyProperty.Register(nameof(HoverAnimation),typeof(Storyboard),typeof(Showcase), new PropertyMetadata(null));

        public Storyboard HoverAnimation
        {
            get => (Storyboard)GetValue(HoverAnimationProperty);
            set => SetValue(HoverAnimationProperty, value);
        }

        public Brush ShowCaseElementBackGround
        {
            get => (Brush)GetValue(ShowCaseElementBackGroundProperty);
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
                // Клонируем так, чтобы не переиспользовать один экземпляр несколько раз
                var sb = templateSb.Clone();

                // Присваиваем каждому таймлайну цель — этот бордер
                foreach (Timeline tl in sb.Children)
                    Storyboard.SetTarget(tl, border);

                // Запускаем
                sb.Begin(border, true);

                // Сохраним в Tag, чтобы потом остановить
                border.Tag = sb;
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Tag is Storyboard sb)
            {
                // Останавливаем именно эту инстанцию
                sb.Stop(border);
                border.Tag = null;
            }
        }
    }
}
