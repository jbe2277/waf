﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Waf.BookLibrary.Library.Applications.Data;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Services;

public class MockDBContextService : IDBContextService
{
    public Action<DbContext>? ContextCreated { get; set; }

    public DbContext GetBookLibraryContext(out string dataSourcePath)
    {
        dataSourcePath = @"C:\Test.db";
        var options = new DbContextOptionsBuilder<BookLibraryContext>().UseInMemoryDatabase(databaseName: "TestDatabase", databaseRoot: new InMemoryDatabaseRoot()).Options;
        var context = new BookLibraryContext(options, modelBuilder =>
        {
            modelBuilder.Entity<Book>().Ignore(x => x.Errors).Ignore(x => x.HasErrors);
            modelBuilder.Entity<Person>().Ignore(x => x.Errors).Ignore(x => x.HasErrors);
        });
        ContextCreated?.Invoke(context);
        return context;
    }
}
