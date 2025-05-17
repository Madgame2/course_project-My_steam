using My_steam_client.ViewModels;
using System.Windows;
using Game_Net_DTOLib;
using My_steam_client.Models;
using My_steam_client.Scripts;
using Microsoft.Extensions.DependencyInjection;

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для UploadingWindow.xaml
    /// </summary>
    public partial class UploadingWindow : Window
    {
        public UploadingWindow(ProjectUploadDto dto)
        {
            InitializeComponent();
            var Service= AppServices.Provider.GetRequiredService<Game_Net.ComunitationMannageer>();
            var Vm = new UploadingViewModel(dto, Service);
            Vm.CloseWindow += ()=> this.Close();

            DataContext = Vm;
        }
    }
}
