using My_steam_client.Controls;
using My_steam_client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

            Projects.Add(new Project { CreatedAt = DateTime.Now, ProjectId=1, ProjectDescription="some decription", ProjectName="some name" });
        }

        private void AddProject()
        {
            OnAddProjectRequested?.Invoke();
        }

        private void EditProject()
        {
            // здесь логика редактирования
        }

        private void DeleteProject()
        {
            if (SelectedProject != null)
            {
                Projects.Remove(SelectedProject);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
