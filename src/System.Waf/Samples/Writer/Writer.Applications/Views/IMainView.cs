using System.Waf.Applications;

namespace Waf.Writer.Applications.Views;

public interface IMainView : IView
{
    ContentViewState ContentViewState { get; set; }
}
