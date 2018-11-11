using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using Waf.BookLibrary.Library.Applications.Data;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Services
{
    [Export(typeof(IEntityService)), Export]
    internal class EntityService : IEntityService
    {
        private ObservableCollection<Book> books;
        private ObservableCollection<Person> persons;

        public BookLibraryContext BookLibraryContext { get; set; }
        
        public ObservableCollection<Book> Books
        {
            get 
            {
                if (books == null && BookLibraryContext != null)
                {
                    // Uncomment this line to generate the ViewCache: GenerateViewCache();
                    BookLibraryContext.Set<Book>().Include(x => x.LendTo).Load();
                    books = BookLibraryContext.Set<Book>().Local;
                }
                return books;
            }
        }

        public ObservableCollection<Person> Persons
        {
            get 
            {
                if (persons == null && BookLibraryContext != null)
                {
                    BookLibraryContext.Set<Person>().Load();
                    persons = BookLibraryContext.Set<Person>().Local;
                }
                return persons;
            }
        }

        private void GenerateViewCache()
        {
            var dbContext = BookLibraryContext;
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            var mappingHashValue = mappingCollection.ComputeMappingHashValue();
            var edmSchemaError = new List<EdmSchemaError>();
            var views = mappingCollection.GenerateViews(edmSchemaError);
        }
    }
}
