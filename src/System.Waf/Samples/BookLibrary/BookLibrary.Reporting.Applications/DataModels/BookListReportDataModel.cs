using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Reporting.Applications.DataModels;

public class BookListReportDataModel(IReadOnlyList<Book> books) : Model
{
    public IReadOnlyList<Book> Books { get; } = books;

    public int BookCount => Books.Count;
}
