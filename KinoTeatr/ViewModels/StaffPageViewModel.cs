using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.Views;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class StaffPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<StaffDTO> GettedStaffs { get; } = new ObservableCollection<StaffDTO>();
        public ObservableCollection<StaffDTO> DefaultStaffs { get; } = new ObservableCollection<StaffDTO>();

        [ObservableProperty]
        public StaffDTO? _selectedStaff;

        [ObservableProperty]
        public string? _searchStroke;
        public StaffPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetStaffAsync();
        }

        public async Task GetStaffAsync()
        {
            var result = await _apiService.GetStaffAsync();
            if (result == null)
            {
                Debug.WriteLine("Массив сотрудников был пуст");
                return;
            }

            GettedStaffs.Clear();
            DefaultStaffs.Clear();
            foreach (var staff in result) 
            {
                GettedStaffs.Add(staff);
                DefaultStaffs.Add(staff);
            }
            
        }

        [RelayCommand]
        public async Task OpenAddStaffFormAsync(Window? currentWindow) //Открыть форму добавления сотрудника
        {
            
            AddNewStaffPageWindow addStaff = new AddNewStaffPageWindow()
            {
                DataContext = new AddNewStaffPageViewModel()
            };

            if(currentWindow != null)
            {
                bool dialogres = await addStaff.ShowDialog<bool>(currentWindow);
                if (dialogres)
                {
                    await GetStaffAsync();
                }
            }
        }

        [RelayCommand]
        public async Task OpenUpdateStaffFormAsync(Window? currentWindow)  // Открыть форму обновления сотрудника
        {
            UpdateStaffPageWindow updateStaff = new UpdateStaffPageWindow()
            {
                DataContext = new UpdateStaffPageViewModel(SelectedStaff)
            };
            if(SelectedStaff == null)
            {
                Debug.WriteLine("Выберите сотрудника для изменения");
                return;
            }
            if (currentWindow != null)
            {
                bool dialogres = await updateStaff.ShowDialog<bool>(currentWindow);
                if (dialogres)
                {
                    await GetStaffAsync();
                }
            }
        }

        [RelayCommand]
        public async Task DeleteSelectedStaffAsync()
        {
            var result = await _apiService.DeleteStaffAsync(SelectedStaff?.Id);
            Debug.WriteLine(SelectedStaff?.Id);
            if (result)
            {
                Debug.WriteLine("Сотрудник удален");
                _ = GetStaffAsync();
            }
            else
            {
                Debug.WriteLine("Ошибка удаления сотрудника");
            }
        }

        partial void OnSearchStrokeChanged(string? value) // Метод поиска
        {
            GettedStaffs.Clear();
            foreach (var filtredStaff in DefaultStaffs)
            {
                if (filtredStaff.Family.ToLower().StartsWith(value.ToLower()) ||
                    filtredStaff.Name.ToLower().StartsWith(value.ToLower()) ||
                    filtredStaff.Father.ToLower().StartsWith(value.ToLower()))
                {
                    GettedStaffs.Add(filtredStaff);
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    GettedStaffs.Add(filtredStaff);
                }
            }
        }
    }
}
