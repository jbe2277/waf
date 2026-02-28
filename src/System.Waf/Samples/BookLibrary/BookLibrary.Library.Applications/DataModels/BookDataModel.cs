using System.Windows.Input;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.DataModels;

public class BookDataModel(Book book, ICommand lendToCommand) : Model
{
    public Book Book { get; } = book ?? throw new ArgumentNullException(nameof(book));

    public ICommand LendToCommand { get; } = lendToCommand ?? throw new ArgumentNullException(nameof(lendToCommand));
}
