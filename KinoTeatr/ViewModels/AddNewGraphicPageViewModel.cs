using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatrShared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class AddNewGraphicPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<VideoProductDTO> GettedProjects { get; } = new ObservableCollection<VideoProductDTO>();
        public ObservableCollection<AreaDTO> GettedAreas { get; } = new ObservableCollection<AreaDTO>();
        public ObservableCollection<Scene> GettedScenes { get; } = new ObservableCollection<Scene>();
        public ObservableCollection<CinemaGroupDTO> GettedGroup { get; } = new ObservableCollection<CinemaGroupDTO>();

        [ObservableProperty]
        public List<string> _gettedTimes = new List<string>() {
            "8:30","9:00","9:30","10:00","10:30","11:00","11:30",
            "12:00","12:30","13:00","13:30","14:00","14:30","15:00",
            "15:30","16:00","16:30","17:00","17:30","18:00","18:30",
            "19:00","19:30","20:00"
        };

        [ObservableProperty]
        public VideoProductDTO? _selectedProduct;

        [ObservableProperty]
        public AreaDTO? _selectedArea;

        [ObservableProperty]
        public Scene? _selectedScene;

        [ObservableProperty]
        public CinemaGroupDTO? _selectedGroup;

        [ObservableProperty]
        public DateTimeOffset? _selectedDate;

        [ObservableProperty]
        public string? _selectedTime;

        [ObservableProperty]
        public string? _errorMessage;
        public AddNewGraphicPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetAllVideoProducts();
        }

        public async Task GetAllVideoProducts()
        {
            var result = await _apiService.GetVideoProductDTOsAsync();
            if (result == null)
            {
                Debug.WriteLine("Массив фильмов был пуст!");
                return;
            }
            GettedProjects.Clear();
            foreach (var video in result)
            {
                GettedProjects.Add(video);
            }
            _ = GetAreasAsync();
        }

        public async Task GetAreasAsync()
        {
            var result = await _apiService.GetAreaDTOsAsync();
            if (result == null)
            {
                ErrorMessage = "Массив площадок был пуст!";
                return;
            }
            GettedAreas.Clear();
            foreach (var area in result)
            {
                GettedAreas.Add(area);
            }
            _ = GetScenesAsync();
        }
        public async Task GetScenesAsync()
        {
            var result = await _apiService.GetScenesAsync();
            if (result == null)
            {
                ErrorMessage = "Массив площадок был пуст!";
                return;
            }
            GettedScenes.Clear();
            foreach (var area in result)
            {
                GettedScenes.Add(area);
            }
            _ = GetGroupsAsync();
        }
        public async Task GetGroupsAsync()
        {
            var result = await _apiService.GetAllGroupsAsync();
            if (result == null)
            {
                ErrorMessage = "Массив площадок был пуст!";
                return;
            }
            GettedGroup.Clear();
            foreach (var area in result)
            {
                GettedGroup.Add(area);
            }
        }


        [RelayCommand]
        public async Task AddNewGraph(Window? currentWindow)
        {
            if (SelectedGroup == null || SelectedProduct == null || SelectedArea == null || SelectedScene  == null || SelectedDate  == null ||  SelectedTime == null)
            {
                ErrorMessage = "Заполните все поля!";
                return;
            }
            var result = await _apiService.AddNewGraphAsync(SelectedGroup.Id,SelectedProduct.Id, SelectedArea.Id, SelectedScene.Id, SelectedDate.Value.ToUniversalTime().AddHours(10), TimeSpan.Parse(SelectedTime));
            if( result == false)
            {
                ErrorMessage = "Произошла ошибка!";
                return;
            }
            currentWindow.Close(true);
        }


    }
}
