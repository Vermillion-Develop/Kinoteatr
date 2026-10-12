using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatrShared.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class AddNewGroupViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<VideoProductDTO> GettedProjects { get; } = new ObservableCollection<VideoProductDTO>();

        [ObservableProperty]
        public VideoProductDTO? _selectedProduct;

        [ObservableProperty]
        public string? _nameGroup;

        [ObservableProperty]
        public string? _errorMessage;
        public AddNewGroupViewModel()
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
        }

        [RelayCommand]
        public async Task AddNewGroupAsync(Window? currentWindow)
        {
            if(SelectedProduct == null || string.IsNullOrWhiteSpace(NameGroup))
            {
                ErrorMessage = "Заполните поля!";
                return;
            }
            var result = await _apiService.AddNewGroupAsync(NameGroup, SelectedProduct?.Id);
            if (result)
            {
                currentWindow?.Close(true);
            }
            else
            {
                Debug.WriteLine("Возникла ошибка добавления группы");
                return;
            }
        }
    }
}
