using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.Views;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class ProjectPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<VideoProductDTO> GettedProjects { get; } = new ObservableCollection<VideoProductDTO>();
        public ObservableCollection<VideoProductDTO> DefaultProjects { get; } = new ObservableCollection<VideoProductDTO>();
        public ObservableCollection<StatusVideoProduct> GettedStatuses { get; } = new ObservableCollection<StatusVideoProduct>();

        [ObservableProperty]
        public VideoProductDTO? _selectedProject;

        [ObservableProperty]
        public StatusVideoProduct? _selectedStatus;

        [ObservableProperty]
        public string? _searchStroke;
        public ProjectPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetStatusesAsync();
        }

        public async Task GetStatusesAsync()
        {
            var result = await _apiService.GetStatusVideoAsync();
            if (result == null) 
            {
                Debug.WriteLine("Массив статусов был пуст");
                return;
            }
            GettedStatuses.Clear();
            foreach(var stat in result)
            {
                GettedStatuses.Add(stat);
            }
            _ = GetProjectsAsync();
        }

        public async Task GetProjectsAsync()
        {
            var result = await _apiService.GetVideoProductDTOsAsync();
            if (result == null)
            {
                Debug.WriteLine("Массив проектов был пуст");
                return;
            }

            GettedProjects.Clear();
            DefaultProjects.Clear();
            foreach (var project in result) 
            {
                GettedProjects.Add(project);
                DefaultProjects.Add(project);
            }
        }

        public async Task OpenAboutProjectFormAsync(Window? currentWindow, VideoProductDTO selectedVideo)
        {

            AboutProjectPageWindow videoWin = new AboutProjectPageWindow()
            {
                DataContext = new AboutProjectViewModel(selectedVideo)
            };
            videoWin.Show();
        }

        [RelayCommand]
        private async Task MultibindingAboutProject(object? parameter)
        {
            if (parameter is System.Collections.IList values && values.Count == 2)
            {
                var window = values[0] as Window;
                var video = values[1] as VideoProductDTO;

                if (video != null)
                {
                    await OpenAboutProjectFormAsync(window, video);
                }
            }
        }

        [RelayCommand]
        public async Task OpenAddProjectFormAsync()
        {
            AddNewProjectPageWindow addVideo = new AddNewProjectPageWindow()
            {
                DataContext = new AddNewProjectViewModel()
            };
            addVideo.Show();
        }

        [RelayCommand]
        public async Task OpenUpdateProjectFormAsync()
        {
            if(SelectedProject == null)
            {
                Debug.WriteLine("Выберите проект для редактирования");
                return;
            }
            UpdateProjectPageWindow addVideo = new UpdateProjectPageWindow()
            {
                DataContext = new UpdateProjectViewModel(SelectedProject)
            };
            addVideo.Show();
        }

        partial void OnSelectedStatusChanging(StatusVideoProduct? value)
        {
            GettedProjects.Clear();

            if (value == null)
            {
                foreach (var project in DefaultProjects)
                {
                    GettedProjects.Add(project);
                }
                return;
            }

            var groplist = DefaultProjects.Where(x => x.StatusVideoProductId == value.Id);
            foreach (var film in groplist)
            {
                GettedProjects.Add(film);
            }
        }

        partial void OnSearchStrokeChanged(string? value)
        {
            GettedProjects.Clear();
            if (string.IsNullOrWhiteSpace(value))
            {
                foreach (var project in DefaultProjects)
                {
                    if (SelectedStatus == null || project.StatusVideoProductId == SelectedStatus.Id)
                    {
                        GettedProjects.Add(project);
                    }
                }
                return;
            }

            string searchText = value.ToLower();

            foreach (var project in DefaultProjects)
            {
                string projectName = project.Name?.ToLower() ?? "";

                if (projectName.StartsWith(searchText))
                {
                    if (SelectedStatus == null || project.StatusVideoProductId == SelectedStatus.Id)
                    {
                        GettedProjects.Add(project);
                    }
                }
            }
        }
    }
}
