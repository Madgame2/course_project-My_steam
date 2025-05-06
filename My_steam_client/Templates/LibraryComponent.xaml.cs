using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для LibraryComponent.xaml
    /// </summary>
    public partial class LibraryComponent : UserControl
    {

        public ObservableCollection<LibraryListItem> Library { get; set; }

        public LibraryListItem selecteditem { get; set; }

        private LibMannager _LibMannager;
        public LibraryComponent()
        {
            _LibMannager = AppServices.Provider.GetRequiredService<LibMannager>();

            InitializeComponent();
            DataContext = this;

            Library = new ObservableCollection<LibraryListItem>
            {
                new LibraryListItem{GameName="testName",
                ImageLink="D:\\projects\\cs\\Course_project(my_steam)\\course_project-My_steam\\My_steam_client\\resources\\images\\test.jpg"}
            };
        }


        private void OnItemClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border) return;

            // Получаем текущий элемент данных
            var currentItem = border.DataContext as LibraryListItem;
            if (currentItem == null) return;

            if (selecteditem != null)
            {
                selecteditem.isActive = false;
                
            }

            selecteditem = currentItem;
            currentItem.isActive = true;


            Root.Content = new LibraryGamePageComponent();
        }
    }
}
