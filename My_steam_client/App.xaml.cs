using System.Configuration;
using System.Data;
using System.Windows;

namespace My_steam_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var authWindow = new AuthWindow();
            authWindow.Show();
        }
    }

}
