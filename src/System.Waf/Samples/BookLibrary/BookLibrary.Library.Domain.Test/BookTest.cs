using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Domain;
using Waf.BookLibrary.Library.Domain.Properties;

namespace Test.BookLibrary.Library.Domain
{
    [TestClass]
    public class BookTest
    {
        [TestMethod]
        public void GeneralBookTest()
        {
            var book = new Book();
            Assert.IsNotNull(book.Id);

            book.Title = "Star Wars - Heir to the Empire";
            book.Author = "Timothy Zahn";
            book.Publisher = "Spectra";
            book.PublishDate = new DateTime(1992, 5, 1);
            book.Isbn = "0553296124";
            book.Language = Language.English;
            book.Pages = 416;

            Assert.IsTrue(book.Validate());

            Assert.AreEqual("Star Wars - Heir to the Empire by Timothy Zahn", book.ToString(null, CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void BookLanguagePropertyChangedTest()
        {
            var book = new Book() { Language = Language.English };
            AssertHelper.PropertyChangedEvent(book, x => x.Language, () => book.Language = Language.German);
            Assert.AreEqual(Language.German, book.Language);
        }

        [TestMethod]
        public void BookLendToPropertyChangedTest()
        {
            var book = new Book();
            Assert.IsNull(book.LendTo);

            var person = new Person();
            AssertHelper.PropertyChangedEvent(book, x => x.LendTo, () => book.LendTo = person);
            Assert.AreEqual(person, book.LendTo);
        }

        [TestMethod]
        public void BookTitleValidationTest()
        {
            var book = new Book();
            book.Validate();

            Assert.AreEqual("", book.Title);
            Assert.AreEqual(Resources.TitleMandatory, book.GetErrors(nameof(book.Title)).Single().ErrorMessage);

            book.Title = new string('A', 101);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.TitleMaxLength, "Title", 100), book.GetErrors(nameof(book.Title)).Single().ErrorMessage);

            book.Title = new string('A', 100);
            Assert.IsFalse(book.GetErrors(nameof(book.Title)).Any());
        }

        [TestMethod]
        public void BookAuthorValidationTest()
        {
            var book = new Book();
            book.Validate();

            Assert.AreEqual("", book.Author);
            Assert.AreEqual(Resources.AuthorMandatory, book.GetErrors(nameof(book.Author)).Single().ErrorMessage);

            book.Author = new string('A', 101);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.AuthorMaxLength, "Author", 100), book.GetErrors(nameof(book.Author)).Single().ErrorMessage);

            book.Author = new string('A', 100);
            Assert.IsFalse(book.GetErrors(nameof(book.Author)).Any());
        }

        [TestMethod]
        public void BookPublisherValidationTest()
        {
            var book = new Book();
            book.Validate();

            Assert.IsNull(book.Publisher);
            Assert.IsFalse(book.GetErrors(nameof(book.Publisher)).Any());

            book.Publisher = new string('A', 101);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.PublisherMaxLength, "Publisher", 100), book.GetErrors(nameof(book.Publisher)).Single().ErrorMessage);

            book.Publisher = new string('A', 100);
            Assert.IsFalse(book.GetErrors(nameof(book.Publisher)).Any());
        }

        [TestMethod]
        public void BookPublishDateValidationTest()
        {
            var book = new Book();
            book.Validate();

            Assert.IsFalse(book.GetErrors(nameof(book.PublishDate)).Any());

            book.PublishDate = new DateTime(1752, 1, 1);
            Assert.IsTrue(book.GetErrors(nameof(book.PublishDate)).Any());

            book.PublishDate = new DateTime(2000, 1, 1);
            Assert.IsFalse(book.GetErrors(nameof(book.PublishDate)).Any());

            book.PublishDate = new DateTime(1000, 1, 1);
            Assert.IsTrue(book.GetErrors(nameof(book.PublishDate)).Any());
        }

        [TestMethod]
        public void BookIsbnValidationTest()
        {
            var book = new Book();
            book.Validate();

            Assert.IsNull(book.Isbn);
            Assert.IsFalse(book.GetErrors(nameof(book.Isbn)).Any());

            book.Isbn = new string('A', 15);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.IsbnMaxLength, "Isbn", 14), book.GetErrors(nameof(book.Isbn)).Single().ErrorMessage);

            book.Isbn = new string('A', 14);
            Assert.IsFalse(book.GetErrors(nameof(book.Isbn)).Any());
        }

        [TestMethod]
        public void BookPagesValidationTest()
        {
            var book = new Book();
            book.Validate();

            Assert.AreEqual(0, book.Pages);
            Assert.IsFalse(book.GetErrors(nameof(book.Pages)).Any());

            book.Pages = -1;
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.PagesEqualOrLarger, "Pages", 0), book.GetErrors(nameof(book.Pages)).Single().ErrorMessage);

            book.Pages = 400;
            Assert.IsFalse(book.GetErrors(nameof(book.Pages)).Any());
        }
    }
}
