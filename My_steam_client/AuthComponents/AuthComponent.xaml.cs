using Game_Net;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using My_steam_server.DTO_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_steam_client.AuthComponents
{
    /// <summary>
    /// Логика взаимодействия для AuthComponent.xaml
    /// </summary>
    public partial class AuthComponent : UserControl
    {
        bool _isValidEmail = false;
        bool _isValidPassword = false;

        private AuthWindow _authWindowdow;

        public AuthComponent(AuthWindow window)
        {
            InitializeComponent();
            _authWindowdow = window;
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RegistratoinButton_Click(object sender, RoutedEventArgs e)
        {
            _authWindowdow.swap_to_registration();
        }

        private bool isValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void email_textChanged(object sender, TextChangedEventArgs e)
        {
            string curentText = ((TextBox)sender).Text;

            _isValidEmail = isValidEmail(curentText);

            isValidData();
        }

        private void password_textChanged(object sender, RoutedEventArgs e)
        {
            string password = ((PasswordBox)sender).Password;

            if(string.IsNullOrEmpty(password)) _isValidPassword = false;
            else _isValidPassword = true;

            isValidData();
        }

        private void isValidData()
        {
            LogIn_button.IsEnabled = _isValidPassword && _isValidEmail;
        }

        private async void LogIn_button_Click(object sender, RoutedEventArgs e)
        {
            EmailErrors.Text = string.Empty;
            PasswordErrors.Text = string.Empty;

            var dto = new LoginDto
            {
                Email=EmailBox.Text,
                Password=Password.Password
            };

            var authService = AppServices.Provider.GetRequiredService<Game_Net.AuthService>();



                var result = await authService.LoginAsync(dto);

            if (result.resultCode == Game_Net_DTOLib.ResultCode.WrongPassword)
            {
                PasswordErrors.Text = "Wrong password";
                return;
            }
            else if(result.resultCode == Game_Net_DTOLib.ResultCode.UserNotfound)
            {
                EmailErrors.Text = "No user with such email";
                return;
            }


            AppServices.UserId = result.data.id;
            AppServices.UsserNickNmae = result.data.NickName;
            AppServices.userRole = result.data.UserRole;
            AppServices.UserRegisterDate = result.data.RegisterDate;

            Tokens.TryParse(result.data.tokens, out var tokens);

            if (StayOnline.IsChecked == true)
            {
                TokenStorage.SaveTokens(tokens);
            }

            var manager = AppServices.Provider.GetRequiredService<ComunitationMannageer>();

            manager.JWT_token = tokens.JWT;
            manager.RefrashToken = tokens.Refresh;

            var test = TokenStorage.LoadTokens();

            var mainWindow= new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();


            AppServices.UserId = result.data.id;

            _authWindowdow.Close();
        }
    }
}
