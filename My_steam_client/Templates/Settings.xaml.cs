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
using System.Windows.Shapes;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private AppSetingsPage appSetingsPage=new AppSetingsPage();
        public Settings()
        {
            DataContext = App.Loc;

            InitializeComponent();
            SettingsRoot.Content = appSetingsPage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsRoot.Content = appSetingsPage;
        }
    }
}
