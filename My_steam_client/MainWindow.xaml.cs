using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using My_steam_client.Templates;

namespace My_steam_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Shop_Component? shopPage = null;

        public MainWindow()
        {
            InitializeComponent();
            HeaderContaner.Content = new Header(this);

            openShopPage();
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
            NavigateTo(null);
        }

        private void toStat(object sender, RoutedEventArgs e)
        {
            NavigateTo(null);
        }

        private void toCommunity(object sender, RoutedEventArgs e)
        {
            NavigateTo(null);
        }
    }
}