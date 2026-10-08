using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using KinoTeatrShared.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class AddNewStaffPageViewModel : ViewModelBase
    {

        private readonly IApiService _apiService;
        public ObservableCollection<Specialization> GettedSpecializations { get; } = new ObservableCollection<Specialization>();
        [ObservableProperty]
        public Specialization? _selectedSpec;
        public AddNewStaffPageViewModel()
        {
            _apiService = new ApiService();
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
    }
}
