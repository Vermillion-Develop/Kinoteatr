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
            foreach (var staff in result) 
            {
                GettedStaffs.Add(staff);
            }
            
        }

        [RelayCommand]
        public async Task OpenAddStaffForm()
        {
            AddNewStaffPageWindow addStaff = new AddNewStaffPageWindow()
            {
                DataContext = new AddNewStaffPageViewModel()
            };
            addStaff.Show();
        }

        
    }
}
