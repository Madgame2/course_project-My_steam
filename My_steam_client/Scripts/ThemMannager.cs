using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace My_steam_client.Scripts
{
    public enum ThemeType
    {
        Light,
        Dark
    }

    public static class ThemeManager
    {
        public static void ChangeTheme(ThemeType theme)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            // Удаляем все словари тем из ресурсов
            for (int i = dictionaries.Count - 1; i >= 0; i--)
            {
                var md = dictionaries[i];
                if (md.Source != null && md.Source.OriginalString.StartsWith("/Themes/"))
                    dictionaries.RemoveAt(i);
            }

            // Создаем новый словарь в зависимости от выбранной темы
            var newDict = new ResourceDictionary();

            switch (theme)
            {
                case ThemeType.Light:
                    newDict.Source = new Uri("/Themes/LightTheme.xaml", UriKind.Relative);
                    break;

                case ThemeType.Dark:
                    newDict.Source = new Uri("/Themes/DarkTheme.xaml", UriKind.Relative);
                    break;

                default:
                    return;
            }

            Application.Current.Resources.MergedDictionaries.Add(newDict);
        }
    }
}
