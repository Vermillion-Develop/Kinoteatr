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
        public static ICommand OpenProjectsCommand { get; } = new AsyncRelayCommand<Window>(OpenProjects);
        public static ICommand OpenStaffGroupCommand { get; } = new AsyncRelayCommand<Window>(OpenStaffGroup);
        public static ICommand OpenAreasCommand { get; } = new AsyncRelayCommand<Window>(OpenAreas);
        public static ICommand OpenProcatCommand { get; } = new AsyncRelayCommand<Window>(OpenProcat);
        public static ICommand OpenZatratCommand { get; } = new AsyncRelayCommand<Window>(OpenZatrat);
        public static ICommand OpenGraphicCommand { get; } = new AsyncRelayCommand<Window>(OpenGraphic);
  

        public static async Task OpenStaff(Window? currentWindow)
        {
            StaffPageWindow staffWin = new StaffPageWindow
            {
                DataContext = new StaffPageViewModel()
            };
            staffWin.Show();
            currentWindow?.Hide();
        }
        public static async Task OpenProcat(Window? currentWindow)
        {
            ProcatPageWindow procatWin = new ProcatPageWindow
            {
                DataContext = new ProcatPageViewModel()
            };
            procatWin.Show();
            currentWindow?.Hide();
        }
        public static async Task OpenGraphic(Window? currentWindow)
        {
            GraphicGroupPageWindow graphicWin = new GraphicGroupPageWindow
            {
                DataContext = new GraphicGroupViewModel()
            };
            graphicWin.Show();
            currentWindow?.Hide();
        }
        public static async Task OpenStaffGroup(Window? currentWindow)
        {
            GroupsPageWindow staffWin = new GroupsPageWindow
            {
                DataContext = new GroupsPageViewModel()
            };
            staffWin.Show();
            currentWindow?.Hide();
        }

        public static async Task OpenZatrat(Window? currentWindow)
        {
            ZatratsPageWindow zatratWin = new ZatratsPageWindow
            {
                DataContext = new ZatratsPageViewModel()
            };
            zatratWin.Show();
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

        public static async Task OpenProjects(Window? currentWindow)
        {
            ProjectPageWindow projectsWin = new ProjectPageWindow()
            {
                DataContext = new ProjectPageViewModel()
            };
            projectsWin.Show();
            currentWindow?.Hide();
        }
        public static async Task OpenAreas(Window? currentWindow)
        {
            AreaPageWindow areaWin = new AreaPageWindow()
            {
                DataContext = new AreaPageViewModel()
            };
            areaWin.Show();
            currentWindow?.Hide();
        }

        public static async Task CloseCurrentWindow(Window? currentWindow)
        {
            currentWindow?.Close();
        }
    }
}
