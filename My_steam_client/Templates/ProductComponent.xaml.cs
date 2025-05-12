using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Game_Net;
using Game_Net_DTOLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using My_steam_client.Scripts;

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


        }

        //private async void WebWiew()
        //{
        //    if (_productDto.MdFileSourcce == null) return;

        //    var service = AppServices.Provider.GetRequiredService<Game_Net.ResourcesService>();
        //    var mdString = await service.GetMarkdownStreamAsync(_productDto.MdFileSourcce);
        //    //markdownViewer.Markdown = mdString;

        //}

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var sevice = AppServices.Provider.GetRequiredService<Game_Net.CartService>();
            var viewModel = (PurchaseOption)((FrameworkElement)sender).DataContext;


            try
            {
                var dto = new AddToCarDto { useerId = AppServices.UserId, purchouseID = viewModel.PurchaseId };
                var result = await sevice.addToCart(dto);


                if (result)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;

                    mainWindow.ToBasket(this, e);
                }
            }
            catch(PurchouseExistExeption)
            {
                MessageBox.Show("This purhcouse aready exist in cart.");
            }

            
        }
    }
}
