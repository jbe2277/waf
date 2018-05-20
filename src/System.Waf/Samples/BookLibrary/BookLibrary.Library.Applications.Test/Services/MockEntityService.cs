using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Services
{
    [Export(typeof(IEntityService))]
    internal class MockEntityService : IEntityService
    {
        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();

        public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>();
    }
}
