using Game_Net;
using Game_Net_DTOLib;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для BasketComponent.xaml
    /// </summary>
    public partial class BasketComponent : INotifyPropertyChanged
    {
        private float _finalCost = 0.0f;
        private readonly CartService _cartService;

        public ObservableCollection<BasketElemModel> BasketElems { get; set; } = new();
        public float FinalCost
        {
            get => _finalCost;
            set
            {
                if (_finalCost != value)
                {
                    _finalCost = value;
                    OnPropertyChanged();
                }
            }
        }

        public BasketComponent()
        {
            _cartService = AppServices.Provider.GetRequiredService<CartService>();

            InitializeComponent();
            DataContext = this;


            InitCart();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void InitCart()
        {
            var Items = await _cartService.getCart(AppServices.UserId);

            foreach (var Item in Items)
            {

                FinalCost += Item.Price;
                var newElem= DtoToModel(Item);
                BasketElems.Add(newElem);
            }
        }

        private BasketElemModel DtoToModel(CartItemDto dto)
        {
            return new BasketElemModel { GameName = dto.purchouseNmae, ImageLink = dto.ImageLink, Price = dto.Price, cartitemId = dto.CarItemtId };
        }

        private async void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is BasketElemModel basketElem)
            {

                var dto = new DeleteFromCartDTO { UserId=AppServices.UserId, CartId = basketElem.cartitemId };
                var result = await _cartService.DeleteCartElem(dto);

                if (result)
                {
                    FinalCost -= basketElem.Price;
                    BasketElems.Remove(basketElem);
                }
                else
                {
                    MessageBox.Show("Server error, failed aatempt to delete element");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
