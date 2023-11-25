using Microsoft.EntityFrameworkCore;

namespace Waf.BookLibrary.Library.Applications.Data;

public class BookLibraryContext(DbContextOptions options, Action<ModelBuilder>? onModelCreating = null) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        onModelCreating?.Invoke(modelBuilder);
    }
}
