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
    public partial class GroupsPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        public ObservableCollection<CinemaGroupDTO> GettedGroups { get; } = new ObservableCollection<CinemaGroupDTO>();
        [ObservableProperty]
        public CinemaGroupDTO? _selectedGroup;
        [ObservableProperty]
        public string? _errorMessage;


        public GroupsPageViewModel()
        {
            _apiService = new ApiService();
            _ = GetGroupsAsync();
        }

        public async Task GetGroupsAsync()
        {
            var result = await _apiService.GetAllGroupsAsync();
            if (result == null) 
            {
                Debug.WriteLine("Массив пуст");
                return;
            }
            GettedGroups.Clear();
            foreach(var group in result)
            {
                GettedGroups.Add(group);
            }
        }

        [RelayCommand]
        public async Task OpenAddGroupFormAsync(Window? currentWindow)
        {
            if (currentWindow != null) 
            {
                AddNewGroupPageWindow newGroup = new AddNewGroupPageWindow()
                {
                    DataContext = new AddNewGroupViewModel()
                };
                if (currentWindow != null)
                {
                    ErrorMessage = "";
                    bool dialog = await newGroup.ShowDialog<bool>(currentWindow);
                    if (dialog)
                    {
                        await GetGroupsAsync();
                    }
                }
            }
        }

        [RelayCommand]
        public async Task UpdateGroupFormAsync(Window? currentWindow)
        {
            if (currentWindow != null && SelectedGroup != null) 
            {
                UpdateGroupPageWindow newGroup = new UpdateGroupPageWindow()
                {
                    DataContext = new UpdateGroupViewModel(SelectedGroup)
                };
                if (currentWindow != null )
                {
                    ErrorMessage = "";
                    bool dialog = await newGroup.ShowDialog<bool>(currentWindow);
                    if (dialog)
                    {
                        await GetGroupsAsync();
                    }
                }
            }
            else
            {
                ErrorMessage = "Выберите группу!";
                return;
            }
        }
        [RelayCommand]
        public async Task OpenStaffGroupFormAsync()
        {
            if(SelectedGroup == null)
            {
                ErrorMessage = "Выберите группу!";
                return;
            }
            StaffInGroupPageWindow staffGroup = new StaffInGroupPageWindow()
            {
                DataContext = new StaffInGroupViewModel(SelectedGroup)
            };
            ErrorMessage = "";
            staffGroup.Show();
        }
    }
}
