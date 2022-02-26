using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.DesignData;

public class SampleBookViewModel : BookViewModel
{
    public SampleBookViewModel() : base(new MockBookView())
    {
        Book = new Book
        {
            Title = "Serenity, Vol 1: Those Left Behind",
            Author = "Joss Whedon, Brett Matthews, Will Conrad",
            Publisher = "Dark Horse",
            PublishDate = new DateTime(2006, 8, 2),
            Isbn = "1593074492",
            Language = Language.English,
            Pages = 104,
            LendTo = new Person
            {
                Firstname = "Harry",
                Lastname = "Potter"
            }
        };
    }


    private class MockBookView : IBookView
    {
        public object? DataContext { get; set; }
    }
}
