using Game_Net;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using My_steam_client.Scripts.Interfaces;
using My_steam_client.Scripts.Services;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace My_steam_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //public static IServiceProvider Services { get; private set; }

        public static LocalizationProvider Loc { get; } = new();

        public static void ChangeLanguage(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            Loc.Update();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            ChangeLanguage("en");
            base.OnStartup(e);

            AppServices.Init();

            var comMannager = AppServices.Provider.GetRequiredService<ComunitationMannageer>();

            comMannager.addNewUrl(new ServerSettings { protocol = Protocol.Http, host = "localhost", port = "5254" });


            var authWindow = new AuthWindow();
            authWindow.Show();
        }
    }

}
