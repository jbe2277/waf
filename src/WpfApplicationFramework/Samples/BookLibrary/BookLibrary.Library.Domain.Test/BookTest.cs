using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Globalization;
using System.Waf.Foundation;
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
            Book book = new Book();
            Assert.IsNotNull(book.Id);

            book.Title = "Star Wars - Heir to the Empire";
            book.Author = "Timothy Zahn";
            book.Publisher = "Spectra";
            book.PublishDate = new DateTime(1992, 5, 1);
            book.Isbn = "0553296124";
            book.Language = Language.English;
            book.Pages = 416;

            Assert.IsTrue(book.Validate());

            Assert.AreEqual("Star Wars - Heir to the Empire by Timothy Zahn", 
                book.ToString(null, CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void BookLanguagePropertyChangedTest()
        {
            Book book = new Book();
            book.Language = Language.English;

            AssertHelper.PropertyChangedEvent(book, x => x.Language, () => book.Language = Language.German);
            Assert.AreEqual(Language.German, book.Language);
        }

        [TestMethod]
        public void BookLendToPropertyChangedTest()
        {
            Book book = new Book();
            Assert.IsNull(book.LendTo);

            Person person = new Person();
            AssertHelper.PropertyChangedEvent(book, x => x.LendTo, () => book.LendTo = person);
            Assert.AreEqual(person, book.LendTo);
        }

        [TestMethod]
        public void BookTitleValidationTest()
        {
            Book book = new Book();
            book.Validate();

            Assert.AreEqual("", book.Title);
            Assert.AreEqual(Resources.TitleMandatory, book.GetErrors("Title").Single().ErrorMessage);

            book.Title = new string('A', 101);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.TitleMaxLength, "Title", 100),
                book.GetErrors("Title").Single().ErrorMessage);

            book.Title = new string('A', 100);
            Assert.IsFalse(book.GetErrors("Title").Any());
        }

        [TestMethod]
        public void BookAuthorValidationTest()
        {
            Book book = new Book();
            book.Validate();

            Assert.AreEqual("", book.Author);
            Assert.AreEqual(Resources.AuthorMandatory, book.GetErrors("Author").Single().ErrorMessage);

            book.Author = new string('A', 101);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.AuthorMaxLength, "Author", 100),
                book.GetErrors("Author").Single().ErrorMessage);

            book.Author = new string('A', 100);
            Assert.IsFalse(book.GetErrors("Author").Any());
        }

        [TestMethod]
        public void BookPublisherValidationTest()
        {
            Book book = new Book();
            book.Validate();

            Assert.IsNull(book.Publisher);
            Assert.IsFalse(book.GetErrors("Publisher").Any());

            book.Publisher = new string('A', 101);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.PublisherMaxLength, "Publisher", 100),
                book.GetErrors("Publisher").Single().ErrorMessage);

            book.Publisher = new string('A', 100);
            Assert.IsFalse(book.GetErrors("Publisher").Any());
        }

        [TestMethod]
        public void BookPublishDateValidationTest()
        {
            Book book = new Book();
            book.Validate();

            Assert.IsFalse(book.GetErrors("PublishDate").Any());

            book.PublishDate = new DateTime(1752, 1, 1);
            Assert.IsTrue(book.GetErrors("PublishDate").Any());

            book.PublishDate = new DateTime(2000, 1, 1);
            Assert.IsFalse(book.GetErrors("PublishDate").Any());

            book.PublishDate = new DateTime(1000, 1, 1);
            Assert.IsTrue(book.GetErrors("PublishDate").Any());
        }

        [TestMethod]
        public void BookIsbnValidationTest()
        {
            Book book = new Book();
            book.Validate();

            Assert.IsNull(book.Isbn);
            Assert.IsFalse(book.GetErrors("Isbn").Any());

            book.Isbn = new string('A', 15);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.IsbnMaxLength, "Isbn", 14),
                book.GetErrors("Isbn").Single().ErrorMessage);

            book.Isbn = new string('A', 14);
            Assert.IsFalse(book.GetErrors("Isbn").Any());
        }

        [TestMethod]
        public void BookPagesValidationTest()
        {
            Book book = new Book();
            book.Validate();

            Assert.AreEqual(0, book.Pages);
            Assert.IsFalse(book.GetErrors("Pages").Any());

            book.Pages = -1;
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.PagesEqualOrLarger, "Pages", 0),
                book.GetErrors("Pages").Single().ErrorMessage);

            book.Pages = 400;
            Assert.IsFalse(book.GetErrors("Pages").Any());
        }
    }
}
