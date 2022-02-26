using System.Windows.Input;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.DataModels
{
    public class BookDataModel : Model
    {
        public BookDataModel(Book book, ICommand lendToCommand)
        {
            Book = book ?? throw new ArgumentNullException(nameof(book));
            LendToCommand = lendToCommand ?? throw new ArgumentNullException(nameof(lendToCommand));
        }

        public Book Book { get; }

        public ICommand LendToCommand { get; }
    }
}
