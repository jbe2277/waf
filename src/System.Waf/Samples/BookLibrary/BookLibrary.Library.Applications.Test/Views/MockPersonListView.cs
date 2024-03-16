using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views;

[Export(typeof(IPersonListView)), Export]
public class MockPersonListView : MockView<PersonListViewModel>, IPersonListView
{
    public bool FirstCellHasFocus { get; set; }

    public void FocusFirstCell() => FirstCellHasFocus = true;
}
