using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Views;

public class MockPersonListView : MockView<PersonListViewModel>, IPersonListView
{
    public bool FirstCellHasFocus { get; set; }

    public void FocusFirstCell() => FirstCellHasFocus = true;

    public void SingleSelect(Person? person)
    {
        ViewModel.SelectedPerson = person;
        foreach (var x in ViewModel.SelectedPersons.ToArray()) ViewModel.RemoveSelectedPerson(x);
        if (person is not null) ViewModel.AddSelectedPerson(person);
    }
}
