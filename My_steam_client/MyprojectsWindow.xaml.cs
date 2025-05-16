using My_steam_client.Templates;
using My_steam_client.ViewModels;
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

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для MyprojectsWindow.xaml
    /// </summary>
    public partial class MyprojectsWindow : Window
    {
        public MyprojectsWindow()
        {
            InitializeComponent();
            HeaderContaner.Content = new Header(this, "My projects");
            DataContext = new ProjectsViewModel();
        }
    }
}
