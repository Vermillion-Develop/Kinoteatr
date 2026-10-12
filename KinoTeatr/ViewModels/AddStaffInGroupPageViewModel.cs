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
    public partial class AddStaffInGroupPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public ObservableCollection<StaffDTO> GettedStaff { get; } = new ObservableCollection<StaffDTO>();
        public ObservableCollection<Specialization> GettedRoles { get; } = new ObservableCollection<Specialization>();

        [ObservableProperty]
        public StaffDTO? _selectedStaff;

        [ObservableProperty]
        public Specialization? _selectedRole;
        private CinemaGroupDTO _selectedGroup;

        public AddStaffInGroupPageViewModel(CinemaGroupDTO group)
        {
            _apiService = new ApiService();
            _selectedGroup = group;
            _ = GetStaffRolesAsync();
        }

        [RelayCommand]
        public async Task AddStaffInGroup(Window? currentWindow)
        {
            if (SelectedRole == null || SelectedStaff == null) 
            {
                return;
            }
            var result = await _apiService.AddStaffInGroup(SelectedStaff.Id, _selectedGroup.VideoId, _selectedGroup.Id, SelectedRole.Id);
            if (result)
            {
                currentWindow?.Close(true);
            }
            else
            {
                Debug.WriteLine("Ошибка");
            }
        }

        public async Task GetStaffRolesAsync()
        {
            var result = await _apiService.GetSpecializationsAsync();
            if (result == null)
            {
                return;
            }
            GettedRoles.Clear();
            foreach (var item in result) 
            {
                GettedRoles.Add(item);
            }
            _ = GetStaffDtoAsync();
        }

        public async Task GetStaffDtoAsync()
        {
            var result = await _apiService.GetStaffAsync();
            if (result == null)
            {
                return;
            }
            GettedStaff.Clear();
            foreach (var item in result)
            {
                GettedStaff.Add(item);
            }
        }
    } 
}
