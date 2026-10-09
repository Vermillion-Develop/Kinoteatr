using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class UpdateStaffPageViewModel : ViewModelBase
    {

        private readonly IApiService _apiService;
        public ObservableCollection<Specialization> GettedSpecializations { get; } = new ObservableCollection<Specialization>();

        [ObservableProperty]
        public StaffDTO? selectedStaff;

        [ObservableProperty]
        public Specialization? _selectedSpec;
        [ObservableProperty]
        public string? _family;
        [ObservableProperty]
        public string? _name;
        [ObservableProperty]
        public string? _father;
        [ObservableProperty]
        public string? _phone;
        [ObservableProperty]
        public string? _email;
        [ObservableProperty]
        public decimal? _stavka;
        [ObservableProperty]
        public string? _datalog;
        [ObservableProperty]
        public string? _password;
        [ObservableProperty]
        public string? _secondPassword;
        [ObservableProperty]
        public string? _errorMessage = null;

        public UpdateStaffPageViewModel(StaffDTO? selectedStaff)
        {
            _apiService = new ApiService();
            SelectedStaff = selectedStaff;
            _ = GetRoleAsync();
        }
        public async Task GetRoleAsync()
        {
            var result = await _apiService.GetSpecializationsAsync();
            if (result == null)
            {
                Debug.WriteLine("Полученный массив специальностей был пуст");
                return;
            }
            GettedSpecializations.Clear();
            foreach (var special in result)
            {
                GettedSpecializations.Add(special);
            }
        }

        [RelayCommand]
        public async Task UpdateStaff(Window? currentWindow)
        {
            if (string.IsNullOrWhiteSpace(Family) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Email) || Stavka == null || string.IsNullOrWhiteSpace(Datalog) || string.IsNullOrWhiteSpace(Password)) 
            {
                ErrorMessage = "Заполните все поля";
                return;
            }
            if(Password != SecondPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return;
            }
            Debug.WriteLine("ЕБАНЫЙ ПЕЛЬМЕНЬ " + Datalog);
            var result = await _apiService.UpdateStaffAsync(Family, Name, Father, SelectedSpec?.Id, Phone, Email, Stavka, SelectedStaff?.DataLogId, Password, SelectedStaff?.Id,  Datalog);
            if(result == true)
            {
                currentWindow?.Close(true);
                Debug.WriteLine("Вроде все гладко");
                return;
            }

            ErrorMessage = "Ошибка при редактировании сотрудника";
            Debug.WriteLine(ErrorMessage);
            return;
        }
    }
}
