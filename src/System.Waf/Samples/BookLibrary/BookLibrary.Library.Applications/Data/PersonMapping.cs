using System.Data.Entity.ModelConfiguration;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal class PersonMapping : EntityTypeConfiguration<Person>
    {
        public PersonMapping()
        {
            HasKey(t => t.Id);

            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Firstname).HasMaxLength(30).HasColumnName("Firstname");
            Property(t => t.Lastname).HasMaxLength(30).HasColumnName("Lastname");
            Property(t => t.Email).HasMaxLength(100).HasColumnName("Email");

            Ignore(t => t.HasErrors);

            ToTable("Person");
        }
    }
}
