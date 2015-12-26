using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class LendToViewModel : ViewModel<ILendToView>
    {
        private readonly DelegateCommand okCommand;
        private Book book;
        private IReadOnlyList<Person> persons;
        private bool isWasReturned;
        private bool isLendTo;
        private Person selectedPerson;
        private bool dialogResult;

        
        [ImportingConstructor]
        public LendToViewModel(ILendToView view) : base(view)
        {
            this.okCommand = new DelegateCommand(OkHandler);
        }


        public static string Title { get { return ApplicationInfo.ProductName; } }

        public ICommand OkCommand { get { return okCommand; } }

        public Book Book
        {
            get { return book; }
            set 
            {
                if (SetProperty(ref book, value))
                {
                    if (Book.LendTo == null) { IsLendTo = true; }
                    else { IsWasReturned = true; }
                }
            }
        }

        public IReadOnlyList<Person> Persons
        {
            get { return persons; }
            set 
            {
                if (SetProperty(ref persons, value))
                {
                    SelectedPerson = persons.FirstOrDefault();
                }
            }
        }

        public bool IsWasReturned
        {
            get { return isWasReturned; }
            set
            {
                if (SetProperty(ref isWasReturned, value))
                {
                    IsLendTo = !value;
                }
            }
        }
        
        public bool IsLendTo
        {
            get { return isLendTo; }
            set
            {
                if (SetProperty(ref isLendTo, value))
                {
                    IsWasReturned = !value;
                }
            }
        }
        
        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set { SetProperty(ref selectedPerson, value); }
        }


        public bool ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
            return dialogResult;
        }

        private void OkHandler() 
        {
            dialogResult = true;
            if (IsWasReturned) { SelectedPerson = null; }
            ViewCore.Close();
        }
    }
}
