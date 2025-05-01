using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Templates
{
    public class SliderImage : INotifyPropertyChanged
    {
        private string _imageLink;
        public string ImageLink
        {
            get { return _imageLink; }
            set
            {
                if (_imageLink != value)
                {
                    _imageLink = value;
                    OnPropertyChanged(nameof(ImageLink));
                }
            }
        }

        private bool _isActive;
        public bool isActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(isActive)); // Уведомляем об изменении isActive
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
