using Avalonia.Controls;
using Avalonia.Controls.Documents;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.Views;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class GraphicGroupViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<VideoProductDTO> GettedVideos { get; } = new ObservableCollection<VideoProductDTO>();
        public ObservableCollection<GraphicVideoDTO> GettedGraphic { get; } = new ObservableCollection<GraphicVideoDTO>();
        [ObservableProperty]
        public DateTimeOffset _selectedDate;
        [ObservableProperty]
        public string? _errorMessage;
        

        [ObservableProperty]
        public string? _searchStroke;
        public GraphicGroupViewModel()
        {
            _apiService = new ApiService();
            _ = GetVideosDTOAsync();
        }

        public async Task GetVideosDTOAsync()
        {
            var result = await _apiService.GetVideoProductDTOsAsync();
            if (result == null)
            {
                ErrorMessage = "Массив видео был пуст!";
                return;
            }
            GettedVideos.Clear();
            foreach (var video in result) 
            {
                GettedVideos.Add(video);
            }
            _ = GetGraphicVideosAsync();
        }

       public async Task GetGraphicVideosAsync()
        {
            var result = await _apiService.GetGraphicVideosDTOAsync();
            if(result  == null)
            {
                ErrorMessage = "Ошибка получения массив графика записей";
                return;
            }
            GettedGraphic.Clear();
            foreach(var graph in result)
            {
                GettedGraphic.Add(graph);
            }
        }
    }
}
