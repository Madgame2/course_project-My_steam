using System.Collections.ObjectModel;
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

        private ProductDto _productDto;

        public ProductComponent(ProductDto dto)
        {
            _productDto = dto;

            InitializeComponent();

            _PurchaseOptions = new List<PurchaseOption>
            { 
                new PurchaseOption { GameName = "cat", Price = "15.15" }
            };

            _ImageSlider._sliderImages = new ObservableCollection<SliderImage>
            {
                new SliderImage{ isActive = true, ImageLink="https://localhost:7199/images/test.jpg"},
                 new SliderImage{ isActive = false, ImageLink="https://localhost:7199/images/test.jpg"},
                  new SliderImage{ isActive = false, ImageLink="https://localhost:7199/images/test.jpg"},
                   new SliderImage{ isActive = false, ImageLink="https://localhost:7199/images/test.jpg"},
                    new SliderImage{ isActive = false, ImageLink="https://localhost:7199/images/test.jpg"},
                     new SliderImage{ isActive = false, ImageLink="https://localhost:7199/images/test.jpg"},
                      new SliderImage{ isActive = false, ImageLink="https://localhost:7199/images/test.jpg"}
            };

            _ImageSlider.selectedImage = _ImageSlider._sliderImages[0];
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
