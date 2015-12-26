using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Domain.Properties;

namespace Waf.BookLibrary.Library.Domain
{
    public class Book : ValidatableModel, IFormattable
    {
        private string title;
        private string author;
        private string publisher;
        private DateTime publishDate;
        private string isbn;
        private Language language;
        private int pages;
        private Person lendTo;


        public Book()
        {
            this.Id = Guid.NewGuid();
            this.title = "";
            this.author = "";
            this.publishDate = DateTime.Now;
        }


        public Guid Id { get; private set; }

        [Required(ErrorMessageResourceName = "TitleMandatory", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessageResourceName = "TitleMaxLength", ErrorMessageResourceType = typeof(Resources))]
        public string Title 
        {
            get { return title; }
            set { SetPropertyAndValidate(ref title, value); }
        }

        [Required(ErrorMessageResourceName = "AuthorMandatory", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessageResourceName = "AuthorMaxLength", ErrorMessageResourceType = typeof(Resources))]
        public string Author 
        {
            get { return author; }
            set { SetPropertyAndValidate(ref author, value); }
        }

        [StringLength(100, ErrorMessageResourceName = "PublisherMaxLength", ErrorMessageResourceType = typeof(Resources))]
        public string Publisher 
        {
            get { return publisher; }
            set { SetPropertyAndValidate(ref publisher, value); }
        }

        [CustomValidation(typeof(Book), "ValidatePublishDate")]
        public DateTime PublishDate
        {
            get { return publishDate; }
            set { SetPropertyAndValidate(ref publishDate, value); }
        }

        [StringLength(14, ErrorMessageResourceName = "IsbnMaxLength", ErrorMessageResourceType = typeof(Resources))]
        public string Isbn 
        { 
            get { return isbn; }
            set { SetPropertyAndValidate(ref isbn, value); }
        }

        public Language Language
        {
            get { return language; }
            set { SetPropertyAndValidate(ref language, value); }
        }

        [Range(0, int.MaxValue, ErrorMessageResourceName = "PagesEqualOrLarger", ErrorMessageResourceType = typeof(Resources))]
        public int Pages 
        {
            get { return pages; }
            set { SetPropertyAndValidate(ref pages, value); }
        }

        public Person LendTo
        {
            get { return lendTo; }
            set { SetPropertyAndValidate(ref lendTo, value); }
        }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, Resources.BookToString, Title, Author);
        }

        public static ValidationResult ValidatePublishDate(DateTime value, ValidationContext context)
        {
            var minValue = new DateTime(1753, 1, 1);
            var maxValue = new DateTime(9999, 12, 31);
            if (value < minValue || value > maxValue) 
            { 
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, Resources.ValueMustBeBetween, value, minValue, maxValue)); 
            }
            return ValidationResult.Success;
        }
    }
}
