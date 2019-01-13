using System.ComponentModel.Composition;
using Microsoft.EntityFrameworkCore;
using Waf.BookLibrary.Library.Applications.Data;
using Waf.BookLibrary.Library.Applications.Services;

namespace Waf.BookLibrary.Library.Presentation.Services
{
    [Export(typeof(IDBContextService))]
    internal class DBContextService : IDBContextService
    {
        public DbContext GetBookLibraryContext(string dataSourcePath)
        {
            return new BookLibraryContext(dataSourcePath);
        }
    }
}
