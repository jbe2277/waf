using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels
{
    [Export]
    public class PersonListViewModel : ViewModel<IPersonListView>
    {
        private readonly ObservableCollection<Person> selectedPersons;
        private bool isValid = true;
        private IReadOnlyList<Person> persons;
        private Person selectedPerson;
        private ICommand addNewCommand;
        private ICommand removeCommand;
        private ICommand createNewEmailCommand;
        private string filterText = "";
        

        [ImportingConstructor]
        public PersonListViewModel(IPersonListView view) : base(view)
        {
            this.selectedPersons = new ObservableCollection<Person>();
        }


        public IReadOnlyList<Person> SelectedPersons { get { return selectedPersons; } }

        public IEnumerable<Person> PersonCollectionView { get; set; }
        
        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }
        
        public IReadOnlyList<Person> Persons
        {
            get { return persons; }
            set { SetProperty(ref persons, value); }
        }

        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set { SetProperty(ref selectedPerson, value); }
        }

        public ICommand AddNewCommand
        {
            get { return addNewCommand; }
            set { SetProperty(ref addNewCommand, value); }
        }

        public ICommand RemoveCommand
        {
            get { return removeCommand; }
            set { SetProperty(ref removeCommand, value); }
        }

        public ICommand CreateNewEmailCommand
        {
            get { return createNewEmailCommand; }
            set { SetProperty(ref createNewEmailCommand, value); }
        }

        public string FilterText
        {
            get { return filterText; }
            set { SetProperty(ref filterText, value); }
        }


        public void Focus()
        {
            ViewCore.FocusFirstCell();
        }

        public bool Filter(Person person)
        {
            if (string.IsNullOrEmpty(filterText)) { return true; }
            
            return (string.IsNullOrEmpty(person.Firstname) || person.Firstname.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                || (string.IsNullOrEmpty(person.Lastname) || person.Lastname.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        public void AddSelectedPerson(Person person)
        {
            selectedPersons.Add(person);
        }

        public void RemoveSelectedPerson(Person person)
        {
            selectedPersons.Remove(person);
        }
    }
}
