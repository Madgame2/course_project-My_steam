using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using My_steam_client.Controls;
using System.Windows.Input;

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
            DependencyProperty.Register(nameof(VisibleElements),typeof(int),typeof(Slider),new PropertyMetadata(4));




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

        public ICommand selectPageCommand => new RelayCommand<int>(OnPageSelected);


        private int _currentPage = 0;

        public Slider()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnPageSelected(int index)
        {
            _currentPage = index;
            UpdateVisibleComponents();
            UpdateIndicators();
        }

        private void UpdateVisibleComponents()
        {

        }

        private void UpdateIndicators()
        {
            PageIndicators.Clear();

            int total = Childrens.Count;
            int pageCounts = (int)Math.Ceiling((double)total / VisibleElements);


            for (int i = 0; i < pageCounts; i++)
            {
                PageIndicators.Add(new PageIndicator { Index = i, isActive = i == _currentPage });
            }
        }

        private void SliderLoaded(object sender, RoutedEventArgs e)
        {
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
    }
}
