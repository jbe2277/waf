using System.Data.Common;
using System.Data.Entity;

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal class BookLibraryContext : DbContext
    {
        public BookLibraryContext(DbConnection dbConnection) : base(dbConnection, false)
        {
            Database.SetInitializer<BookLibraryContext>(null);
        }

        public bool HasChanges() 
        {
            ChangeTracker.DetectChanges();
            return ChangeTracker.HasChanges(); 
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PersonMapping());
            modelBuilder.Configurations.Add(new BookMapping());
        }
    }
}
