using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views;

public class MockBookListView : MockView<BookListViewModel>, IBookListView
{
    public bool FirstCellHasFocus { get; set; }

    public void FocusFirstCell() => FirstCellHasFocus = true;

    public void SingleSelect(BookDataModel? book)
    {
        ViewModel.SelectedBook = book;
        foreach (var x in ViewModel.SelectedBooks.ToArray()) ViewModel.RemoveSelectedBook(x);
        if (book is not null) ViewModel.AddSelectedBook(book);
    }
}
