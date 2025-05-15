using My_steam_client.Scripts;
using My_steam_client.Templates;
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
using Game_Net_DTOLib;
using My_steam_server.DTO_models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для MyAccountWindow.xaml
    /// </summary>
    public partial class MyAccountWindow : Window, INotifyPropertyChanged
    {
        private string nickName;
        private UserRole role;
        private DateTime registerDate;
        private DateTime? lastPurchase;
        private int gamesCount;
        private TimeSpan timeInGames;

        public TimeSpan TimeInGames
        {
            get => timeInGames;
            set=> SetProperty(ref timeInGames, value);
        }

        public int GamesCount
        {
            get => gamesCount;
            set=> SetProperty(ref gamesCount, value);
        }

        public string NickName
        {
            get => nickName;
            set => SetProperty(ref nickName, value);
        }

        public UserRole Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }

        public DateTime RegisterDate
        {
            get => registerDate;
            set => SetProperty(ref registerDate, value);
        }

        public DateTime? LastPurchase
        {
            get => lastPurchase;
            set => SetProperty(ref lastPurchase, value);
        }

        public MyAccountWindow()
        {
            InitializeComponent();
            InitPage();
            HeaderContaner.Content = new Header(this);
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private async void InitPage()
        {
            NickName = AppServices.UsserNickNmae;
            Role = AppServices.userRole;
            RegisterDate = AppServices.UserRegisterDate;

            var lib = AppServices.Provider.GetRequiredService<LibMannager>();
            LastPurchase = await lib.GetLastPurchase();
            GamesCount = await lib.GetGamesCount();
            TimeInGames= await lib.GetTimeInGames();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (new SupportMesageForm()).Show();
            this.Close();
        }
    }

}
