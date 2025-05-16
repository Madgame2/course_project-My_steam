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
        private CreateRedactProjectWindow _editWindow;

        public MyprojectsWindow()
        {
            InitializeComponent();
            HeaderContaner.Content = new Header(this, "My projects");
            var vm = new ProjectsViewModel();
            DataContext = vm;

            vm.OnAddProjectRequested += OpenProjectsWindow;
        }


        private void OpenProjectsWindow()
        {
            if (_editWindow == null || !_editWindow.IsLoaded)
            {
                _editWindow = new CreateRedactProjectWindow("New project");
                _editWindow.Owner = this;
                _editWindow.Closed += (_, _) => _editWindow = null;
                _editWindow.Show();
            }
            else
            {
                _editWindow.Activate();
            }
        }
    }
}
