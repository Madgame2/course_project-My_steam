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
    /// Логика взаимодействия для Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {

        public string TitleText { get; set; } = "Заголовок окна";
        private Window connntectedWidnow;
        public Header(Window thisWindow, string title= "Заголовок окна")
        {
            InitializeComponent();
            DataContext = this;

            TitleText=title;
            connntectedWidnow = thisWindow;
            
        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            connntectedWidnow.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            connntectedWidnow.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            connntectedWidnow.WindowState = connntectedWidnow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                connntectedWidnow.DragMove();
        }
    }
}
