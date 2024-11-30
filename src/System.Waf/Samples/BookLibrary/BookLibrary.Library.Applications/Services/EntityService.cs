using System.ComponentModel.Composition;
using Microsoft.EntityFrameworkCore;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Services;

[Export(typeof(IEntityService)), Export]
internal class EntityService : IEntityService
{
    private ObservableCollection<Book>? books;
    private ObservableCollection<Person>? persons;

    public DbContext? BookLibraryContext { get; set; }

    public ObservableCollection<Book> Books => books ??= BookLibraryContext!.Set<Book>().Local.ToObservableCollection();

    public ObservableCollection<Person> Persons => persons ??= BookLibraryContext!.Set<Person>().Local.ToObservableCollection();

    public async Task LoadBooks(CancellationToken cancellation = default)
    {
        // Simulate a delay or an error
        //await Task.Delay(2000, cancellation);
        //throw new InvalidOperationException("Loading failed");
        await BookLibraryContext!.Set<Book>().Include(x => x.LendTo).LoadAsync(cancellation);
    }

    public async Task LoadPersons(CancellationToken cancellation = default)
    {
        // Simulate a delay or an error
        //await Task.Delay(2000, cancellation);
        //throw new InvalidOperationException("Loading failed");
        await BookLibraryContext!.Set<Person>().LoadAsync(cancellation);
    }

    public async Task SaveChanges(CancellationToken cancellation = default)
    {
        // Simulate a delay or an error
        //await Task.Delay(2000, cancellation).ConfigureAwait(false);
        //throw new InvalidOperationException("Loading failed");
        await BookLibraryContext!.SaveChangesAsync(cancellation);
    }
}
