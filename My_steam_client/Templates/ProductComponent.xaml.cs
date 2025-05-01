using System.Windows;
using System.Windows.Controls;
using Game_Net_DTOLib;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для ProductComponent.xaml
    /// </summary>
    public partial class ProductComponent : UserControl
    {
        public List<PurchaseOption> _PurchaseOptions { get; set; }

        public ProductComponent()
        {
            InitializeComponent();

            _PurchaseOptions = new List<PurchaseOption>
            { 
                new PurchaseOption { GameName = "cat", Price = "15.15" }
            };

            DataContext = this;
            Loaded += ProductComponent_Loaded;
        }

        private async void ProductComponent_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ProductComponent_Loaded; 
            await webView.EnsureCoreWebView2Async();
            
        }
    }
}
