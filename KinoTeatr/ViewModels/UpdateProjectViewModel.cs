using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatrShared.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class UpdateProjectViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        public VideoProductDTO? _videoProductDTO;

        public ObservableCollection<StatusVideoProduct> GettedStatuses { get; } = new ObservableCollection<StatusVideoProduct>();
        public ObservableCollection<AdultVideoProduct> GettedAdults { get; } = new ObservableCollection<AdultVideoProduct>();
        public ObservableCollection<VidVideoProduct> GettedTypeVideo { get; } = new ObservableCollection<VidVideoProduct>();

        [ObservableProperty]
        public AdultVideoProduct? _selectedAdult;

        [ObservableProperty]
        public VidVideoProduct? _selectedVidVideo;

        [ObservableProperty]
        public StatusVideoProduct? _selectedStatus;

        [ObservableProperty]
        public string? _errorMessage;

        [ObservableProperty]
        public DateTimeOffset? _selectedDateEnd;

        [ObservableProperty]
        public string? _nameProject;

        [ObservableProperty]
        public string? _idea;

        [ObservableProperty]
        public decimal? _budget;
        public UpdateProjectViewModel(VideoProductDTO selectedVideo)
        {
            _apiService = new ApiService();
            VideoProductDTO = selectedVideo;
            _ = GetAllStatuses();
        }

        public async Task GetAllStatuses()
        {
            var result = await _apiService.GetStatusVideoAsync();
            if (result == null)
            {
                Debug.WriteLine("Полученный массив статусов был пуст!");
                return;
            }
            GettedStatuses.Clear();
            foreach (var status in result)
            {
                GettedStatuses.Add(status);
            }
            _ = GetAdultsAsync();
        }

        public async Task GetAdultsAsync()
        {
            var result = await _apiService.GetAdultsAsync();
            if (result == null)
            {
                Debug.WriteLine("Полученный массив статусов был пуст!");
                return;
            }
            GettedAdults.Clear();
            foreach (var status in result)
            {
                GettedAdults.Add(status);
            }
            _ = GetVidVideoAsync();
        }

        public async Task GetVidVideoAsync()
        {
            var result = await _apiService.GetVidVideoAsync();
            if (result == null)
            {
                Debug.WriteLine("Полученный массив статусов был пуст!");
                return;
            }
            GettedTypeVideo.Clear();
            foreach (var status in result)
            {
                GettedTypeVideo.Add(status);
            }
        }

        [RelayCommand]
        public async Task UpdateProjectAsync(Window? currentWindow)
        {
            if(string.IsNullOrWhiteSpace(NameProject) || VideoProductDTO == null || Budget == null || SelectedVidVideo == null || SelectedDateEnd == null || SelectedStatus == null || SelectedAdult == null)
            {
                ErrorMessage = "Заполните поля!";
                return;
            }
                Debug.WriteLine(VideoProductDTO.Id);

            var result = await _apiService.UpdateSelectedProjectAsync(VideoProductDTO.Id, NameProject, Budget, SelectedVidVideo.Id, SelectedDateEnd.Value.ToUniversalTime(), SelectedStatus.Id, SelectedAdult.Id, Idea);
            if (result)
            {
                currentWindow?.Close(true);
            }
        }
    }
}
