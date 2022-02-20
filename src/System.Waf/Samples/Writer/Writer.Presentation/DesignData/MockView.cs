using System.Waf.Applications;

namespace Waf.Writer.Presentation.DesignData;

public class MockView : IView
{
    public object? DataContext { get; set; }
}
