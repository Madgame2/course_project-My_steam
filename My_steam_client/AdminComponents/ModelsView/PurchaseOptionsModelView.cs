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
    internal class PurchaseOptionsModelView : INotifyPropertyChanged
    {
        public ObservableCollection<PurchseOptions> Options { get; set; } = new();
        private readonly AdminService adminService;

        public PurchaseOptionsModelView()
        {
            adminService = AppServices.Provider.GetRequiredService<AdminService>();

            Init();
        }

        private async void Init()
        {
            var Dto = await adminService.GetPurhcaseOptions();

            foreach (var item in Dto)
            {
                var newItem = new PurchseOptions
                {
                    GameID= item.GameID,
                    imageLinnk= item.imageLinnk,
                    Price= item.Price,
                    PurhcaseId= item.PurhcaseId,
                    PurhcaseName= item.PurhcaseName,
                };
                Options.Add(newItem);
            }


            Options.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (PurchseOptions u in e.NewItems!) u.IsNew = true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
