using System.Windows;
using System.Windows.Controls;
using My_steam_client.Controls;
using My_steam_client.Templates;
using Game_Net_DTOLib;
using My_steam_client.Scripts;
using Microsoft.Extensions.DependencyInjection;
using My_steam_server.Services;
using System.Security.Policy;
using System.Windows.Media;

namespace My_steam_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Shop_Component? shopPage = null;
        private LibraryComponent? libraryPage = null;


        public MainWindow()
        {
            InitializeComponent();
            HeaderContaner.Content = new Header(this);

             var libmannager = AppServices.Provider.GetRequiredService<LibMannager>();
            libmannager.SynnchronizeLibs();

            openShopPage();
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppServices.userRole == UserRole.General)
            {
                var button = FindVisualChildren<SimpleSideButton>(this)
                    .FirstOrDefault(b => (string)b.Tag == "ProjectsButton");

                if (button != null)
                {
                    button.IsEnabled = false;
                    button.Visibility = Visibility.Collapsed;
                }
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                        yield return t;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private void openShopPage()
        {
            if (shopPage == null)
            {
                shopPage = new Shop_Component(this);
            }

            NavigateTo(shopPage);
        }

        public void NavigateTo(UserControl userControl)
        {
            RootContent.Content = userControl;
        }

        private void toShop(object sender, RoutedEventArgs e)
        {
            openShopPage();
        }

        private void toLib(object sender, RoutedEventArgs e)
        {
            if (libraryPage == null) {
                libraryPage = new LibraryComponent();
            }

            NavigateTo(libraryPage);
        }

        public void ToBasket(object sender, RoutedEventArgs e)
        {
            NavigateTo(new BasketComponent());
        }

        private void toStat(object sender, RoutedEventArgs e)
        {
            NavigateTo(null);
        }

        private void toCommunity(object sender, RoutedEventArgs e)
        {
            NavigateTo(null);
        }

        public void toProductPage(ProductDto dto)
        {
            SideButtonGroup.setAllUncehceked("MainNav");
            NavigateTo(new ProductComponent(dto));
        }

        private void MyAccount(object sender, RoutedEventArgs e)
        {
            var aacountWindow = new MyAccountWindow();
            aacountWindow.Show();
        }

        private void Myprojects(object sender, RoutedEventArgs e)
        {
            var myProjects = new MyprojectsWindow();
            myProjects.Show();
        }

        private async void LogOut (object sender,RoutedEventArgs e)
        {
            var dialog = new YesNoDialog("Confirm Log out", "LOG OUT", "This will log out you from your account. You will nead re-enter your Email and password.");
            bool? dialogResult = dialog.ShowDialog();

            bool userResult = dialog.Result;

            if (userResult)
            {
                var auth_service = AppServices.Provider.GetRequiredService<Game_Net.AuthService>();
                var result = await auth_service.LogOutAsync();

                if (result.Success)
                {
                    TokenStorage.DeleteTokens();

                    var authWindow = new AuthWindow();
                    Application.Current.MainWindow = authWindow;
                    this.Close();

                    authWindow.Show();
                }
                else
                {
                    MessageBox.Show("Unadebel to Log out");
                }
            }
        }
    }
}