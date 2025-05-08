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
                PlayButton.ButtonClicked += launchApp;
            }
            InfoRoot.Content = new PlayInfo(manifestRecord.playedTime, manifestRecord.lastPlayed);
            ActivityRoot.Content = new Activity();

            HeaderImageLink = manifestRecord.HeaderImageSource;
        }

        private void launchApp(object? sender, EventArgs arg)
        {
            if (sender  is not PlayButton playButton) return;

            if(InfoRoot.Content is PlayInfo playInfo) playInfo.lastLaynch=DateTime.Now;

            _ = Task.Run(async () =>
            {
                try
                {
                    StopButton stopButton = null!;

                    Dispatcher.Invoke(() =>
                    {
                        stopButton = new StopButton();
                        stopButton.ButtonClecked += async (s, e) =>
                        {
                            var duration = await _launchAppService.TerminateGameAsync(manifestRecord.RecordId);
                            if (duration.HasValue && InfoRoot.Content is PlayInfo info)
                            {
                                info.PlayTime += duration.Value;
                            }

                            // Вернуть кнопку Play после завершения
                            await Dispatcher.InvokeAsync(() => ButttonRoot.Content = playButton);
                        };
                        ButttonRoot.Content = stopButton;
                    });

                    var timeSpan = await _launchAppService.LaunchAndTrackGame(
                        manifestRecord.RecordId,
                        manifestRecord.GameName,
                        manifestRecord.ExecuteFileSource);

                    Dispatcher.Invoke(() =>
                    {
                        if (timeSpan.HasValue && InfoRoot.Content is PlayInfo info)
                        {
                            info.PlayTime += timeSpan.Value;
                        }

                        Dispatcher.Invoke(() => ButttonRoot.Content = playButton);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при запуске игры: {ex.Message}");
                }
            });

        }
    }
}
