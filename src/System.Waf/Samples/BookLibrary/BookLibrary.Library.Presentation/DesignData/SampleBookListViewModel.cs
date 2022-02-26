using System.Waf.Applications;
using Waf.BookLibrary.Library.Applications.DataModels;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.DesignData;

public class SampleBookListViewModel : BookListViewModel
{
    public SampleBookListViewModel() : base(new MockBookListView())
    {
        FilterText = "An example search text";
        var command = new DelegateCommand(() => { });
        Books = new List<BookDataModel>
        {
            new(new Book
            {
                Title = "Serenity, Vol 1: Those Left Behind",
                Author = "Joss Whedon, Brett Matthews, Will Conrad",
                PublishDate = new DateTime(2006, 8, 2)
            }, command),
            new(new Book
            {
                Title = "Star Wars - Heir to the Empire",
                Author = "Timothy Zahn",
                PublishDate = new DateTime(1992, 1, 5),
                LendTo = new Person
                {
                    Firstname = "Harry",
                    Lastname = "Potter"
                }
            }, command),
            new(new Book
            {
                Title = "The Lord of the Rings - The Fellowship of the Ring",
                Author = "J.R.R. Tolkien",
                PublishDate = new DateTime(1986, 12, 8)
            }, command),
        };
    }


    private class MockBookListView : IBookListView
    {
        public object? DataContext { get; set; }

        public void FocusFirstCell() { }
    }
}
