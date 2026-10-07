using Avalonia.Controls;
using KinoTeatr.Views;
using CommunityToolkit.Mvvm.Input;
using KinoTeatr.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KinoTeatr.ViewModels
{
    public static partial class PublicViewModelMethods
    {

        public static ICommand OpenStaffCommand { get; } = new AsyncRelayCommand<Window>(OpenStaff);
  

        public static async Task OpenStaff(Window? currentWindow)
        {
            StaffPageWindow staffWin = new StaffPageWindow
            {
                DataContext = new StaffPageViewModel()
            };
            staffWin.Show();
            currentWindow?.Hide();
        }
    }
}
