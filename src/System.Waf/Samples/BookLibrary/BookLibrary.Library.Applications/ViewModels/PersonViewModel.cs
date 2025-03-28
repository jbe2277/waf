using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class PersonViewModel : ViewModel<IPersonView>
{
    private bool isValid = true;
    private Person? person;

    public PersonViewModel(IPersonView view) : base(view)
    {
    }

    public bool IsEnabled => Person != null;

    public bool IsValid
    {
        get => isValid;
        set => SetProperty(ref isValid, value);
    }

    public Person? Person
    {
        get => person;
        set
        {
            if (!SetProperty(ref person, value)) return;
            RaisePropertyChanged(nameof(IsEnabled));
        }
    }

    public ICommand? CreateNewEmailCommand { get; set; }
}
