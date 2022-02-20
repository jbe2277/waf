using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

[Export(typeof(IMainView))]
public class MockMainView : MockView, IMainView
{
    public ContentViewState ContentViewState { get; set; }
}
