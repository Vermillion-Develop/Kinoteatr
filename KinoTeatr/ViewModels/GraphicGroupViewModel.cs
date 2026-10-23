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
        public ObservableCollection<GraphicVideoDTO> DefaultGraphic { get; } = new ObservableCollection<GraphicVideoDTO>();
        [ObservableProperty]
        public DateTimeOffset? _selectedDate;
        [ObservableProperty]
        public string? _errorMessage;

        [ObservableProperty]
        public VideoProductDTO? _selectedProduct;

        [ObservableProperty]
        public GraphicVideoDTO? _selectedGraph;
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
                DefaultGraphic.Add(graph);
            }
        }

        partial void  OnSelectedProductChanged(VideoProductDTO? value)
        {
            _ =  ApplySearch();
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            _ = ApplySearch();
        }

        public async Task ApplySearch()
        {
            IEnumerable<GraphicVideoDTO> FiltredVideos;
            if (SelectedProduct != null && SelectedDate != null)
            {
               FiltredVideos = DefaultGraphic.Where(x => x.VideoProductId == SelectedProduct.Id && x.Date == SelectedDate.Value.AddHours(10));
               if(FiltredVideos != null)
                {
                    GettedGraphic.Clear();
                    foreach (var graph in FiltredVideos)
                    {
                        GettedGraphic.Add(graph);
                    }
                } 
            }
            else if (SelectedDate != null && SelectedProduct == null) 
            {
               FiltredVideos = DefaultGraphic.Where(x => x.Date == SelectedDate.Value.AddHours(10));
                if (FiltredVideos != null)
                {
                    GettedGraphic.Clear();
                    foreach (var graph in FiltredVideos)
                    {
                        GettedGraphic.Add(graph);
                    }
                }
            }
            else if (SelectedProduct != null && SelectedDate == null) 
            {
               FiltredVideos = DefaultGraphic.Where(x => x.VideoProductId == SelectedProduct.Id);
                if (FiltredVideos != null)
                {
                    GettedGraphic.Clear();
                    foreach (var graph in FiltredVideos)
                    {
                        GettedGraphic.Add(graph);
                    }
                }
            }
            else
            {
                FiltredVideos = DefaultGraphic;
                if (FiltredVideos != null)
                {
                    GettedGraphic.Clear();
                    foreach (var graph in FiltredVideos)
                    {
                        GettedGraphic.Add(graph);
                    }
                }
            }

        }

        [RelayCommand]
        public async Task OpenAddGraphicFormAsync(Window? currentWindow)
        {
            AddNewGraphicPageWindow addGraph = new AddNewGraphicPageWindow()
            {
                DataContext = new AddNewGraphicPageViewModel()
            };
            bool dialog = await addGraph.ShowDialog<bool>(currentWindow);
            if (dialog)
            {
                await GetGraphicVideosAsync();
            }
        }

        [RelayCommand]
        public async Task OpenAboutGraphicFormAsync()
        {
            if(SelectedGraph == null)
            {
                ErrorMessage = "Выберите запись съемок!";
                return;
            }
            AboutGraphicPageWindow aboutWin = new AboutGraphicPageWindow()
            {
                DataContext = new AboutGraphicViewModel(SelectedGraph)
            };
            aboutWin.Show();
        }
    }
}
