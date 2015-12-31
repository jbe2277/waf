using System.Collections.Generic;
using System.Linq;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Reporting.Applications.DataModels
{
    public class BookListReportDataModel : Model
    {
        public BookListReportDataModel(IReadOnlyList<Book> books)
        {
            Books = books;
        }


        public IReadOnlyList<Book> Books { get; }

        public int BookCount => Books.Count;
    }
}
