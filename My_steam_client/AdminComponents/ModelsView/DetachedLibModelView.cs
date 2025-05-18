using My_steam_client.AdminComponents.Models;
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

        public DetachedLibModelView()
        {
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
