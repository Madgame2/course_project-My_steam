using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using My_steam_client.Scripts.Models;
using My_steam_client.Scripts.Services;
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

            // Инициализация коллекции пустой — чтобы привязка работала
            Library = new ObservableCollection<LibraryListItem>();

            // Асинхронная загрузка данных
            LoadLibraryAsync();
        }


        private async void LoadLibraryAsync()
        {
            //await _LibMannager.repository.AddRecordAsync(new ManifestRecord
            //{
            //    UserId = AppServices.UserId,
            //    GameId = 1,
            //    GameName = "test",
            //    LibIconSource = "images\\cat2.jpg",
            //    HeaderImageSource = "images\\cat2.jpg",
            //    ExecuteFileSource = "some sapce",
            //    SpaceRequered = 1024,
            //    lastPlayed = DateTime.Now,
            //    playedTime = TimeSpan.FromMinutes(1000)
            //});

            var libItems = await _LibMannager.getLibAsync();

            foreach (var item in libItems)
                Library.Add(item);
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


            Root.Content = new LibraryGamePageComponent(currentItem.RecordId);
        }
    }
}
