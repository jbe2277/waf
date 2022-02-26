using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Reporting.Applications.DataModels;

namespace Waf.BookLibrary.Reporting.Presentation.DesignData
{
    public class SampleBookListReportDataModel : BookListReportDataModel
    {
        public SampleBookListReportDataModel() : base(new List<Book>
            {
                new()
                {
                    Title = "Serenity, Vol 1: Those Left Behind",
                    Author = "Joss Whedon, Brett Matthews, Will Conrad",
                    PublishDate = new DateTime(2006, 8, 2)
                },
                new()
                {
                    Title = "Star Wars - Heir to the Empire",
                    Author = "Timothy Zahn",
                    PublishDate = new DateTime(1992, 1, 5),
                    LendTo = new Person
                    {
                        Firstname = "Harry",
                        Lastname = "Potter"
                    }
                },
                new()
                {
                    Title = "The Lord of the Rings - The Fellowship of the Ring",
                    Author = "J.R.R. Tolkien",
                    PublishDate = new DateTime(1986, 12, 8)
                },
            })
        {
        }
    }
}
