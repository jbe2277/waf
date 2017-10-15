using System;
using System.Collections.Generic;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.DesignData
{
    public class SampleLendToViewModel : LendToViewModel
    {
        public SampleLendToViewModel() : base(new MockLendToView())
        {
            Persons = new List<Person>
            {
                new Person
                {
                    Firstname = "Harry",
                    Lastname = "Potter"
                },
                new Person
                {
                    Firstname = "Ron",
                    Lastname = "Weasley"
                }
            };
            Book = new Book
            {
                Title = "Serenity, Vol 1: Those Left Behind",
                Author = "Joss Whedon, Brett Matthews, Will Conrad",
                Publisher = "Dark Horse",
                PublishDate = new DateTime(2006, 8, 2),
                Isbn = "1593074492",
                Language = Language.English,
                Pages = 104,
                LendTo = Persons[0]
            };
            IsLendTo = true;
        }


        private class MockLendToView : ILendToView
        {
            public object DataContext { get; set; }

            public void Close() { }

            public void ShowDialog(object owner) { }
        }
    }
}
