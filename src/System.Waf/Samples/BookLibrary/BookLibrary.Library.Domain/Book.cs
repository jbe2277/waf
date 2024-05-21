using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Waf.BookLibrary.Library.Domain.Properties;

namespace Waf.BookLibrary.Library.Domain;

public class Book : ValidatableModel, IFormattable
{
    private string title = "";
    private string author = "";
    private string? publisher;
    private DateTime publishDate;
    private string? isbn;
    private Language language;
    private int pages;
    private Person? lendTo;

    public Book()
    {
        Id = Guid.NewGuid();
        publishDate = DateTime.Now;
    }

    public Guid Id { get; private set; }

    [Required(ErrorMessageResourceName = nameof(Resources.TitleMandatory), ErrorMessageResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.TitleMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string Title
    {
        get => title;
        set => SetPropertyAndValidate(ref title, value);
    }

    [Required(ErrorMessageResourceName = nameof(Resources.AuthorMandatory), ErrorMessageResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.AuthorMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string Author
    {
        get => author;
        set => SetPropertyAndValidate(ref author, value);
    }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.PublisherMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string? Publisher
    {
        get => publisher;
        set => SetPropertyAndValidate(ref publisher, value);
    }

    [CustomValidation(typeof(Book), nameof(ValidatePublishDate))]
    public DateTime PublishDate
    {
        get => publishDate;
        set => SetPropertyAndValidate(ref publishDate, value);
    }

    [StringLength(14, ErrorMessageResourceName = nameof(Resources.IsbnMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string? Isbn
    {
        get => isbn;
        set => SetPropertyAndValidate(ref isbn, value);
    }

    public Language Language
    {
        get => language;
        set => SetPropertyAndValidate(ref language, value);
    }

    [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(Resources.PagesEqualOrLarger), ErrorMessageResourceType = typeof(Resources))]
    public int Pages
    {
        get => pages;
        set => SetPropertyAndValidate(ref pages, value);
    }

    public Person? LendTo
    {
        get => lendTo;
        set => SetPropertyAndValidate(ref lendTo, value);
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => string.Format(formatProvider, Resources.BookToString, Title, Author);

    public static ValidationResult? ValidatePublishDate(DateTime value, ValidationContext _)
    {
        var minValue = new DateTime(1753, 1, 1);
        var maxValue = new DateTime(9999, 12, 31);
        if (value < minValue || value > maxValue)
        {
            return new(string.Format(CultureInfo.CurrentCulture, Resources.ValueMustBeBetween, value, minValue, maxValue), [nameof(PublishDate)]);
        }
        return ValidationResult.Success;
    }
}
