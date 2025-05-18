using Game_Net;
using Game_Net_DTOLib;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.AdminComponents.Models;
using My_steam_client.AdminComponents.ModelsView;
using My_steam_client.AdminComponents.Templates;
using My_steam_client.Scripts;
using My_steam_server.Models;
using System.Windows;
using System.Windows.Controls;

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

        private readonly Game_Net.AdminService adminService;

        public AdminWindow()
        {
            adminService = AppServices.Provider.GetRequiredService<Game_Net.AdminService>();

            InitializeComponent();

            ToPage<GoodsDBComponent>();
        }

        private async void LogOut(object sender, RoutedEventArgs e)
        {

            var auth_service = AppServices.Provider.GetRequiredService<Game_Net.AuthService>();// .Provider.GetRequiredService<Game_Net.AuthService>();
                var result = await auth_service.LogOutAsync();

                if (result.Success)
                {
                    TokenStorage.DeleteTokens();

                    var authWindow = new AuthWindow();
                    Application.Current.MainWindow = authWindow;
                    this.Close();

                    authWindow.Show();
                }
                else
                {
                    MessageBox.Show("Unadebel to Log out");
                }
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SynchronizeDto dto = new SynchronizeDto();

            var Users = UserPage.DataContext as UserViewModel;
            var goods = goodspage.DataContext as GamesViewModel;
            var PurchaseOptions = PurchaseOptionsPage.DataContext as PurchaseOptionsModelView;
            var Lib = UserDetachedLibPage.DataContext as DetachedLibModelView;

            var UsersList = Users?.Users;
            var goodsList = goods?.games;
            var PurchsaeOptions = PurchaseOptions?.Options;
            var LibList = Lib?.LibItems;

            dto.Users = ConvertToDB_dto(UsersList.ToList());
            dto.AttachetLib = ConvertToDB_dto(LibList.ToList());
            dto.Purhcases = ConvertToDB_dto(PurchsaeOptions.ToList());
            dto.Games = ConvertToDB_dto(goodsList.ToList());


            await adminService.Synchronize(dto);
        }

        private List<UserDB_dto> ConvertToDB_dto(List<My_steam_client.AdminComponents.Models.User> rep)
        {
            var result = new List<UserDB_dto>();

            foreach (var item in rep) { 
                var newItem = new UserDB_dto();
                newItem.registerDate = item.registerDate;
                newItem.Role = item.Role;
                newItem.Email = item.Email;
                newItem.registerDate = item.registerDate;
                newItem.UserName = item.UserName;
                newItem.UserId = item.UserId;
                newItem.IsNew = item.IsNew;
                
                result.Add(newItem);    
            }

            return result;
        }
        private List<DetachetLibDB_dto> ConvertToDB_dto(List<My_steam_client.AdminComponents.Models.DetachedLib> list)
        {
            var result = new List<DetachetLibDB_dto>();

            foreach (var item in list)
            {
                var newItem = new DetachetLibDB_dto();
                newItem.UserId = item.UserId;
                newItem.GameId = item.GameId;
                newItem.PurchaseDate = item.PurchaseDate;
                newItem.IsNew = item.IsNew;

                result.Add(newItem);
            }

            return result;
        }

        private List<Game_Net_DTOLib.PurhcaseOptionsDB_dto> ConvertToDB_dto(List<My_steam_client.AdminComponents.Models.PurchseOptions> list)
        {
            var result = new List<PurhcaseOptionsDB_dto>();

            foreach (var item in list)
            {
                var newItem = new PurhcaseOptionsDB_dto();
                newItem.PurhcaseId = item.PurhcaseId;
                newItem.GameID = item.GameID;
                newItem.imageLinnk = item.imageLinnk;
                newItem.Price = item.Price;
                newItem.IsNew = item.IsNew;

                result.Add(newItem);
            }

            return result;
        }

        private List<GamesDB_dto> ConvertToDB_dto(List<Games> list)
        {
            var result = new List<GamesDB_dto>();

            foreach (var item in list)
            {
                var newItem = new GamesDB_dto();
                newItem.GameId = item.GameId;
                newItem.GameName = item.GameName;
                newItem.Descritption = item.Descritption;
                newItem.ReliseDate = item.ReliseDate;
                newItem.Price = item.Price;
                newItem.HeaderImageSource = item.HeaderImageSource;
                newItem.DownloadedSource= item.DownloadedSource;
                newItem.Rating = item.Rating;
                newItem.UserId = item.UserId;
                newItem.IsNew = item.IsNew;

                result.Add(newItem);
            }

            return result;
        }
    }
}
