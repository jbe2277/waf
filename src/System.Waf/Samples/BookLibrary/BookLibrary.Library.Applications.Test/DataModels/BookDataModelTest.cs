using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Applications.DataModels;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Domain;
using System.Windows.Input;
using System.Waf.Applications;

namespace Test.BookLibrary.Library.Applications.DataModels
{
    [TestClass]
    public class BookDataModelTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Book book = new Book();
            ICommand dummyCommand = new DelegateCommand(() => {});
            
            AssertHelper.ExpectedException<ArgumentNullException>(() => new BookDataModel(null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new BookDataModel(book, null));

            BookDataModel bookDataModel = new BookDataModel(book, dummyCommand);
            Assert.AreEqual(book, bookDataModel.Book);
            Assert.AreEqual(dummyCommand, bookDataModel.LendToCommand);
        }
    }
}
