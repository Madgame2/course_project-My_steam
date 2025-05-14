using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    public partial class Showcase : UserControl, INotifyPropertyChanged
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public ObservableCollection<ShowCaseObject> Items { get; set; } = new();

        public event EventHandler Clicked;
        public event EventHandler ShowMoreClicked;
        public event PropertyChangedEventHandler? PropertyChanged;

        public static DependencyProperty canShowModeProperty =
            DependencyProperty.Register(nameof(canShowMore), typeof(bool), typeof(Showcase), new PropertyMetadata(false));

        public bool canShowMore {
            get=>(bool) GetValue(canShowModeProperty);
            set=> SetValue(canShowModeProperty, value);
        }

        public Showcase()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void addObject(ShowCaseObject obj) {
            Items.Add(obj);
        }


        public void addObject(IEnumerable<ShowCaseObject> objects) {
            foreach (var obj in objects) {
                Items.Add((ShowCaseObject)obj);
            }
        }



        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Background is SolidColorBrush scb)
                {
                    if (scb.IsFrozen)
                    {
                        scb = scb.Clone();
                        border.Background = scb;
                    }

                    var originalColor = scb.Color;

                    // Прямо останавливаем прошлую анимацию
                    scb.BeginAnimation(SolidColorBrush.ColorProperty, null);

                    var animation = new ColorAnimation
                    {
                        To = Colors.DimGray,
                        Duration = TimeSpan.FromSeconds(0.15),
                        FillBehavior = FillBehavior.HoldEnd
                    };

                    scb.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                    border.Tag = originalColor;
                }
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Tag is Color originalColor && border.Background is SolidColorBrush scb)
            {
                // Прямо останавливаем прошлую анимацию
                scb.BeginAnimation(SolidColorBrush.ColorProperty, null);

                var animation = new ColorAnimation
                {
                    To = originalColor,
                    Duration = TimeSpan.FromSeconds(0.3),
                    FillBehavior = FillBehavior.HoldEnd
                };

                scb.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                border.Tag = null;
            }
        }




        private void OnItemClicked(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var dataContext = border?.DataContext as ShowCaseObject;


            if (dataContext != null)
            {
                Clicked?.Invoke(dataContext, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowMoreClicked?.Invoke(this, e);
        }
    }
}
