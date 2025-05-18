using Game_Net;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.AdminComponents.Models;
using My_steam_client.Scripts;
using My_steam_server.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.AdminComponents.ModelsView
{
    internal class GamesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Games> games { get; set; } = new();
        private readonly AdminService adminService;

        public GamesViewModel()
        {
            adminService = AppServices.Provider.GetRequiredService<AdminService>();

            Init();
        }

        private async void Init()
        {

            var Dto = await adminService.GetGames();

            foreach (var user in Dto)
            {
                var newItem = new Games
                {
                    Descritption = user.Descritption,
                    DownloadedSource = user.DownloadedSource,
                    GameId = user.GameId,
                    GameName = user.GameName,
                    HeaderImageSource = user.HeaderImageSource,
                    Price = user.Price,
                    Rating = user.Rating,
                    ReliseDate = user.ReliseDate,
                    UserId = user.UserId,
                };
                games.Add(newItem);
            }

            games.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (Games u in e.NewItems!) u.IsNew = true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }

}
