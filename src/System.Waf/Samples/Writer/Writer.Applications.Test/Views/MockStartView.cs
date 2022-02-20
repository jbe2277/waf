using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

[Export(typeof(IStartView))]
public class MockStartView : MockView, IStartView
{
}
