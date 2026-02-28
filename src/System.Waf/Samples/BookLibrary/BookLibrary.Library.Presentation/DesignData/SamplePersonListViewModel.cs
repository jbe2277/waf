using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.DesignData;

public class SamplePersonListViewModel : PersonListViewModel
{
    public SamplePersonListViewModel() : base(new MockPersonListView())
    {
        FilterText = "An example search text";
        Persons =
        [
            new()
            {
                Firstname = "Harry",
                Lastname = "Potter",
                Email = "harry.potter@hogwarts.edu"
            },
            new()
            {
                Firstname = "Ron",
                Lastname = "Weasley",
                Email = "hermione.granger@howarts.edu"
            }
        ];
    }


    private sealed class MockPersonListView : IPersonListView
    {
        public object? DataContext { get; set; }

        public void FocusFirstCell() { }
    }
}
