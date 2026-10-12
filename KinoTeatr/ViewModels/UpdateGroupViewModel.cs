using Avalonia.Controls;
using CinemaHub.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinoTeatrShared.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KinoTeatr.ViewModels
{
    public partial class UpdateGroupViewModel : ViewModelBase
    {

        private readonly IApiService _apiService;

        [ObservableProperty]
        public CinemaGroupDTO? _cinemaGroupDTO;

        [ObservableProperty]
        public VideoProductDTO? _selectedVideo;

        [ObservableProperty]
        public string? _groupName;

        [ObservableProperty]
        public string? _errorMessage;

        public ObservableCollection<VideoProductDTO> GettedVideos { get; } = new ObservableCollection<VideoProductDTO>();
        public UpdateGroupViewModel(CinemaGroupDTO selectedGroup)
        {
            _apiService = new ApiService(); 
            CinemaGroupDTO = selectedGroup;
            _ = GetAllVideosAsync();
        }

        public async Task GetAllVideosAsync()
        {
            var result = await _apiService.GetVideoProductDTOsAsync();
            if (result == null) 
            {
                return;
            }
            GettedVideos.Clear();
            foreach(var video in result)
            {
                GettedVideos.Add(video);
            }
        }

        [RelayCommand]
        public async Task UpdateGroupAsync(Window? currentWindow)
        {
            if (CinemaGroupDTO == null || SelectedVideo == null)
            {
                Debug.WriteLine("Ошибка на фронтенде!");
                return;
            }
            var result = await _apiService.UpdateGroupAsync(GroupName, SelectedVideo.Id, CinemaGroupDTO.Id);
            if (result)
            {
                currentWindow?.Close(true);
            }
            else
            {
                Debug.WriteLine("Какая то ошибка!");
                return;
            }
        }
        
    }
}
