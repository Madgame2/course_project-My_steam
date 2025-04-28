using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для SliderComponent.xaml
    /// </summary>
    public partial class SliderComponent : UserControl
    {
        public static readonly DependencyProperty ImageSourceLinkProperty =
         DependencyProperty.Register(nameof(ImageSourceLink), typeof(string), typeof(SliderComponent), new PropertyMetadata("images/no-image.svg"));
        public static readonly DependencyProperty GameNameProperty =
            DependencyProperty.Register(nameof(GameName), typeof(string), typeof(SliderComponent), new PropertyMetadata("Undefinded"));
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(SliderComponent), new PropertyMetadata("No info"));
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register(nameof(Price), typeof(string), typeof(SliderComponent), new PropertyMetadata("no price"));
  


        public int Id { get; set; }
        public string Price
        {
            get => (string)GetValue(PriceProperty);
            set => SetValue(PriceProperty, value);
        }
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public string GameName
        {
            get => (string)GetValue(GameNameProperty);
            set => SetValue(GameNameProperty, value);
        }
        public string ImageSourceLink
        {
            get => (string)GetValue(ImageSourceLinkProperty);
            set => SetValue(ImageSourceLinkProperty, value);
        }

        public SliderComponent()
        {
            InitializeComponent();
            DataContext = this;

            this.Loaded += (s, e) =>
            {
                var clipGeometry = new RectangleGeometry
                {
                    RadiusX = 25,
                    RadiusY = 25,
                    Rect = new Rect(0, 0, ActualWidth, ActualHeight)
                };
                this.Clip = clipGeometry;

                // Подписываемся на изменения размеров
                this.SizeChanged += (sender, args) =>
                {
                    clipGeometry.Rect = new Rect(0, 0, ActualWidth, ActualHeight);
                };
            };
        }
    }
}
