using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Waf.Applications;
using Microsoft.EntityFrameworkCore;
using Waf.BookLibrary.Library.Presentation.Data;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.Data;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.Services
{
    [Export(typeof(IDBContextService))]
    internal class DBContextService : IDBContextService
    {
        private const string ResourcesDirectoryName = "Resources";

        public DbContext GetBookLibraryContext(out string dataSourcePath)
        {
            // Create directory for the database.
            string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationInfo.Company, ApplicationInfo.ProductName);
            if (!Directory.Exists(Path.Combine(dataDirectory, ResourcesDirectoryName))) Directory.CreateDirectory(Path.Combine(dataDirectory, ResourcesDirectoryName));
            
            // Copy the template database file into the DataDirectory when it doesn't exists.
            var connectionString = ConfigurationManager.ConnectionStrings["BookLibraryContext"].ConnectionString;
            dataSourcePath = Path.Combine(dataDirectory, connectionString);
            if (!File.Exists(dataSourcePath))
            {
                string dbFile = Path.GetFileName(dataSourcePath);
                File.Copy(Path.Combine(ApplicationInfo.ApplicationPath, ResourcesDirectoryName, dbFile), dataSourcePath);
            }

            var options = new DbContextOptionsBuilder<BookLibraryContext>().UseSqlite("Data Source=" + dataSourcePath).Options;
            return new BookLibraryContext(options, modelBuilder =>
            {
                modelBuilder.UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
                modelBuilder.Entity<Person>(PersonMapping.Builder);
                modelBuilder.Entity<Book>(BookMapping.Builder);
            });
        }
    }
}
