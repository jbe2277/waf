using System.ComponentModel.Composition;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

[Export, PartCreationPolicy(CreationPolicy.NonShared)]
public class PrintPreviewViewModel : ZoomViewModel<IPrintPreviewView>
{
    [ImportingConstructor]
    public PrintPreviewViewModel(IPrintPreviewView view, IShellService shellService) : base(view, shellService)
    {
    }

    public IRichTextDocument Document { get; set; } = null!;

    protected override void FitToWidthCore()
    {
        base.FitToWidthCore();
        ViewCore.FitToWidth();
    }
}
