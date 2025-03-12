using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

public class MockPrintPreviewView : MockView<PrintPreviewViewModel>, IPrintPreviewView
{
    public bool FitToWidthCalled { get; set; }

    public void FitToWidth() => FitToWidthCalled = true;
}
