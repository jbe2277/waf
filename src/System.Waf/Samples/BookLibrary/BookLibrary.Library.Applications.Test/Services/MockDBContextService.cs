using System.ComponentModel.Composition;
using Microsoft.EntityFrameworkCore;
using Waf.BookLibrary.Library.Applications.Data;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Services
{
    [Export, Export(typeof(IDBContextService))]
    public class MockDBContextService : IDBContextService
    {
        public DbContext GetBookLibraryContext(out string dataSourcePath)
        {
            dataSourcePath = @"C:\Test.db";
            var options = new DbContextOptionsBuilder<BookLibraryContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            var context = new BookLibraryContext(options, modelBuilder =>
            {
                modelBuilder.Entity<Book>().Ignore(x => x.Errors).Ignore(x => x.HasErrors);
                modelBuilder.Entity<Person>().Ignore(x => x.Errors).Ignore(x => x.HasErrors);
            });
            return context;
        }
    }
}
