using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels;

public class PersonListViewModel(IPersonListView view) : ViewModel<IPersonListView>(view)
{
    private readonly ObservableList<Person> selectedPersons = [];

    public IReadOnlyList<Person> SelectedPersons => selectedPersons;

    public bool IsValid { get; set => SetProperty(ref field, value); } = true;

    public IReadOnlyList<Person>? Persons { get; set; }

    public Person? SelectedPerson { get; set => SetProperty(ref field, value); }

    public ICommand? AddNewCommand { get; set; }

    public ICommand? RemoveCommand { get; set; }

    public ICommand? CreateNewEmailCommand { get; set; }

    public string FilterText { get; set => SetProperty(ref field, value); } = "";

    public Func<IEnumerable<Person>, IOrderedEnumerable<Person>>? Sort { get; set => SetProperty(ref field, value); }

    public void Focus() => ViewCore.FocusFirstCell();

    public bool Filter(Person person)
    {
        return string.IsNullOrEmpty(FilterText)
            || (!string.IsNullOrEmpty(person.Firstname) && person.Firstname.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
            || (!string.IsNullOrEmpty(person.Lastname) && person.Lastname.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase));
    }

    public void AddSelectedPerson(Person person) => selectedPersons.Add(person);

    public void RemoveSelectedPerson(Person person) => selectedPersons.Remove(person);
}
