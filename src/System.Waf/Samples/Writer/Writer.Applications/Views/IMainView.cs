using System.Waf.Applications;

namespace Waf.Writer.Applications.Views;

public enum ContentViewState
{
    StartViewVisible,
    DocumentViewVisible
}

public interface IMainView : IView
{
    ContentViewState ContentViewState { get; set; }
}
