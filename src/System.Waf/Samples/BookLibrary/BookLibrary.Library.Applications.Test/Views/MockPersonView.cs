using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views;

[Export(typeof(IPersonView)), Export]
public class MockPersonView : MockView<PersonViewModel>, IPersonView
{
}
