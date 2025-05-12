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
        private string _finalCost = "0.0";


        public ObservableCollection<BasketElemModel> BasketElems { get; set; } = new();
        public string FinalCost
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
            InitializeComponent();
            DataContext = this;

            BasketElems.Add(new BasketElemModel { GameId =0, GameName="test", ImageLink= "https://localhost:7199/images/five_nights_at_Fredys_3/Header.jpg", Price=4.99f });
            BasketElems.Add(new BasketElemModel { GameId = 0, GameName = "test", ImageLink = "https://localhost:7199/images/five_nights_at_Fredys_3/Header.jpg", Price = 4.99f });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is BasketElemModel basketElem)
            {
                BasketElems.Remove(basketElem);
            }
        }
    }
}
