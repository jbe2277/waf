using System;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.DataModels
{
    public class BookDataModel : Model
    {
        private readonly Book book;
        private readonly ICommand lendToCommand;


        public BookDataModel(Book book, ICommand lendToCommand)
        {
            if (book == null) { throw new ArgumentNullException("book"); }
            if (lendToCommand == null) { throw new ArgumentNullException("lendToCommand"); }

            this.book = book;
            this.lendToCommand = lendToCommand;
        }


        public Book Book { get { return book; } }

        public ICommand LendToCommand { get { return lendToCommand; } }
    }
}
