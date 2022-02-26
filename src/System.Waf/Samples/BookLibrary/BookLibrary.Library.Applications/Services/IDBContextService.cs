using Microsoft.EntityFrameworkCore;

namespace Waf.BookLibrary.Library.Applications.Services;

public interface IDBContextService
{
    DbContext GetBookLibraryContext(out string dataSourcePath);
}
