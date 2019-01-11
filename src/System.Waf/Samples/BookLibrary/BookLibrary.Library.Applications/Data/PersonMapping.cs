using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal static class PersonMapping
    {
        public static void Builder(EntityTypeBuilder<Person> builder)
        {
            builder.Ignore(t => t.Errors);
            builder.Ignore(t => t.HasErrors);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Firstname).HasMaxLength(30).HasColumnName("Firstname");
            builder.Property(t => t.Lastname).HasMaxLength(30).HasColumnName("Lastname");
            builder.Property(t => t.Email).HasMaxLength(100).HasColumnName("Email");

            builder.ToTable("Person");
        }
    }
}
