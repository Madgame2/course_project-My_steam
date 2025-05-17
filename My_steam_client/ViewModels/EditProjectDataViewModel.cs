using Game_Net_DTOLib;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using My_steam_client.Controls;
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
using System.Windows.Input;

namespace My_steam_client.ViewModels
{
    public enum FileType
    {
        HeaderImage,
        Zip,
        LibHeader,
        LibIcon,
        Screenshot
    }

    internal class EditProjectDataViewModel : INotifyPropertyChanged
    {
        private string _projectName;
        private string _projectDescription;
        private string _headerImageSource;
        private string _ZIPPath;
        private string _libHeaderPath;
        private string _libIconPath;
        private string _price;
        private bool _isPriceValid = true;
        private string _selectedScreenshot;

        public ObservableCollection<string> ScrinShots { get; set; } = new();



        public string SelectedScreenshot
        {
            get => _selectedScreenshot;
            set { _selectedScreenshot = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public bool CanRemoveScreenshot => !string.IsNullOrEmpty(SelectedScreenshot);

        public string ProjectName
        {
            get => _projectName;
            set { _projectName = value; OnPropertyChanged(); }
        }

        public string ProjectDiscription
        {
            get => _projectDescription;
            set { _projectDescription = value; OnPropertyChanged(); }
        }

        public string HeaderImageSource
        {
            get => _headerImageSource;
            set { _headerImageSource = value; OnPropertyChanged(); }
        }

        public string ZIP_Path
        {
            get => _ZIPPath;
            set { _ZIPPath = value; OnPropertyChanged(); }
        }

        public string LibHeaderPath
        {
            get => _libHeaderPath;
            set { _libHeaderPath = value; OnPropertyChanged(); }
        }

        public string LibIconPath
        {
            get => _libIconPath;
            set { _libIconPath = value; OnPropertyChanged(); }
        }

        public string Price
        {
            get => _price;
            set
            {
                var newValue = value?.Replace(',', '.') ?? string.Empty;
                if (IsValidPrice(newValue))
                {
                    _price = newValue;
                    IsPriceValid = true;
                }
                else
                {
                    _price = newValue;
                    IsPriceValid = false;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPriceValid));
            }
        }

        public bool IsPriceValid
        {
            get => _isPriceValid;
            set
            {
                if (_isPriceValid != value)
                {
                    _isPriceValid = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool IsValidPrice(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true; 

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d*(\.)?\d{0,2}$"))
                return false;

            return decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _);
        }

        public ICommand SubmitCommand { get; }
        public ICommand SelectFileCommand { get; }
        public ICommand RemoveScreenshotCommand { get; }


        public event Action<ProjectUploadDto>? ShowLoadingWindowRequested;
        public event Action? CloseLoadingWindowRequested;

        public event Action? CloseWindow;

        public EditProjectDataViewModel()
        {

            SelectFileCommand = new RelayCommand<string>(param =>
            {
                if (Enum.TryParse<FileType>(param, out var fileType))
                    SelectFile(fileType);
            });

            RemoveScreenshotCommand = new RelayCommand<string>(
                path =>
                {
                    ScrinShots.Remove(path);
                    if (ScrinShots.Count > 0)
                        SelectedScreenshot = ScrinShots.FirstOrDefault();
                    else
                        SelectedScreenshot = null;
                },
                path => CanRemoveScreenshot
            );

            ScrinShots.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(CanRemoveScreenshot));
                CommandManager.InvalidateRequerySuggested();
            };

            SubmitCommand = new RelayCommand(SendData);
        }


        private bool CanSend()
        {
            return !string.IsNullOrEmpty(_projectName) &&
                   !string.IsNullOrEmpty(_headerImageSource) &&
                   !string.IsNullOrEmpty(_ZIPPath) &&
                   !string.IsNullOrEmpty(_libHeaderPath) &&
                   !string.IsNullOrEmpty(_libIconPath) &&
                   !(ScrinShots.Count== 0) &&
                   !string.IsNullOrEmpty(_price);
        }
        private void SendData()
        {
            if (!CanSend())
            {
                MessageBox.Show("Please. fill in all fields before sending.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dto = new ProjectUploadDto
            {
                UserId = AppServices.UserId.ToString(),
                ProjectName = _projectName,
                Description = _projectDescription,
                Price = Convert.ToSingle(_price),
                HeaderImage = File.OpenRead(_headerImageSource),
                ZIPFile = File.OpenRead(_ZIPPath),
                LibHeader = File.OpenRead(_libHeaderPath),
                LibIcon = File.OpenRead(_libIconPath),
                Screenshots = new List<Stream>()
            };

            foreach (var path in ScrinShots)
            {
                dto.Screenshots.Add(File.OpenRead(path));
            }

 
                CloseWindow?.Invoke();
                ShowLoadingWindowRequested?.Invoke(dto);

        }
        private void SelectFile(FileType fileTyoe)
        {
            var dialog = new OpenFileDialog();

            switch (fileTyoe)
            {
                case FileType.Zip:
                    dialog.Filter = "ZIP архивы (*.zip)|*.zip";
                    break;
                default:
                    dialog.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                    break;
            }

            if (dialog.ShowDialog() != true)
                return;

            var selectedPath = dialog.FileName;

            switch (fileTyoe)
            {
                case FileType.HeaderImage:
                    HeaderImageSource = selectedPath;
                    break;
                case FileType.Zip:
                    ZIP_Path = selectedPath;
                    break;
                case FileType.LibHeader:
                    LibHeaderPath = selectedPath;
                    break;
                case FileType.LibIcon:
                    LibIconPath = selectedPath;
                    break;
                case FileType.Screenshot:
                    {
                        foreach (var file in dialog.FileNames)
                            ScrinShots.Add(file);
                        if (ScrinShots.Count > 0 && string.IsNullOrEmpty(SelectedScreenshot))
                            SelectedScreenshot = ScrinShots.FirstOrDefault();
                        break;
                    }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
