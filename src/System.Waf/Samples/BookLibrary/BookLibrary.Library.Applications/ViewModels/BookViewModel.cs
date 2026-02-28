using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class BookViewModel(IBookView view) : ViewModel<IBookView>(view)
{
    public bool IsEnabled => Book != null;

    public bool IsValid { get; set => SetProperty(ref field, value); } = true;

    public Book? Book 
    { 
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            RaisePropertyChanged(nameof(IsEnabled));
        }
    }

    public ICommand? LendToCommand { get; set; }
}
