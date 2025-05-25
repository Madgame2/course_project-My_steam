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
using System.Windows.Input;

namespace My_steam_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Shop_Component? shopPage = null;
        private LibraryComponent? libraryPage = null;
        private Settings? SettingsWindow= null;

        private Stack<(SideButton?,UserControl)> UndoStack=new();
        private Stack<(SideButton?, UserControl)> RedoStack=new();

        private SideButton? currentButton;

        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }


        private bool CanUndo => UndoStack.Any();
        private bool CanRedo => RedoStack.Any();


        private void Undo()
        {
            if (UndoStack.Count > 1)
            {
                var current = UndoStack.Peek();
                if(current.Item1!=null) current.Item1.IsChecked = true;
                RootContent.Content = current.Item2;
                RedoStack.Push(current);
                UndoStack.Pop();
            }
        }
        private void Rend()
        {
            var current = RedoStack.Peek();
            if (current.Item1 != null) current.Item1.IsChecked = true;
            RootContent.Content = current.Item2;
            UndoStack.Push(current);
            RedoStack.Pop();
        }

        public MainWindow()
        {
            App.ChangeLanguage("en");
            App.Loc.Update();

            InitializeComponent();

            HeaderContaner.Content = new Header(this);

            var libmannager = AppServices.Provider.GetRequiredService<LibMannager>();
            libmannager.SynnchronizeLibs();

            openShopPage();

            UndoCommand = new RelayCommand(Undo, () => CanUndo);
            RedoCommand = new RelayCommand(Rend, () => CanRedo);
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

            RedoStack.Clear();
            NavigateTo(shopPage);
        }

        public void NavigateTo(UserControl userControl)
        {
            UndoStack.Push((currentButton, RootContent.Content as UserControl));
            RootContent.Content = userControl;
        }

        private void toShop(object sender, RoutedEventArgs e)
        {
            if (sender is SideButton button) {
                openShopPage();
                currentButton = button;

            }
        }

        private void toLib(object sender, RoutedEventArgs e)
        {
            if (sender is SideButton button)
            {
                if (libraryPage == null)
                {
                    libraryPage = new LibraryComponent();
                }
                RedoStack.Clear();
                NavigateTo(libraryPage);
                currentButton = button;

            }
        }

        public void ToBasket(object sender, RoutedEventArgs e)
        {
            if (sender is SideButton button)
            {
                var basket = new BasketComponent();
                RedoStack.Clear();
                NavigateTo(basket);
                currentButton = button;

            }
        }

        private void toStat(object sender, RoutedEventArgs e)
        {
            if (sender is SideButton button)
            {
                RedoStack.Clear();
                NavigateTo(null);
                currentButton = button;
            }
        }

        private void toCommunity(object sender, RoutedEventArgs e)
        {
            if (sender is SideButton button)
            {
                RedoStack.Clear();
                NavigateTo(null);
                currentButton = button;

            }
        }

        public void toProductPage(ProductDto dto)
        {
            SideButtonGroup.setAllUncehceked("MainNav");
            var productPage = new ProductComponent(dto);
            RedoStack.Clear();
            NavigateTo(productPage);
            currentButton = null;

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

        private void Settings(object sender, RoutedEventArgs e)
        {
            // Если окно не существует или оно было закрыто — создаём новое
            if (SettingsWindow == null || !SettingsWindow.IsLoaded)
            {
                SettingsWindow = new Settings();
                SettingsWindow.Owner = this; 
                SettingsWindow.Closed += (_, _) => SettingsWindow = null; 
                SettingsWindow.Show();
            }
            else
            {
                SettingsWindow.Activate(); 
            }

            SettingsWindow.Topmost = true;
            SettingsWindow.Topmost = false;
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