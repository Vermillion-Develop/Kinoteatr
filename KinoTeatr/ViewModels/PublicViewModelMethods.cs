using Avalonia.Controls;
using KinoTeatr.Views;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KinoTeatr.Services;

namespace KinoTeatr.ViewModels
{
    public static partial class PublicViewModelMethods
    {

        public static ICommand OpenStaffCommand { get; } = new AsyncRelayCommand<Window>(OpenStaff);
        public static ICommand LeaveFromSessionCommand { get; } = new AsyncRelayCommand<Window>(LeaveFromSession);
        public static ICommand CloseCurrentWindowCommand { get; } = new AsyncRelayCommand<Window>(CloseCurrentWindow);
  

        public static async Task OpenStaff(Window? currentWindow)
        {
            StaffPageWindow staffWin = new StaffPageWindow
            {
                DataContext = new StaffPageViewModel()
            };
            staffWin.Show();
            currentWindow?.Hide();
        }

        public static async Task LeaveFromSession(Window? currentWindow)
        {
            StartPageWindow startPage = new StartPageWindow
            {
                DataContext = new StartPageViewModel()
            };
            UserSession.CurrentStaff = null;
            startPage.Show();
            currentWindow?.Hide();
        }

        public static async Task CloseCurrentWindow(Window? currentWindow)
        {
            currentWindow?.Close();
        }
    }
}
