using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class PersonListViewModel : ViewModel<IPersonListView>
{
    private readonly ObservableList<Person> selectedPersons = [];
    private bool isValid = true;
    private Person? selectedPerson;
    private string filterText = "";
    private Func<IEnumerable<Person>, IOrderedEnumerable<Person>>? sort;

    public PersonListViewModel(IPersonListView view) : base(view)
    {
    }

    public IReadOnlyList<Person> SelectedPersons => selectedPersons;

    public bool IsValid
    {
        get => isValid;
        set => SetProperty(ref isValid, value);
    }

    public IReadOnlyList<Person>? Persons { get; set; }

    public Person? SelectedPerson
    {
        get => selectedPerson;
        set => SetProperty(ref selectedPerson, value);
    }

    public ICommand? AddNewCommand { get; set; }

    public ICommand? RemoveCommand { get; set; }

    public ICommand? CreateNewEmailCommand { get; set; }

    public string FilterText
    {
        get => filterText;
        set => SetProperty(ref filterText, value);
    }

    public Func<IEnumerable<Person>, IOrderedEnumerable<Person>>? Sort
    {
        get => sort;
        set => SetProperty(ref sort, value);
    }

    public void Focus() => ViewCore.FocusFirstCell();

    public bool Filter(Person person)
    {
        return string.IsNullOrEmpty(filterText)
            || (!string.IsNullOrEmpty(person.Firstname) && person.Firstname.Contains(filterText, StringComparison.CurrentCultureIgnoreCase))
            || (!string.IsNullOrEmpty(person.Lastname) && person.Lastname.Contains(filterText, StringComparison.CurrentCultureIgnoreCase));
    }

    public void AddSelectedPerson(Person person) => selectedPersons.Add(person);

    public void RemoveSelectedPerson(Person person) => selectedPersons.Remove(person);
}
