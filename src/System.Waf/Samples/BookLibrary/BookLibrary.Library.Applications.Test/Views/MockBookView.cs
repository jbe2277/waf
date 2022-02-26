using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views;

[Export(typeof(IBookView)), Export]
public class MockBookView : MockView, IBookView
{
}
