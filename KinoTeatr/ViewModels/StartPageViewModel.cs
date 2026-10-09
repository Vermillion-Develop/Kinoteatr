using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.Services;
using KinoTeatr.Views;
using KinoTeatrShared.ModelsDTO;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class StartPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public StartPageViewModel()
        {
            _apiService = new ApiService();
        }

        [ObservableProperty]
        public string? _login;
        [ObservableProperty]
        public string? _password;
        [ObservableProperty]
        public string? _errorMessage;

        [RelayCommand]
        public async Task LoginIn(Window? currentWindow)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password)) 
            {
                ErrorMessage = "Заполните поля!";
                return;
            }

            StaffDTO? user = await _apiService.LoginInAsync(Login, Password);
            if(user != null)
            {
                UserSession.CurrentStaff = user;
                PrimaryPageWindow primaryPageWindow = new PrimaryPageWindow()
                {
                    DataContext = new PrimaryPageViewModel()
                };

                primaryPageWindow.Show();
                currentWindow?.Hide();

            }
            else
            {
                ErrorMessage = "Неверный логин/пароль";
                return;
            }
        }
    }
}
