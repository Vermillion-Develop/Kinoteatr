using Avalonia.Controls;
using Avalonia.Media;
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
    public partial class AddNewProcatViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
       
        public ObservableCollection<VideoProductDTO> GettedVideos { get; } = new ObservableCollection<VideoProductDTO>();
        public ObservableCollection<Cinema> GettedCinemas { get; } = new ObservableCollection<Cinema>();

        [ObservableProperty]
        public VideoProductDTO? _selectedVideo;

        [ObservableProperty]
        public Cinema? _selectedCinema;

        [ObservableProperty]
        public decimal? _revenue;

        [ObservableProperty]
        public decimal? rating;

        [ObservableProperty]
        public string? _errorMessage;
        public AddNewProcatViewModel()
        {
            _apiService = new ApiService();
            _ = GetVideoProductsAsync();
        }


        public async Task GetVideoProductsAsync()
        {
            var result = await _apiService.GetVideoProductDTOsAsync();
            if(result == null)
            {
                return;
            }

            GettedVideos.Clear();
            foreach(var video in result)
            {
                GettedVideos.Add(video);
            }
            _ = GetCinemaAsync();
        }
        
        public async Task GetCinemaAsync()
        {
            var result = await _apiService.GetCinemasAsync();
            if(result == null)
            {
                return;
            }
            GettedCinemas.Clear();
            foreach(var  cinema in result)
            {
               GettedCinemas.Add(cinema);
            }
        }

        [RelayCommand]
        public async Task AddNewProcat(Window? currentWindow)
        {
            if(SelectedCinema == null || SelectedVideo == null)
            {
                ErrorMessage = "Выберите видеопродукт и кинотеатр!";
                return;
            }
            if(Revenue == null || rating == null)
            {
                ErrorMessage = "Пожалуйста заполните все поля!";
                return;
            }
            if(SelectedVideo.DateEnd > DateTime.Now)
            {
                ErrorMessage = "Фильм еще не вышел!";
                return;
            }
            var result = await _apiService.AddNewProcatAsync(SelectedVideo.Id, SelectedCinema.Id, Revenue, Rating);
            if (result)
            {
                ErrorMessage = "";
                currentWindow.Close(true);
            }
            else
            {
                ErrorMessage = "Произошла ошика!";
            }
        }
    }
}
