using Microsoft.EntityFrameworkCore;

namespace Waf.BookLibrary.Library.Applications.Services
{
    public interface IDBContextService
    {
        DbContext GetBookLibraryContext(string dataSourcePath);
    }
}
