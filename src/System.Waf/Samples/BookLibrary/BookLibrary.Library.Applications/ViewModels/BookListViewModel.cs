using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class BookListViewModel(IBookListView view) : ViewModel<IBookListView>(view)
{
    private readonly ObservableList<BookDataModel> selectedBooks = [];

    public IReadOnlyObservableList<BookDataModel> SelectedBooks => selectedBooks;

    public bool IsValid { get; set => SetProperty(ref field, value); } = true;

    public IReadOnlyList<BookDataModel>? Books { get; set; }

    public BookDataModel? SelectedBook { get; set => SetProperty(ref field, value); }

    public ICommand? AddNewCommand { get; set; }

    public ICommand? RemoveCommand { get; set; }

    public string FilterText { get; set => SetProperty(ref field, value); } = "";

    public Func<IEnumerable<BookDataModel>, IOrderedEnumerable<BookDataModel>>? Sort { get; set => SetProperty(ref field, value); }

    public void Focus() => ViewCore.FocusFirstCell();

    public bool Filter(BookDataModel bookDataModel)
    {
        var book = bookDataModel.Book;
        return string.IsNullOrEmpty(FilterText)
            || (!string.IsNullOrEmpty(book.Title) && book.Title.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
            || (!string.IsNullOrEmpty(book.Author) && book.Author.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase));
    }

    public void AddSelectedBook(BookDataModel bookDataModel) => selectedBooks.Add(bookDataModel);

    public void RemoveSelectedBook(BookDataModel bookDataModel) => selectedBooks.Remove(bookDataModel);
}
