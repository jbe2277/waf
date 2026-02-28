using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Waf.BookLibrary.Library.Domain.Properties;

namespace Waf.BookLibrary.Library.Domain;

public class Book : ValidatableModel, IFormattable
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required(ErrorMessageResourceName = nameof(Resources.TitleMandatory), ErrorMessageResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.TitleMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string Title { get; set => SetPropertyAndValidate(ref field, value); } = "";

    [Required(ErrorMessageResourceName = nameof(Resources.AuthorMandatory), ErrorMessageResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.AuthorMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string Author { get; set => SetPropertyAndValidate(ref field, value); } = "";

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.PublisherMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string? Publisher { get; set => SetPropertyAndValidate(ref field, value); }

    [CustomValidation(typeof(Book), nameof(ValidatePublishDate))]
    public DateTime PublishDate { get; set => SetPropertyAndValidate(ref field, value); } = DateTime.Now;

    [StringLength(14, ErrorMessageResourceName = nameof(Resources.IsbnMaxLength), ErrorMessageResourceType = typeof(Resources))]
    public string? Isbn { get; set => SetPropertyAndValidate(ref field, value); }

    public Language Language { get; set => SetPropertyAndValidate(ref field, value); }

    [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(Resources.PagesEqualOrLarger), ErrorMessageResourceType = typeof(Resources))]
    public int Pages { get; set => SetPropertyAndValidate(ref field, value); }

    public Person? LendTo { get; set => SetPropertyAndValidate(ref field, value); }

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
