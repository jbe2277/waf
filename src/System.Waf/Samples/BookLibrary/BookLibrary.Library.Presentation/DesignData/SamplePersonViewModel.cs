using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.DesignData
{
    public class SamplePersonViewModel : PersonViewModel
    {
        public SamplePersonViewModel() : base(new MockPersonView())
        {
            Person = new Person
            {
                Firstname = "Harry",
                Lastname = "Potter",
                Email = "harry.potter@hogwarts.edu"
            };
        }


        private class MockPersonView : IPersonView
        {
            public object? DataContext { get; set; }
        }
    }
}
