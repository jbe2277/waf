using Microsoft.EntityFrameworkCore;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal class BookLibraryContext : DbContext
    {
        private readonly string connectionString;

        public BookLibraryContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool HasChanges() 
        {
            ChangeTracker.DetectChanges();
            return ChangeTracker.HasChanges(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(PersonMapping.Builder);
            modelBuilder.Entity<Book>(BookMapping.Builder);
        }
    }
}
