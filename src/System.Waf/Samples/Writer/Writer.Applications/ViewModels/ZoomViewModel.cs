using System.Globalization;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Applications.ViewModels;

public abstract class ZoomViewModel<T> : ViewModel<T>, IZoomCommands where T : IView
{
    private const double minZoom = 0.2;
    private const double maxZoom = 16;
    private readonly DelegateCommand zoomInCommand;
    private readonly DelegateCommand zoomOutCommand;
    private readonly DelegateCommand fitToWidthCommand;

    protected ZoomViewModel(T view, IShellService shellService) : base(view)
    {
        ShellService = shellService;
        DefaultZooms = new ReadOnlyCollection<string>(new[] { 2, 1.5, 1.25, 1, 0.75, 0.5 }.Select(d => string.Format(CultureInfo.CurrentCulture, "{0:P0}", d)).ToArray());
        zoomInCommand = new DelegateCommand(ZoomIn, CanZoomIn);
        zoomOutCommand = new DelegateCommand(ZoomOut, CanZoomOut);
        fitToWidthCommand = new DelegateCommand(FitToWidth);
    }

    public IReadOnlyList<string> DefaultZooms { get; }

    public ICommand ZoomInCommand => zoomInCommand;

    public ICommand ZoomOutCommand => zoomOutCommand;

    public ICommand FitToWidthCommand => fitToWidthCommand;

    public bool IsVisible
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            ShellService.ActiveZoomCommands = field ? this : null;
        }
    }

    public double Zoom
    {
        get;
        set
        {
            if (field == value) return;
            field = Math.Max(value, minZoom);
            field = Math.Min(field, maxZoom);
            RaisePropertyChanged();
            DelegateCommand.RaiseCanExecuteChanged(zoomInCommand, zoomOutCommand);
        }
    } = 1;

    protected IShellService ShellService { get; }

    protected virtual void FitToWidthCore() { }

    private bool CanZoomIn() => Zoom < maxZoom;

    private void ZoomIn() => Zoom = Math.Floor(Math.Round((Zoom + 0.1) * 10, 3)) / 10;

    private bool CanZoomOut() => Zoom > minZoom;

    private void ZoomOut() => Zoom = Math.Ceiling(Math.Round((Zoom - 0.1) * 10, 3)) / 10;

    private void FitToWidth() => FitToWidthCore();
}
