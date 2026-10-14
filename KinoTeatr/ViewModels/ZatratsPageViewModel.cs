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
    public partial class ZatratsPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public ObservableCollection<TypeZatrat> GettedTypeZatrat { get; } = new ObservableCollection<TypeZatrat>();
        public ObservableCollection<VideoProductDTO> GettedProducts { get; } = new ObservableCollection<VideoProductDTO>();
        public ObservableCollection<ZatratDTO> GettedZatratDTO { get; } = new ObservableCollection<ZatratDTO>();
        public ObservableCollection<ZatratDTO> GettedDefaultProducts { get; } = new ObservableCollection<ZatratDTO>();

        [ObservableProperty]
        public ZatratDTO? _selectedZatrat;

        [ObservableProperty]
        public TypeZatrat? _selectedType;

        [ObservableProperty]
        public VideoProductDTO? _selectedVideo;

        public ZatratsPageViewModel()
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
            _ = GetZatratsAsync();
        }

        public async Task GetZatratsAsync()
        {
            GettedZatratDTO.Clear();
            GettedDefaultProducts.Clear();
            var result = await _apiService.GetZatratAsync();
            if (result == null)
            {
                return;
            }
            GettedZatratDTO.Clear();
            foreach (var zat in result)
            {
                GettedZatratDTO.Add(zat);
                GettedDefaultProducts.Add(zat);
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
        public async Task OpenAddNewZatratFormAsync(Window? currentWindow)
        {
            AddNewZatratPageWindow zatratWin = new AddNewZatratPageWindow()
            {
                DataContext = new AddNewZatratViewModel()
            };
            bool dialog = await zatratWin.ShowDialog<bool>(currentWindow);
            if (dialog) 
            {
                await GetZatratsAsync();
            }
        }

        partial void OnSelectedTypeChanged(TypeZatrat? value)
        {
            ApplyFilter();
        }
        partial void OnSelectedVideoChanged(VideoProductDTO? value)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            GettedZatratDTO.Clear();

            IEnumerable<ZatratDTO> result = GettedDefaultProducts;

            if (SelectedType != null)
            {
                result = result.Where(x => x.TypeZatratId == SelectedType.Id);
            }

            if (SelectedVideo != null)
            {
                result = result.Where(x => x.VideoProductId == SelectedVideo.Id);
            }

            foreach (var item in result)
            {
                GettedZatratDTO.Add(item);
            }
        }
    }
}
