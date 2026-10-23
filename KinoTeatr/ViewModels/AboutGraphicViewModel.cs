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
    public partial class AboutGraphicViewModel : ViewModelBase
    {

        [ObservableProperty]
        public GraphicVideoDTO? _graphicVideo;
        public AboutGraphicViewModel(GraphicVideoDTO _graphicVideo)
        {
            GraphicVideo = _graphicVideo;
        }
        
    }
}
