using System.Data.Entity.ModelConfiguration;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal class BookMapping : EntityTypeConfiguration<Book>
    {
        public BookMapping()
        {
            HasKey(t => t.Id);

            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Title).HasColumnName("Title").HasMaxLength(100);
            Property(t => t.Author).HasColumnName("Author").HasMaxLength(100);
            Property(t => t.Publisher).HasColumnName("Publisher").HasMaxLength(100);
            Property(t => t.PublishDate).HasColumnName("PublishDate");
            Property(t => t.Isbn).HasColumnName("Isbn").HasMaxLength(14);
            Property(t => t.Language).HasColumnName("Language");
            Property(t => t.Pages).HasColumnName("Pages");

            Ignore(t => t.HasErrors);

            HasOptional(t => t.LendTo).WithMany().Map(t => t.MapKey("PersonId"));

            ToTable("Book");
        }
    }
}
