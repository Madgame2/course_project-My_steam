using Game_Net;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Controls;

namespace My_steam_client.AdminComponents.ModelsView
{
    internal class UserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; } = new();
        private AdminService adminService;

        public UserViewModel()
        {
            adminService = AppServices.Provider.GetRequiredService<AdminService>();

            

            Init();
        }

        private async void Init()
        {
            var Dto = await adminService.GetUsers();

            foreach (var user in Dto)
            {
                var newItem = new User
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = user.Role,
                    registerDate = user.registerDate,
                    UserId = user.UserId
                };
                Users.Add(newItem);
            }


            Users.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (User u in e.NewItems!) u.IsNew = true;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
