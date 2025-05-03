using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Game_Net_DTOLib;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для ProductComponent.xaml
    /// </summary>
    public partial class ProductComponent : UserControl, INotifyPropertyChanged
    {
        public List<PurchaseOption> _PurchaseOptions { get; set; }

        public ProductDto _productDto { get; set; }

        private string _curentImageLink;

        public string curentImageLink
        {
            get => _curentImageLink;
            set
            {
                if (_curentImageLink != value)
                {
                    _curentImageLink = value;
                    OnPropertyChanged(nameof(curentImageLink));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProductComponent(ProductDto dto)
        {
            _productDto = dto;

            InitializeComponent();

            _ImageSlider._sliderImages = InitSlider(_productDto.imagesLinks);
            curentImageLink = _ImageSlider.selectedImage.ImageLink;
            _ImageSlider.ImageChaged += (newImage) => { curentImageLink = newImage.ImageLink; };

            DataContext = this;
            Loaded += ProductComponent_Loaded;
        }

        private ObservableCollection<SliderImage> InitSlider(List<string> imagesSource)
        {
            var result = new ObservableCollection<SliderImage>();

            bool first = true;
            foreach (var image in imagesSource)
            {
                var item = new SliderImage();

                if (first) 
                { 
                    item.isActive = true; 
                    _ImageSlider.selectedImage = item;
                }
                item.ImageLink = image;

                result.Add(item);

                first = false;
            }

            return result;
        }

        private async void ProductComponent_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ProductComponent_Loaded; 
            await webView.EnsureCoreWebView2Async();
            
        }
    }
}
