using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Reporting.Applications.DataModels;

namespace Waf.BookLibrary.Reporting.Presentation.DesignData
{
    public class SampleBorrowedBooksReportDataModel : BorrowedBooksReportDataModel
    {
        private static readonly Person harryPotter = new() { Firstname = "Harry", Lastname = "Potter", Email = "harry.potter@hogwarts.edu" };
        
        public SampleBorrowedBooksReportDataModel() : base(new List<Book>()
            {
                new() { LendTo = harryPotter, Title = "Serenity, Vol 1: Those Left Behind", Author = "Joss Whedon, Brett Matthews, Will Conrad" },
                new() { LendTo = harryPotter, Title = "Star Wars - Heir to the Empire", Author = "Timothy Zahn" },
                new() { LendTo = harryPotter, Title = "The Lord of the Rings - The Fellowship of the Ring", Author = "J.R.R. Tolkien" }
            })
        {
        }
    }
}
