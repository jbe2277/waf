using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

public class PrintPreviewViewModel(IPrintPreviewView view, IShellService shellService) : ZoomViewModel<IPrintPreviewView>(view, shellService)
{
    public IRichTextDocument Document { get; set; } = null!;

    protected override void FitToWidthCore()
    {
        base.FitToWidthCore();
        ViewCore.FitToWidth();
    }
}
