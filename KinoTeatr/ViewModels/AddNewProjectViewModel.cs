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
    public partial class AddNewProjectViewModel : ViewModelBase
    {

        private readonly IApiService _apiService;
       
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
        public DateTimeOffset? _selectedDateStart;

        [ObservableProperty]
        public DateTimeOffset? _selectedDateEnd;

        [ObservableProperty]
        public string? _nameProject;

        [ObservableProperty]
        public string? _idea;

        [ObservableProperty]
        public decimal? _budget;

        public AddNewProjectViewModel()
        {
            _apiService = new ApiService();
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
        public async Task AddNewProjectAsync(Window? currentWindow)
        {
            if(SelectedDateEnd == null ||
                SelectedDateStart == null ||
                string.IsNullOrWhiteSpace(NameProject) ||
                string.IsNullOrWhiteSpace(Idea) ||
                Budget == null ||
                SelectedStatus == null ||
                SelectedVidVideo == null ||
                SelectedAdult == null)
            {
                ErrorMessage = "Заполните все поля!";
                return;
            }
            var result = await _apiService.AddNewProjectAsync(NameProject, Budget, SelectedVidVideo?.Id, SelectedDateStart.Value.ToUniversalTime(), SelectedDateEnd.Value.ToUniversalTime(), SelectedVidVideo?.Id,SelectedAdult?.Id,Idea);
            if (result)
            {
                currentWindow?.Hide();
            }
            else
            {
                Debug.WriteLine("Ошибка при добавлении проекта");
            }
        }

    }
}
