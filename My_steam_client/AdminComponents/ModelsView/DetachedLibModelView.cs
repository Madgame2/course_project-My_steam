using Game_Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using My_steam_client.AdminComponents.Models;
using My_steam_client.Scripts;
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
    internal class DetachedLibModelView : INotifyPropertyChanged
    {
        public ObservableCollection<DetachedLib> LibItems { get; set; } = new();
        private readonly AdminService adminService;

        public DetachedLibModelView()
        {
            adminService = AppServices.Provider.GetRequiredService<AdminService>();


            Init();
        }

        private async void Init()
        {
            var Dto = await adminService.GetDetachedLib();

            foreach (var item in Dto)
            {
                var newItem = new DetachedLib
                {
                    GameId = item.GameId,
                    PurchaseDate = item.PurchaseDate,
                    UserId = item.UserId,
                };
                LibItems.Add(newItem);
            }


            LibItems.CollectionChanged += OnCollectionChanged;
        }
        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (DetachedLib u in e.NewItems!) u.IsNew = true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
