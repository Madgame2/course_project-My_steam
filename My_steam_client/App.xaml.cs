using Game_Net;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using My_steam_client.Scripts.Interfaces;
using My_steam_client.Scripts.Services;
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

        //public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppServices.Init();

            var comMannager = AppServices.Provider.GetRequiredService<ComunitationMannageer>();

            comMannager.addNewUrl(new ServerSettings { protocol = Protocol.Http, host = "localhost", port = "5254" });


            var authWindow = new AuthWindow();
            authWindow.Show();
        }
    }

}
