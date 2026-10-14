using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.Views;
using KinoTeatrShared.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class AreaPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<AreaDTO> GettedAreas { get; } = new ObservableCollection<AreaDTO>();
        [ObservableProperty]
        public CinemaGroupDTO? _selectedArea;
        [ObservableProperty]
        public string? _errorMessage;


        public AreaPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetAreasAsync();
        }

        public async Task GetAreasAsync()
        {
            var result = await _apiService.GetAreaDTOsAsync();
            if (result == null)
            {
                Debug.WriteLine("Массив пуст");
                return;
            }
            GettedAreas.Clear();
            foreach (var area in result)
            {
                GettedAreas.Add(area);
            }
        }

        [RelayCommand]
        public async Task OpenAddAreaAsync(Window? currentWindow)
        {
            if (currentWindow != null)
            {
                AddNewAreaPageWindow newArea = new AddNewAreaPageWindow()
                {
                    DataContext = new AddNewAreaPageViewModel()
                };
                if (currentWindow != null)
                {
                    ErrorMessage = "";
                    bool dialog = await newArea.ShowDialog<bool>(currentWindow);
                    if (dialog)
                    {
                        await GetAreasAsync();
                    }
                }
            }

        }
    }
}
