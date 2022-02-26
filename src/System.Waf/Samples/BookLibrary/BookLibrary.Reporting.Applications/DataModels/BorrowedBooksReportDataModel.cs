using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Reporting.Applications.DataModels;

public class BorrowedBooksReportDataModel
{
    public BorrowedBooksReportDataModel(IEnumerable<Book> books)
    {
        GroupedBooks = (from book in books
                        group book by book.LendTo into grp
                        where grp.Key != null
                        orderby grp.Key.Firstname
                        select grp
                        ).ToArray();
    }

    public IReadOnlyList<IGrouping<Person, Book>> GroupedBooks { get; }
}
