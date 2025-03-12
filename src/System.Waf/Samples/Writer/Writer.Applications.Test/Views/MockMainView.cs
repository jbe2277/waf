using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

public class MockMainView : MockView<MainViewModel>, IMainView
{
    public ContentViewState ContentViewState { get; set; }
}
