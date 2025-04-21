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

        public AuthComponent()
        {
            InitializeComponent();
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
    }
}
