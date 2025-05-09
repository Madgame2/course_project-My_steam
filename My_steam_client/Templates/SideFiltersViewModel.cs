using My_steam_client.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Templates
{
    internal class SideFiltersViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<IFilter> Filters { get; set; } = new();

        public SideFiltersViewModel()
        {
            Filters.Add(new SearchFilterViewModel());
            Filters.Add(new PriceFilterViewModel());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
