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
    /// Логика взаимодействия для LibraryGamePageComponent.xaml
    /// </summary>
    public partial class LibraryGamePageComponent : UserControl
    {
        public string HeaderImageLink { get; set; }
        public LibraryGamePageComponent()
        {
            HeaderImageLink = "D:\\projects\\cs\\Course_project(my_steam)\\course_project-My_steam\\My_steam_client\\resources\\images\\test.jpg";

            InitializeComponent();
            DataContext = this;

            ButttonRoot.Content = new InstallButton();
            DownloadInfoRoot.Content = new DownLoadInfo();
            InfoRoot.Content = new PlayInfo();
            ActivityRoot.Content = new Activity();
        }
    }
}
