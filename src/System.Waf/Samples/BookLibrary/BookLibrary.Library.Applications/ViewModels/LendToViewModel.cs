using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class LendToViewModel : ViewModel<ILendToView>
{
    private readonly DelegateCommand okCommand;
    private Book? book;
    private bool isLendTo;
    private Person? selectedPerson;
    private bool dialogResult;

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
            IsLendTo = value.LendTo != null;
        }
    }

    public IReadOnlyList<Person> Persons { get; set; } = [];

    public bool IsLendTo
    {
        get => isLendTo;
        set => SetProperty(ref isLendTo, value);
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
        if (!IsLendTo) SelectedPerson = null;
        ViewCore.Close();
    }
}
