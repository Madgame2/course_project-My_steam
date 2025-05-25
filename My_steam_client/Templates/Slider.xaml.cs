using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using My_steam_client.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Animation;
using My_steam_client.Converters;
using System.ComponentModel;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для Slider.xaml
    /// </summary>

    [ContentProperty("Childrens")]
    public partial class Slider : UserControl
    {
        public static readonly DependencyProperty PrecentWidthProperty =
            DependencyProperty.Register(nameof(PrecentWidth), typeof(double), typeof(Slider), new PropertyMetadata(100.0));

        public static readonly DependencyProperty PrecentHeightProperty =
            DependencyProperty.Register(nameof(PrecentHeight), typeof(double), typeof(Slider), new PropertyMetadata(100.0));

        public static readonly DependencyProperty StackModeProperty =
            DependencyProperty.Register(nameof(StackMode), typeof(Orientation), typeof(Slider), new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty ComponentWidthProperty =
            DependencyProperty.Register(nameof(ComponentWidth),typeof(double),typeof(Slider),new PropertyMetadata(100.0));

        public static readonly DependencyProperty CompoentHeightProperty =
            DependencyProperty.Register(nameof(CompoentHeight),typeof(double),typeof(Slider),new PropertyMetadata(300.0));

        public static readonly DependencyProperty ComponetnsMarginProperty =
            DependencyProperty.Register(nameof(ComponetnsMargin),typeof(Thickness),typeof(Slider),new PropertyMetadata(new Thickness(10,0,10,0)));

        public static readonly DependencyProperty VisibleElementsProperty =
            DependencyProperty.Register(
                nameof(VisibleElements),
                typeof(int),
                typeof(Slider),
                new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceVisibleElements),
                ValidateVisibleElements);

        public static readonly DependencyProperty IndicatorsActiveBrushProperty =
            DependencyProperty.Register("IndicatorsActiveBrush", typeof(Brush), typeof(Slider), new PropertyMetadata(Brushes.DodgerBlue));

        public static readonly DependencyProperty IndicatorsInactiveBrushProperty =
            DependencyProperty.Register("IndicatorsInactiveBrush", typeof(Brush), typeof(Slider), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty IndicatorsHoverBrushProperty =
            DependencyProperty.Register(nameof(IndicatorsHoverBrush), typeof(Brush), typeof(Slider), new PropertyMetadata(Brushes.Gray));


        private static bool ValidateVisibleElements(object value)
        {
            int val = (int)value;
            return val > 0; // нельзя установить 0 или меньше
        }

        private static object CoerceVisibleElements(DependencyObject d, object baseValue)
        {
            int val = (int)baseValue;
            if (val > 10) return 10; // максимум 10
            return val;
        }


        public Brush IndicatorsHoverBrush
        {
            get=>(Brush)GetValue(IndicatorsHoverBrushProperty);
            set=>SetValue(IndicatorsHoverBrushProperty, value);
        }
        public Brush IndicatorsActiveBrush
        {
            get => (Brush)GetValue(IndicatorsActiveBrushProperty);
            set => SetValue(IndicatorsActiveBrushProperty, value);
        }

        public Brush IndicatorsInactiveBrush
        {
            get => (Brush)GetValue(IndicatorsInactiveBrushProperty);
            set => SetValue(IndicatorsInactiveBrushProperty, value);
        }

        public int VisibleElements
        {
            get => (int)GetValue(VisibleElementsProperty);
            set => SetValue(VisibleElementsProperty, value);
        }
        public Thickness ComponetnsMargin
        {
            get => (Thickness)GetValue(ComponetnsMarginProperty);
            set => SetValue(ComponetnsMarginProperty, value);
        }

        public double ComponentWidth
        {
            get => (double)GetValue(ComponentWidthProperty);
            set => SetValue(ComponentWidthProperty, value);
        }

        public double CompoentHeight
        {
            get => (double)GetValue(CompoentHeightProperty);
            set => SetValue (CompoentHeightProperty, value);
        }

        public Orientation StackMode
        {
            get => (Orientation)GetValue(StackModeProperty);
            set => SetValue(StackModeProperty, value);
        }

        public double PrecentHeight
        {
            get => (double)GetValue(PrecentHeightProperty);
            set => SetValue(PrecentHeightProperty, value);
        }

        public double PrecentWidth
        {
            get => (double)GetValue(PrecentWidthProperty);
            set => SetValue(PrecentWidthProperty, value);
        }


        public UIElementCollection Childrens => Stack_contaner.Children;

        public ObservableCollection<PageIndicator> PageIndicators { get; set; } = new();

        public ICommand selectPageCommand { get; }


        private int _currentPage = 0;

        private int pageCounts=0;

        public Slider()
        {
            selectPageCommand = new RelayCommand<int>(OnPageSelected);

            InitializeComponent();
            DataContext = this;
        }

        public void OnPageSelected(int index)
        {
            _currentPage = index;
            UpdateVisibleComponents();
            UpdateIndicators();
        }

        private void UpdateVisibleComponents()
        {
            Debug.WriteLine(_currentPage);

            double offSetX = -1 * _currentPage * (VisibleElements * (ComponentWidth + ComponetnsMargin.Left + ComponetnsMargin.Right));

            if (Stack_contaner.RenderTransform is not TranslateTransform transform)
            {
                transform = new TranslateTransform();
                Stack_contaner.RenderTransform = transform;
            }


            var animation = new DoubleAnimation
            {
                To = offSetX,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void UpdateIndicators()
        {
            PageIndicators.Clear();


            for (int i = 0; i < pageCounts; i++)
            {
                PageIndicators.Add(new PageIndicator { Index = i, isActive = i == _currentPage });
            }
        }

        public void ArrangeComponents()
        {
            double currentX = 0;

            foreach (var child in Childrens)
            {
                if (child is SliderComponent component)
                {
                    component.Width = ComponentWidth;
                    component.Height = CompoentHeight;
                    component.Margin = new Thickness(0);

                    Canvas.SetLeft(component, currentX);
                    Canvas.SetTop(component, 0);

                    currentX += ComponentWidth + ComponetnsMargin.Left + ComponetnsMargin.Right;
                }
            }
        }

        private void SwipeToRight(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            if (_currentPage >= pageCounts)
            {
                _currentPage = 0;
            }

            UpdateVisibleComponents();
            UpdateIndicators();
        }
        private void SwipeToLeft(object sender, RoutedEventArgs e)
        {
            _currentPage--;
            if (_currentPage <0)
            {
                _currentPage = pageCounts-1;
            }

            UpdateVisibleComponents();
            UpdateIndicators();
        }


        public void SliderInti()
        {

            int total = Childrens.Count;
            pageCounts = (int)Math.Ceiling((double)total / VisibleElements);


            ArrangeComponents();

            double visibleAreaWidht = VisibleElements * (ComponentWidth + ComponetnsMargin.Left+ ComponetnsMargin.Right);

            VisibleArea.Width = visibleAreaWidht;

            SlideBar.ColumnDefinitions[1].Width = new GridLength(visibleAreaWidht);
            MainGrid.RowDefinitions[0].Height = new GridLength(CompoentHeight + ComponetnsMargin.Top + ComponetnsMargin.Bottom);

            foreach (var child in Childrens) {

                if (child is SliderComponent component) {
                    component.Width = ComponentWidth;
                    component.Height = CompoentHeight;
                    component.Margin = ComponetnsMargin;
                }
            }

            OnPageSelected(0);
        }


        private void IndicatorButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button && button.Template.FindName("border", button) is Border border)
            {
                Brush fromBrush = border.Background;
                Brush toBrush = IndicatorsHoverBrush;

                var anim = new BrushAnimation
                {
                    From = fromBrush,
                    To = toBrush,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100))
                };

                border.BeginAnimation(Border.BackgroundProperty, anim);
            }
        }

        private void IndicatorButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button && button.Template.FindName("border", button) is Border border)
            {
                Brush fromBrush = border.Background;
                // Опять пересчитай оригинальный цвет через MultiBinding, или задавай свой явно
                Brush toBrush = (Brush)button.GetValue(Button.BackgroundProperty);

                var anim = new BrushAnimation
                {
                    From = fromBrush,
                    To = toBrush,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100))
                };

                border.BeginAnimation(Border.BackgroundProperty, anim);
            }
        }
        public void addComponent(SliderComponent sliderComponent)
        {
            sliderComponent.Width = ComponentWidth;
            sliderComponent.Height = CompoentHeight;
            sliderComponent.Margin = ComponetnsMargin;

            Stack_contaner.Children.Add(sliderComponent);
        }
    }
}
