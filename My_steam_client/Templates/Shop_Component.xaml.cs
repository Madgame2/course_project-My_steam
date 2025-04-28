using Game_Net;
using Game_Net_DTOLib;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using System;
using System.Collections.Generic;
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

        public  Shop_Component(MainWindow window)
        {
            _mainWindow = window;
            _storeService = AppServices.Provider.GetRequiredService<StoreServices>();
            InitSlider();

            InitializeComponent();





            showCase.addObject(new ShowCaseObject());
            showCase.addObject(new ShowCaseObject());
        }

        private async Task InitSlider()
        {
            var objects = await _storeService.GetSliderObjects(16);

            foreach (var obj in objects) { 
                var newComponent = DtoToSliderComponent(obj);

                RecomendSlider.addComponent(newComponent);
            }

            RecomendSlider.ArrangeComponents();
        }

        private SliderComponent DtoToSliderComponent(GameSliderDto dto)
        {
            var newSliderComponent = new SliderComponent();

            newSliderComponent.Id = dto.GameId;

            newSliderComponent.GameName = dto.Name;
            newSliderComponent.Description = dto.Description;
            newSliderComponent.ImageSourceLink = dto.imageLink;
            newSliderComponent.Price = Convert.ToString(dto.price);


            return newSliderComponent;
        }
    }
}
