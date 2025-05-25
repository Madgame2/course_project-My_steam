using My_steam_client.Scripts;
using System.Windows.Controls;
using My_steam_client;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для AppSetingsPage.xaml
    /// </summary>
    public partial class AppSetingsPage : UserControl
    {
        public AppSetingsPage()
        {
            InitializeComponent();

            var themes = new List<ThemeItem>()
                {
                    new ThemeItem(ThemeType.Light),  // локализованные строки
                    new ThemeItem(ThemeType.Dark)
                };

            ThemeSelector.ItemsSource = themes;

            ThemeSelector.SelectedItem = themes.First(t => t.Type == ThemeType.Dark);
        }

        private bool _isThemeInitialized = false;
        private bool _isLangInitialized = false;

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isThemeInitialized)
            {
                _isThemeInitialized = true;
                return;
            }

            if (ThemeSelector.SelectedItem is ThemeItem item)
            {
                ThemeManager.ChangeTheme(item.Type);
            }
        }

        private void LangSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLangInitialized)
            {
                _isLangInitialized = true;
                return;
            }


            if (LanguageComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem selectedItem)
            {
                string cultureCode = selectedItem.Tag.ToString();
                App.ChangeLanguage(cultureCode);
            }
        }
    }
}
