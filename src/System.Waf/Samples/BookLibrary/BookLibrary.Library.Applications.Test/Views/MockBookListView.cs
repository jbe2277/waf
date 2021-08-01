using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views
{
    [Export(typeof(IBookListView)), Export]
    public class MockBookListView : MockView, IBookListView
    {
        public bool FirstCellHasFocus { get; set; }

        public void FocusFirstCell() => FirstCellHasFocus = true;
    }
}
