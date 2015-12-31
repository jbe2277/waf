using System;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.DataModels
{
    public class BookDataModel : Model
    {
        public BookDataModel(Book book, ICommand lendToCommand)
        {
            if (book == null) { throw new ArgumentNullException(nameof(book)); }
            if (lendToCommand == null) { throw new ArgumentNullException(nameof(lendToCommand)); }

            Book = book;
            LendToCommand = lendToCommand;
        }


        public Book Book { get; }

        public ICommand LendToCommand { get; }
    }
}
