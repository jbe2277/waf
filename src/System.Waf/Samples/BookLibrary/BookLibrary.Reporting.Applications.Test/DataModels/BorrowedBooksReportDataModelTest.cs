using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Reporting.Applications.DataModels;

namespace Test.BookLibrary.Reporting.Applications.DataModels
{
    [TestClass]
    public class BorrowedBooksReportDataModelTest : ReportingTest
    {
        [TestMethod]
        public void GroupBooksTest()
        {
            var harryPotter = new Person() { Firstname = "Harry", Lastname = "Potter", Email = "harry.potter@hogwarts.edu" };
            var books = new List<Book>()
            {
                new() { LendTo = harryPotter, Title = "Serenity, Vol 1: Those Left Behind", Author = "Joss Whedon, Brett Matthews, Will Conrad" },
                new() { LendTo = harryPotter, Title = "Star Wars - Heir to the Empire", Author = "Timothy Zahn" },
                new() { LendTo = harryPotter, Title = "The Lord of the Rings - The Fellowship of the Ring", Author = "J.R.R. Tolkien" }
            };
            var dataModel = new BorrowedBooksReportDataModel(books);
            Assert.AreEqual(harryPotter, dataModel.GroupedBooks.Single().Key);
            Assert.AreEqual(3, dataModel.GroupedBooks.Single().Count());
        }
    }
}
