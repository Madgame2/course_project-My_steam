using Game_Net;
using Game_Net_DTOLib;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Controls;
using My_steam_client.Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для Shop_Component.xaml
    /// </summary>
    public partial class Shop_Component : UserControl
    {
        private MainWindow _mainWindow;
        private StoreServices _storeService;


        private bool HasMoreProduct = true;
        private long lastId = 0;
        private string lastQueryString = string.Empty;
        public  Shop_Component(MainWindow window)
        {
            _mainWindow = window;
            _storeService = AppServices.Provider.GetRequiredService<StoreServices>();
            InitSlider();

            InitializeComponent();

            InitShowCase();
        }

        private async void InitShowCase()
        {
            showCase.Clicked += (sender, e) =>
            {
                if (sender is not ShowCaseObject showCaseObj) return;

                ToGamePage(showCaseObj.GameId);
            };

            var baseFilterInfo = new ProductFilterDto {};
            var QueryString = QueryStringBuilder.ToQueryString(baseFilterInfo);

            var result = await _storeService.tryGetProducts(QueryString);

            HasMoreProduct = result.Item1;

            if (result.Item2 != null)
                foreach (var item in result.Item2)
                {
                    var newShowCaseObj = new ShowCaseObject();
                    newShowCaseObj.Title = item.title;
                    newShowCaseObj.Description = item.description;
                    newShowCaseObj.ImageURL = item.headerImageSource;
                    newShowCaseObj.Coast = Convert.ToString(item.price);
                    newShowCaseObj.GameId = item.productId;

                    showCase.addObject(newShowCaseObj);
                    lastId = newShowCaseObj.GameId;
                }

            scroll.ScrollChanged += ScrollViewer_ScrollChanged;
            showCase.FiltersLable.applyedFilters += filtersChanged;
            showCase.ShowMoreClicked += ShowMore;
        }

        private async void InitSlider()
        {
            var objects = await _storeService.GetSliderObjects(16);

            foreach (var obj in objects) { 
                var newComponent = DtoToSliderComponent(obj);


                RecomendSlider.addComponent(newComponent);
            }

            RecomendSlider.SliderInti();
        }

        private SliderComponent DtoToSliderComponent(GameSliderDto dto)
        {
            var newSliderComponent = new SliderComponent();

            newSliderComponent.Id = dto.GameId;

            newSliderComponent.GameName = dto.Name;
            newSliderComponent.Description = dto.Description;
            newSliderComponent.ImageSourceLink = dto.imageLink;
            newSliderComponent.Price = Convert.ToString(dto.price);

            newSliderComponent.Click += (s, e) =>
            {
                if (s is SliderComponent sliderComponent)
                {
                    ToGamePage(sliderComponent.Id);
                }
            };

            return newSliderComponent;
        }

        private async Task ToGamePage(long gameId)
        {
            try
            {
                var service = AppServices.Provider.GetRequiredService<StoreServices>();

                var result = await service.GetGamePageAsync(gameId);

                if(result != null) 
                _mainWindow.toProductPage(result);
            }
            catch (NotFoundExaption) {

                Debug.WriteLine("404 code");

                return;
            }

            
        }

        private async Task AddToShowcase()
        {
            if (HasMoreProduct)
            {
                string filters = showCase.FiltersLable.getQueryFilters();

                var baseFilterInfo = new ProductFilterDto { LastSeenId = lastId };

                var QueryString = QueryStringBuilder.ToQueryString(baseFilterInfo);
                var result = await _storeService.tryGetProducts($"{filters}&{QueryString}");

                HasMoreProduct = result.Item1;
                showCase.canShowMore = HasMoreProduct;

                if (result.Item2 != null)
                    foreach (var item in result.Item2)
                    {
                        var newShowCaseObj = new ShowCaseObject();
                        newShowCaseObj.Title = item.title;
                        newShowCaseObj.Description = item.description;
                        newShowCaseObj.ImageURL = item.headerImageSource;
                        newShowCaseObj.Coast = Convert.ToString(item.price);
                        newShowCaseObj.GameId = item.productId;

                        showCase.addObject(newShowCaseObj);
                        lastId = newShowCaseObj.GameId;
                    }

                lastQueryString = filters;
            }

        }

        private async void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //var scrollViewer = sender as ScrollViewer;

            //if(scrollViewer != null)
            //{
            //    bool isABottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;

            //    if (isABottom)
            //    {
            //        await AddToShowcase();
            //    }
            //}
        }

        private async void ShowMore(object sender, EventArgs e)
        {

            await AddToShowcase();
       
        }

        private async void filtersChanged(object? sender, EventArgs e)
        {
            var sideFilters = sender as SideFilters;
            if (sideFilters == null) return;

            var filtersQureu = sideFilters.getQueryFilters();

            if (lastQueryString == filtersQureu) return;

            HasMoreProduct = true;
            lastId = 0;
            showCase.Items.Clear();

            await AddToShowcase();
        }
    }
}
