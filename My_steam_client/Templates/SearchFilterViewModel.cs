using My_steam_client.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Templates
{
    internal class SearchFilterViewModel : INotifyPropertyChanged, IFilter
    {
        public string? searchLable;


        public string? SearchLable
        {
            get => searchLable;
            set { searchLable = value; OnPropertyChanged(nameof(SearchLable)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<KeyValuePair<string, string>> ToQueryParameters()
        {
            if(!string.IsNullOrEmpty(searchLable))
                yield return new KeyValuePair<string, string>("Search",searchLable);
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
