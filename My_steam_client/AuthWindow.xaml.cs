using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.AuthComponents;
using My_steam_client.Scripts;
using My_steam_client.Scripts.Interfaces;
using My_steam_client.Templates;
using Game_Net;
using System.Net;
using System.Net.Http;

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private PingPage pingPage = new PingPage();
        private NoConnetrion noConnetrion;
        private RegistrationComponent registrationComponent;
        private AuthComponent authComponent;

        public AuthWindow()
        {
            InitializeComponent();
            noConnetrion = new NoConnetrion(this);
            registrationComponent = new RegistrationComponent(this);
            authComponent = new AuthComponent(this);

            _ = startPing();
        }

        public void swap_to_registration()
        {
            Root.Content = registrationComponent;
        }
        public void swap_to_authorization()
        {
            Root.Content = authComponent;

        }

        public async Task startPing()
        {
            Root.Content = pingPage;

            var pingService = AppServices.Provider.GetRequiredService<IPingService_client>();

            var result = await pingService.PingAsync();


            if (!result) Root.Content = noConnetrion;
            else 
            {
                await TryAuthorization();
                Root.Content = authComponent; 
            }
        }

        private void SwapToMainWindow()
        {

            if (AppServices.userRole != Game_Net_DTOLib.UserRole.Admin)
            {
                var mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                var mainWindow = new AdminWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }


            this.Close();
        }

        private async Task TryAuthorization()
        {
            var tokens = TokenStorage.LoadTokens();

            if (tokens == null) return;

            var service = AppServices.Provider.GetRequiredService<AuthService>();
            var manager = AppServices.Provider.GetRequiredService<ComunitationMannageer>();

            try
            {
                manager.JWT_token = tokens.JWT;
                manager.RefrashToken = tokens.Refresh;

                var result = await service.isValid_JWT_Token();

                if (result.Success)
                {

                    AppServices.UserId = result.data.id;
                    AppServices.UsserNickNmae = result.data.NickName;
                    AppServices.userRole = result.data.UserRole;
                    AppServices.UserRegisterDate = result.data.RegisterDate;

                    SwapToMainWindow();
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    var result = await service.sendRefrashTokenAsync();

                    if (result.Success)
                    {
                        Tokens.TryParse(result.data.tokens, out tokens);

                        manager.JWT_token = tokens.JWT;
                        manager.RefrashToken = tokens.Refresh;
                        TokenStorage.SaveTokens(tokens);

                        AppServices.UserId = result.data.id;
                        AppServices.UsserNickNmae = result.data.NickName;
                        AppServices.userRole = result.data.UserRole;
                        AppServices.UserRegisterDate = result.data.RegisterDate;

                        SwapToMainWindow();
                    }
                }
                catch (HttpRequestException ex1) when (ex1.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return;
                }
            }

        }
    }
}
