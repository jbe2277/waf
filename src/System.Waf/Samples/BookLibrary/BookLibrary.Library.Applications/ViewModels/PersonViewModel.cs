using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class PersonViewModel(IPersonView view) : ViewModel<IPersonView>(view)
{
    public bool IsEnabled => Person != null;

    public bool IsValid { get; set => SetProperty(ref field, value); } = true;

    public Person? Person
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            RaisePropertyChanged(nameof(IsEnabled));
        }
    }

    public ICommand? CreateNewEmailCommand { get; set; }
}
