using My_steam_client.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Templates
{
    public class ThemeItem : INotifyPropertyChanged
    {
        public ThemeType Type { get; }

        public ThemeItem(ThemeType type)
        {
            Type = type;
        }

        public string DisplayName => Type switch
        {
            ThemeType.Light => Resources.ThemeLight,
            ThemeType.Dark => Resources.ThemeDark,
            _ => "[Unknown]"
        };

        public override string ToString()
        {
            return DisplayName;
        }

        public void RaiseNameChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
