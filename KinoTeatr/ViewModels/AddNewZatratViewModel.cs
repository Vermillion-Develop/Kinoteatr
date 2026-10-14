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
    public partial class AddNewZatratViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public ObservableCollection<TypeZatrat> GettedTypeZatrat { get; } = new ObservableCollection<TypeZatrat>();
        public ObservableCollection<VideoProductDTO> GettedProducts { get; } = new ObservableCollection<VideoProductDTO>();

        [ObservableProperty]
        public TypeZatrat? _selectedType;

        [ObservableProperty]
        public VideoProductDTO? _selectedVideo;

        [ObservableProperty]
        public decimal? _selectedCost;

        [ObservableProperty]
        public DateTimeOffset? _selectedDate;

        [ObservableProperty]
        public string? _errorMessage;

        public AddNewZatratViewModel()
        {
            _apiService = new ApiService();
            _ = GetTypesAsync();
        }

        public async Task GetTypesAsync()
        {
            var result = await _apiService.GetTypesZatratAsync();
            if (result == null)
            {
                return;
            }
            GettedTypeZatrat.Clear();
            foreach (var zat in result)
            {
                GettedTypeZatrat.Add(zat);
            }
            _ = GetVideoAsync();
        }

        public async Task GetVideoAsync()
        {
            var result = await _apiService.GetVideoProductDTOsAsync();
            if (result == null)
            {
                return;
            }
            GettedProducts.Clear();
            foreach (var zat in result)
            {
                GettedProducts.Add(zat);
            }
        }

        [RelayCommand]
        public async Task AddNewZatratAsync(Window? currentWindow)
        {
            if(SelectedVideo == null || SelectedType == null || SelectedCost == null || SelectedDate == null)
            {
                ErrorMessage = "Заполните поля!";
                return;
            }
            var result = await _apiService.AddNewZatratAsync(SelectedType.Id, SelectedCost, SelectedDate.Value.ToUniversalTime(), SelectedVideo.Id);
            if (result)
            {
                currentWindow.Close(true);
            }
            else
            {
                ErrorMessage = "Произошла ошибка!";
                return;
            }
        }

    }
}
