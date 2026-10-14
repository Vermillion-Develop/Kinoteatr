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
    public partial class AddNewAreaPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<StreetDTO> GettedStreets { get; } = new ObservableCollection<StreetDTO>();
        public ObservableCollection<TypeArea> GettedTypes { get; } = new ObservableCollection<TypeArea>();

        [ObservableProperty]
        public StreetDTO? _selectedStreet;

        [ObservableProperty]
        public TypeArea? _selectedType;

        [ObservableProperty]
        public decimal? _cost;

        [ObservableProperty]
        public int? _capacity;

        [ObservableProperty]
        public string? _adressArea;

        [ObservableProperty]
        public string? _errorMessage;

       
        public AddNewAreaPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetStreetsAsync();
        }

        public async Task GetStreetsAsync()
        {
            var result = await _apiService.GetStreetsAsync();
            if(result == null)
            {
                return;
            }
            GettedStreets.Clear();
            foreach(var street in result)
            {
                GettedStreets.Add(street);
            }
            _ = GetTypesAreaAsync();
        }

        public async Task GetTypesAreaAsync()
        {
            var result = await _apiService.GetTypesAreaAsync();
            if (result == null)
            {
                return;
            }
            GettedTypes.Clear();
            foreach (var street in result)
            {
                GettedTypes.Add(street);
            }
        }

        [RelayCommand]
        public async Task AddAreaAsync(Window? currentWindow)
        {
            if (SelectedStreet == null || SelectedType == null)
            {
                ErrorMessage = "Выберите улицу и тип площадки!";
                return;
            }
            if (string.IsNullOrWhiteSpace(AdressArea) || Cost == null || Capacity == null)
            {
                ErrorMessage = "Заполните поля!";
                return;
            }
            var result = await _apiService.AddNewArea(SelectedType.Id, AdressArea, SelectedStreet.Id, Cost, Capacity);
            if (result)
            {
                currentWindow?.Close(true);
            }
        }

      
    }
}
