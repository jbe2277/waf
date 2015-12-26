using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.BookLibrary.Library.Applications.Services;
using System.Collections.ObjectModel;
using Waf.BookLibrary.Library.Domain;
using System.ComponentModel.Composition;

namespace Test.BookLibrary.Library.Applications.Services
{
    [Export(typeof(IEntityService))]
    internal class MockEntityService : IEntityService
    {
        public MockEntityService()
        {
            Books = new ObservableCollection<Book>();
            Persons = new ObservableCollection<Person>();
        }


        public ObservableCollection<Book> Books { get; private set; }

        public ObservableCollection<Person> Persons { get; private set; }
    }
}
