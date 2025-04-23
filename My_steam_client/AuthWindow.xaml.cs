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
            else Root.Content = authComponent;
        }
    }
}
