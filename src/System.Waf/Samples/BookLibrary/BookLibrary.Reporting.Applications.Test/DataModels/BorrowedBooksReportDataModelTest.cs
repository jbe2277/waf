using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Reporting.Applications.DataModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Reporting.Applications.DataModels
{
    [TestClass]
    public class BorrowedBooksReportDataModelTest : ReportingTest
    {
        [TestMethod]
        public void GroupBooksTest()
        {
            Person harryPotter = new Person() { Firstname = "Harry", Lastname = "Potter", Email = "harry.potter@hogwarts.edu" };
        
            var books = new List<Book>()
            {
                new Book() { LendTo = harryPotter, Title = "Serenity, Vol 1: Those Left Behind", Author = "Joss Whedon, Brett Matthews, Will Conrad" },
                new Book() { LendTo = harryPotter, Title = "Star Wars - Heir to the Empire", Author = "Timothy Zahn" },
                new Book() { LendTo = harryPotter, Title = "The Lord of the Rings - The Fellowship of the Ring", Author = "J.R.R. Tolkien" }
            };

            var dataModel = new BorrowedBooksReportDataModel(books);
            Assert.AreEqual(harryPotter, dataModel.GroupedBooks.Single().Key);
            Assert.AreEqual(3, dataModel.GroupedBooks.Single().Count());
        }
    }
}
