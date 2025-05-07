using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts;
using My_steam_client.Scripts.Models;
using My_steam_client.Scripts.Services;
using My_steam_server.Models;
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
    /// Логика взаимодействия для LibraryGamePageComponent.xaml
    /// </summary>
    public partial class LibraryGamePageComponent : UserControl
    {
        private ManifestRecord? manifestRecord;
        private LaunchAppService _launchAppService;

        public string HeaderImageLink { get; set; }
        public LibraryGamePageComponent(long GameId)
        {
            InitPage(GameId);

            InitializeComponent();
            DataContext = this;
        }

        private async void InitPage(long GameId)
        {
            _launchAppService = AppServices.Provider.GetRequiredService<LaunchAppService>();


            var LibMannager = AppServices.Provider.GetRequiredService<LibMannager>();
            manifestRecord = await LibMannager.repository.getRecordByIdAsync(GameId);

            if (manifestRecord == null) throw new Exception("Un definded record");

            if (string.IsNullOrEmpty(manifestRecord.ExecuteFileSource))
            {
                ButttonRoot.Content = new InstallButton();
                DownloadInfoRoot.Content = new DownLoadInfo(manifestRecord.SpaceRequered);
            }
            else
            {
                var PlayButton = new PlayButton();
                ButttonRoot.Content = PlayButton;
                PlayButton.ButtonClicked += (s, e) =>
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _launchAppService.LaunchAndTrackGame(manifestRecord.RecordId, manifestRecord.GameName, manifestRecord.ExecuteFileSource);
                        }
                        catch (Exception ex)
                        {
                            // Обработай или залогируй ошибку
                            Console.WriteLine($"Ошибка при запуске игры: {ex.Message}");
                        }
                    });
                };
            }
            InfoRoot.Content = new PlayInfo(manifestRecord.playedTime, manifestRecord.lastPlayed);
            ActivityRoot.Content = new Activity();

            HeaderImageLink = manifestRecord.HeaderImageSource;
        }

        private void launchApp()
        {

        }
    }
}
