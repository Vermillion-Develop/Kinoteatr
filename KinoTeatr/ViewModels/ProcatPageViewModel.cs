using Avalonia.Controls;
using Avalonia.Controls.Documents;
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
    public partial class ProcatPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<VideoProductInCinemasDTO> GettedVideoInCinemas { get; } = new ObservableCollection<VideoProductInCinemasDTO>();
        public ObservableCollection<VideoProductInCinemasDTO> DefaultVideo { get; } = new ObservableCollection<VideoProductInCinemasDTO>();
        public ObservableCollection<Cinema> GettedCinema { get; } = new ObservableCollection<Cinema>();

        [ObservableProperty]
        public Cinema? _selectedCinema;

        public ProcatPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetVideosInCinemasAsync();
        }

        public async Task GetVideosInCinemasAsync()
        {
            var result = await _apiService.GetVideoCinemasAsync();
            if (result == null)
            {
                Debug.WriteLine("Массив полученных видео был пуст!");
                return;
            }
            GettedVideoInCinemas.Clear();
            DefaultVideo.Clear();
            foreach (var video in result)
            {
                GettedVideoInCinemas.Add(video);
                DefaultVideo.Add(video);
            }
            _ = GetCinemaAsync();
        }

        [RelayCommand]
        public async Task OpenAddProcatFormAsync(Window? currentWindow)
        {
            AddNewProcatPageWindow procatWin = new AddNewProcatPageWindow()
            {
                DataContext = new AddNewProcatViewModel()
            };
            bool dialog = await procatWin.ShowDialog<bool>(currentWindow);
            if (dialog)
            {
                await GetVideosInCinemasAsync();
            }
        }

        public async Task GetCinemaAsync()
        {
            var result = await _apiService.GetCinemasAsync();
            if (result == null)
            {
                Debug.WriteLine("Массив полученных кинотеатров был пуст!");
                return;
            }
            GettedCinema.Clear();
            foreach (var cinema in result)
            {
                GettedCinema.Add(cinema);
            }
        }

        partial void OnSelectedCinemaChanged(Cinema? value)
        {
            if (value == null)
            {
                return;
            }
            GettedVideoInCinemas.Clear();
            var filtredList = DefaultVideo.Where(x => x.CinemaId == value.Id).ToList();

            var groupedMovies = filtredList.GroupBy(x => x.VideoProductId).Select(group => new VideoProductInCinemasDTO()
                {
                    VideoProductId = group.Key,

                    Revenue = group.Sum(x => x.Revenue),

                    Id = group.First().Id,
                    CinemaId = group.First().CinemaId,
                    CinemaName = group.First().CinemaName,
                    VideoName = group.First().VideoName,
                    Rating = group.First().Rating,
                    DateProduct = group.First().DateProduct
                }).ToList();

            foreach (var movie in groupedMovies)
            {
                GettedVideoInCinemas.Add(movie);
            }
        }
    }
}
