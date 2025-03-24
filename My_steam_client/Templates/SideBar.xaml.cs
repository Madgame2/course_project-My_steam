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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для SideBar.xaml
    /// </summary>
    [ContentProperty("Children")]
    public partial class SideBar : UserControl
    {

        public UIElementCollection Children => Main_contaner.Children;
        public SideBar()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                var clipGeometry = new RectangleGeometry
                {
                    RadiusX = BorderRadius,
                    RadiusY = BorderRadius,
                    Rect = new Rect(0, 0, ActualWidth, ActualHeight)
                };
                this.Clip = clipGeometry;

                // Подписываемся на изменения размеров
                this.SizeChanged += (sender, args) =>
                {
                    clipGeometry.Rect = new Rect(0, 0, ActualWidth, ActualHeight);
                };
            };

            //this.SizeChanged += SideBar_SizeChanged;
        }
        public double HeightPercent
        {
            get => (double)GetValue(HeightPercentProperty);
            set => SetValue(HeightPercentProperty, value);
        }

        public static readonly DependencyProperty HeightPercentProperty =
            DependencyProperty.Register(
                "HeightPercent",
                typeof(double),
                typeof(SideBar),
                new PropertyMetadata(100.0));


        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.Register(
                nameof(BorderRadius),
                typeof(double),
                typeof(SideBar),
                new PropertyMetadata(50.0));

        public double BorderRadius
        {
            get => (double)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }

    }

}
