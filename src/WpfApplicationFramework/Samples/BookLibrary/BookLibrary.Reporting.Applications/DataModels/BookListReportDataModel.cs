using System.Collections.Generic;
using System.Linq;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Reporting.Applications.DataModels
{
    public class BookListReportDataModel : Model
    {
        private readonly IReadOnlyList<Book> books;


        public BookListReportDataModel(IReadOnlyList<Book> books)
        {
            this.books = books;
        }


        public IReadOnlyList<Book> Books { get { return books; } }

        public int BookCount { get { return books.Count(); } }
    }
}
