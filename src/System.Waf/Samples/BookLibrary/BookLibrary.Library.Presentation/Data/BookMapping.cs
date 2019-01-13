using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.Data
{
    internal static class BookMapping
    {
        public static void Builder(EntityTypeBuilder<Book> builder)
        {
            builder.Ignore(t => t.Errors);
            builder.Ignore(t => t.HasErrors);
            
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title").HasMaxLength(100);
            builder.Property(t => t.Author).HasColumnName("Author").HasMaxLength(100);
            builder.Property(t => t.Publisher).HasColumnName("Publisher").HasMaxLength(100);
            builder.Property(t => t.PublishDate).HasColumnName("PublishDate");
            builder.Property(t => t.Isbn).HasColumnName("Isbn").HasMaxLength(14);
            builder.Property(t => t.Language).HasColumnName("Language");
            builder.Property(t => t.Pages).HasColumnName("Pages");

            builder.HasOne(t => t.LendTo).WithMany().HasForeignKey("PersonId");

            builder.ToTable("Book");
        }
    }
}
