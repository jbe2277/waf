using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Waf.Writer.Applications.Services
{
    public interface IZoomCommands : INotifyPropertyChanged
    {
        IReadOnlyList<string> DefaultZooms { get; }

        double Zoom { get; set; }
        
        ICommand ZoomInCommand { get; }

        ICommand ZoomOutCommand { get; }

        ICommand FitToWidthCommand { get; }
    }
}
