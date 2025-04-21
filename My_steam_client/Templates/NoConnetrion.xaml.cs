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
    /// Логика взаимодействия для NoConnetrion.xaml
    /// </summary>
    public partial class NoConnetrion : UserControl
    {
        private AuthWindow rootWindow;

        public NoConnetrion(AuthWindow rootWindow)
        {
            InitializeComponent();
            this.rootWindow = rootWindow;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           await rootWindow.startPing();
        }
    }
}
