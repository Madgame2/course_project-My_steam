﻿using My_steam_client.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Templates
{
    public class PriceFilterViewModel : IFilter, INotifyPropertyChanged
    {
        private decimal? _minPrice;
        private decimal? _maxPrice;

        public decimal? MinPrice
        {
            get => _minPrice;
            set { _minPrice = value; OnPropertyChanged(nameof(MinPrice)); }
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set { _maxPrice = value; OnPropertyChanged(nameof(MaxPrice)); }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<KeyValuePair<string, string>> ToQueryParameters()
        {
            if (_minPrice.HasValue)
                yield return new KeyValuePair<string, string>("minPrice", _minPrice.Value.ToString(CultureInfo.InvariantCulture));

            if (_maxPrice.HasValue)
                yield return new KeyValuePair<string, string>("maxPrice", _maxPrice.Value.ToString(CultureInfo.InvariantCulture));
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
