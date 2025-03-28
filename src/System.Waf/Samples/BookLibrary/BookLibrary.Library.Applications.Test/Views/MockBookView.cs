using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views;

public class MockBookView : MockView<BookViewModel>, IBookView
{
}
