using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views
{
    [Export(typeof(IPersonListView)), Export]
    public class MockPersonListView : MockView, IPersonListView
    {
        public bool FirstCellHasFocus { get; set; }

        public void FocusFirstCell() => FirstCellHasFocus = true;
    }
}
