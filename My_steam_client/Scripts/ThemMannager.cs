using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace My_steam_client.Scripts
{
    public static class ThemeManager
    {
        public static void ChangeTheme(string themeName)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            for (int i = dictionaries.Count - 1; i >= 0; i--)
            {
                var md = dictionaries[i];
                if (md.Source != null && md.Source.OriginalString.StartsWith("/Themes/"))
                    dictionaries.RemoveAt(i);
            }

            var newDict = new ResourceDictionary();

            switch (themeName)
            {
                case "Light":
                    newDict.Source = new Uri("/Themes/LightTheme.xaml", UriKind.Relative);
                    break;
                case "Dark":
                    newDict.Source = new Uri("/Themes/DarkTheme.xaml", UriKind.Relative);
                    break;
                default:
                    return;
            }

            Application.Current.Resources.MergedDictionaries.Add(newDict);
        }
    }
}
