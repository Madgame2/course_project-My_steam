using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace My_steam_client.Scripts.Services
{
    public class LocalizationProvider : INotifyPropertyChanged
    {
        private readonly ResourceManager _resourceManager;

        public LocalizationProvider()
        {
            _resourceManager = Resources.ResourceManager;
        }

        public string this[string key]
        {
            get => _resourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture) ?? $"[{key}]";
        }

        public void Update()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
