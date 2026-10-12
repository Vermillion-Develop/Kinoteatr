using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.Views;
using KinoTeatrShared.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class StaffInGroupViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<StaffGroupDTO> GettedStaffInGroups { get; } = new ObservableCollection<StaffGroupDTO>();
        [ObservableProperty]
        public CinemaGroupDTO _selectedGroup;
        public StaffInGroupViewModel(CinemaGroupDTO group)
        {
            SelectedGroup = group;
            _apiService = new ApiService();
            _ = GetStaffGroupsAsync();
        }

        public async Task GetStaffGroupsAsync()
        {
            
            var result = await _apiService.GetStaffGroupDTOsAsync(SelectedGroup.Id);
            if (result == null) 
            {
                Debug.WriteLine("Массив сотрудников был пуст!");
                return;
            }
            GettedStaffInGroups.Clear();
            foreach (var staff in result) 
            {
                GettedStaffInGroups.Add(staff);
            }
        }

        [RelayCommand]
        public async Task OpenAddStaffFormAsync(Window? currentWindow)
        {
            if(currentWindow != null && SelectedGroup != null) 
            {
                AddStaffInGroupPageWindow staffgr = new AddStaffInGroupPageWindow()
                {
                    DataContext = new AddStaffInGroupPageViewModel(SelectedGroup)
                };
                bool dialog = await staffgr.ShowDialog<bool>(currentWindow);
                if (dialog)
                {
                    await GetStaffGroupsAsync();
                }
            }
        }

        


    }
}
