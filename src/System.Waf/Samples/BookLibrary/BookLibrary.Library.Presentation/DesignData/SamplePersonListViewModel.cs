using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.DesignData
{
    public class SamplePersonListViewModel : PersonListViewModel
    {
        public SamplePersonListViewModel() : base(new MockPersonListView())
        {
            FilterText = "An example search text";
            Persons = new List<Person>
            {
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
            };
        }


        private class MockPersonListView : IPersonListView
        {
            public object? DataContext { get; set; }

            public void FocusFirstCell() { }
        }
    }
}
