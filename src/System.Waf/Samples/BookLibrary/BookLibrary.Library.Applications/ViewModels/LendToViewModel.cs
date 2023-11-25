using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

[Export, PartCreationPolicy(CreationPolicy.NonShared)]
public class LendToViewModel : ViewModel<ILendToView>
{
    private readonly DelegateCommand okCommand;
    private Book? book;
    private bool isWasReturned;
    private bool isLendTo;
    private Person? selectedPerson;
    private bool dialogResult;

    [ImportingConstructor]
    public LendToViewModel(ILendToView view) : base(view)
    {
        okCommand = new(OkHandler);
    }

    public string Title => ApplicationInfo.ProductName;

    public ICommand OkCommand => okCommand;

    [DisallowNull]
    public Book? Book
    {
        get => book;
        set
        {
            if (!SetProperty(ref book, value)) return;
            if (value!.LendTo == null) IsLendTo = true;
            else IsWasReturned = true;
        }
    }

    public IReadOnlyList<Person>? Persons { get; set; }

    public bool IsWasReturned
    {
        get => isWasReturned;
        set
        {
            if (!SetProperty(ref isWasReturned, value)) return;
            IsLendTo = !value;
        }
    }

    public bool IsLendTo
    {
        get => isLendTo;
        set
        {
            if (!SetProperty(ref isLendTo, value)) return;
            IsWasReturned = !value;
        }
    }

    public Person? SelectedPerson
    {
        get => selectedPerson;
        set => SetProperty(ref selectedPerson, value);
    }

    public bool ShowDialog(object owner)
    {
        ViewCore.ShowDialog(owner);
        return dialogResult;
    }

    private void OkHandler()
    {
        dialogResult = true;
        if (IsWasReturned) SelectedPerson = null;
        ViewCore.Close();
    }
}
