using Game_Net;
using Game_Net_DTOLib;
using My_steam_client.Controls;
using My_steam_client.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace My_steam_client.ViewModels
{
    public class UploadingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ProjectUploadDto _dto;
        private readonly ComunitationMannageer _comManager;


        public Action CloseWindow;

        public string StatusText { get; set; } = "Uploading";
        public bool IsError { get; set; } = false;
        public ICommand RetryCommand { get; }


        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }


        public UploadingViewModel(ProjectUploadDto dto, ComunitationMannageer comManager)
        {
            _dto = dto;
            _comManager = comManager;

            RetryCommand = new RelayCommand(async () => await SendAsync());

            _ = SendAsync(); // Запускаем отправку при создании
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
                        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async Task SendAsync()
        {

            CurrentView = new SendingComponent();

            IsError = false;
            StatusText = "Uploading";

            try
            {
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(_dto.ProjectName), "ProjectName");
                content.Add(new StringContent(_dto.Description), "Description");
                content.Add(new StringContent(_dto.Price.ToString()), "Price");

                content.Add(new ByteArrayContent(_dto.HeaderImage), "HeaderImage");
                content.Add(new StreamContent(_dto.ZIPFile), "ZIPFile");
                content.Add(new ByteArrayContent(_dto.LibHeader), "LibHeader");
                content.Add(new ByteArrayContent(_dto.LibIcon), "LibIcon");

                for (int i = 0; i < _dto.Screenshots.Count; i++)
                {
                    content.Add(new ByteArrayContent(_dto.Screenshots[i]), "Screenshots", $"screenshot_{i}.png");
                }

                try
                {
                    await _comManager.SendMultipartAsync<bool>("api/upload", Protocol.Http, content);
                    StatusText = "Secsses sended!";
                }
                catch
                {
                    var ErrorPage = new SendingErrorComponent();
                    ErrorPage.CloseButtonnPrsed += CloseWindow;
                    ErrorPage.TryAgainPrsed += async () => await SendAsync();

                    CurrentView = ErrorPage;
                    IsError = true;
                }
            }
            catch (Exception ex)
            {
                StatusText = $"System error: {ex.Message}";
                IsError = true;
            }
        }
    }
}
