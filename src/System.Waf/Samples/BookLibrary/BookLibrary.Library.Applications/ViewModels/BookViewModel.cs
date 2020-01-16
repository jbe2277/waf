using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels
{
    [Export]
    public class BookViewModel : ViewModel<IBookView>
    {
        private bool isValid = true;
        private Book? book;
        
        [ImportingConstructor]
        public BookViewModel(IBookView view) : base(view)
        {
        }

        public bool IsEnabled => Book != null;

        public bool IsValid
        {
            get => isValid;
            set => SetProperty(ref isValid, value);
        }

        public Book? Book
        {
            get => book;
            set
            {
                if (SetProperty(ref book, value))
                {
                    RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public ICommand? LendToCommand { get; set; }
    }
}
