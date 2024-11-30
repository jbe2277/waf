using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Services;

public interface IEntityService
{
    ObservableCollection<Book> Books { get; }

    ObservableCollection<Person> Persons { get; }

    Task LoadBooks(CancellationToken cancellation = default);

    Task LoadPersons(CancellationToken cancellation = default);

    Task SaveChanges(CancellationToken cancellation = default);
}
