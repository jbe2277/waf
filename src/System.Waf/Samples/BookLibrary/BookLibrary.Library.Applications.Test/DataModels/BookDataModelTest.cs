using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.DataModels
{
    [TestClass]
    public class BookDataModelTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var book = new Book();
            var dummyCommand = new DelegateCommand(() => {});
            
            AssertHelper.ExpectedException<ArgumentNullException>(() => new BookDataModel(null!, null!));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new BookDataModel(book, null!));

            var bookDataModel = new BookDataModel(book, dummyCommand);
            Assert.AreEqual(book, bookDataModel.Book);
            Assert.AreEqual(dummyCommand, bookDataModel.LendToCommand);
        }
    }
}
