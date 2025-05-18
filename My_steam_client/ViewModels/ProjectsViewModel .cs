using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Controls;
using My_steam_client.Models;
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
    public class ProjectsViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Project> Projects { get; set; } = new();

        private Project _selectedProject;
        public Project SelectedProject
        {
            get => _selectedProject;
            set { _selectedProject = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public Action? OnAddProjectRequested { get; set; }

        public ProjectsViewModel()
        {

            AddCommand = new RelayCommand(AddProject);
            EditCommand = new RelayCommand(EditProject, () => SelectedProject != null);
            DeleteCommand = new RelayCommand(DeleteProject, () => SelectedProject != null);

            InitProjects();
            //Projects.Add(new Project { CreatedAt = DateTime.Now, ProjectId=1, ProjectDescription="some decription", ProjectName="some name" });
        }


        private async void InitProjects()
        {
            var service = AppServices.Provider.GetRequiredService<Game_Net.PublisherService>();
            var dtoList = await service.GetMyProjects(AppServices.UserId);

            foreach (var d in dtoList)
            {
                Projects.Add(new Project { CreatedAt=d.CreatedAt,ProjectId=d.ProjectId, ProjectDescription=d.ProjectDescription, ProjectName= d.ProjectName});
            }
        }

        private void AddProject()
        {
            OnAddProjectRequested?.Invoke();
        }

        private void EditProject()
        {
            // здесь логика редактирования
        }

        private async void DeleteProject()
        {
            if (SelectedProject != null)
            {
                var messageWindow = new YesNoDialog("DeliteProject", "Are you shure?", "This will complitely delete all records about your project");
                bool? dialogResult = messageWindow.ShowDialog();

                bool userResult = messageWindow.Result;

                if (!userResult) return;

                var service = AppServices.Provider.GetRequiredService<Game_Net.PublisherService>();
                try
                {
                    await service.DeleteMyProject(AppServices.UserId, SelectedProject.ProjectId);
                    Projects.Remove(SelectedProject);

                }
                catch
                {
                    MessageBox.Show("Не удалось удалить", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
