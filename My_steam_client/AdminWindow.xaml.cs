using Microsoft.AspNetCore.Mvc;
using My_steam_client.AdminComponents.Templates;
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
using System.Windows.Shapes;

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private GoodsDBComponent? goodspage = null;
        private PurchaseOptionsDBConmponent? PurchaseOptionsPage = null;
        private ResivedGoodsDBComponent? ResivedGoodsPage = null;
        private UserDBComponent? UserPage = null;
        private UserDetachedLibDBComponent? UserDetachedLibPage = null;

        public AdminWindow()
        {
            InitializeComponent();

            ToPage<GoodsDBComponent>();
        }


        public void ToPage<T>() where T : UserControl, new()
        {
            if (typeof(T) == typeof(GoodsDBComponent))
                goodspage ??= new GoodsDBComponent();
            else if (typeof(T) == typeof(PurchaseOptionsDBConmponent))
                PurchaseOptionsPage ??= new PurchaseOptionsDBConmponent();
            else if (typeof(T) == typeof(ResivedGoodsDBComponent))
                ResivedGoodsPage ??= new ResivedGoodsDBComponent();
            else if (typeof(T) == typeof(UserDBComponent))
                UserPage ??= new UserDBComponent();
            else if (typeof(T) == typeof(UserDetachedLibDBComponent))
                UserDetachedLibPage ??= new UserDetachedLibDBComponent();

            UserControl? component = typeof(T) switch
            {
                var t when t == typeof(GoodsDBComponent) => goodspage,
                var t when t == typeof(PurchaseOptionsDBConmponent) => PurchaseOptionsPage,
                var t when t == typeof(ResivedGoodsDBComponent) => ResivedGoodsPage,
                var t when t == typeof(UserDBComponent) => UserPage,
                var t when t == typeof(UserDetachedLibDBComponent) => UserDetachedLibPage,
                _ => null
            };

            if (component != null)
                dbPresenter.Content = component;
        }

        private void User_DB(object sender, RoutedEventArgs e)
        {
            ToPage<UserDBComponent>();
        }

        private void games_DB(object sender, RoutedEventArgs e)
        {
            ToPage<GoodsDBComponent>();
        }

        private void PurhcaeOptions_DB(object sender, RoutedEventArgs e)
        {
            ToPage<PurchaseOptionsDBConmponent>();
        }

        private void ResivedGoods_DB(object sender, RoutedEventArgs e)
        {
            ToPage<ResivedGoodsDBComponent>();
        }
        private void UserDetachedLib_DB(object sender, RoutedEventArgs e)
        {
            ToPage<UserDetachedLibDBComponent>();
        }
    }
}
