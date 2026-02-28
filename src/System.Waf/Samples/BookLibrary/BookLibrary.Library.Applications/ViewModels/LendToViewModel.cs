using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class LendToViewModel : ViewModel<ILendToView>
{
    private readonly DelegateCommand okCommand;
    private bool dialogResult;

    public LendToViewModel(ILendToView view) : base(view)
    {
        okCommand = new(OkHandler);
    }

    public string Title => ApplicationInfo.ProductName;

    public ICommand OkCommand => okCommand;

    [DisallowNull] public Book? Book
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            IsLendTo = value.LendTo != null;
        }
    }

    public IReadOnlyList<Person> Persons { get; set; } = [];

    public bool IsLendTo { get; set => SetProperty(ref field, value); }

    public Person? SelectedPerson { get; set => SetProperty(ref field, value); }

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
