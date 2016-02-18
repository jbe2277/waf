using Waf.BookLibrary.Library.Domain;
using System.Collections.ObjectModel;

namespace Waf.BookLibrary.Library.Applications.Services
{
    public interface IEntityService
    {
        ObservableCollection<Book> Books { get; }

        ObservableCollection<Person> Persons { get; }
    }
}
