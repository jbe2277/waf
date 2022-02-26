using Microsoft.EntityFrameworkCore;

namespace Waf.BookLibrary.Library.Applications.Data
{
    public class BookLibraryContext : DbContext
    {
        private readonly Action<ModelBuilder>? onModelCreating;

        public BookLibraryContext(DbContextOptions options, Action<ModelBuilder>? onModelCreating = null) : base(options)
        {
            this.onModelCreating = onModelCreating;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            onModelCreating?.Invoke(modelBuilder);
        }
    }
}
