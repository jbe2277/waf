using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Controllers
{
    /// <summary>
    /// Responsible for the person management and the master / detail views.
    /// </summary>
    [Export]
    internal class PersonController
    {
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly IEntityService entityService;
        private readonly IEmailService emailService;
        private readonly PersonListViewModel personListViewModel;
        private readonly PersonViewModel personViewModel;
        private readonly DelegateCommand addNewCommand;
        private readonly DelegateCommand removeCommand;
        private readonly DelegateCommand createNewEmailCommand;
        

        [ImportingConstructor]
        public PersonController(IMessageService messageService, IShellService shellService,
            IEntityService entityService, IEmailService emailService, 
            PersonListViewModel personListViewModel, PersonViewModel personViewModel)
        {
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;
            this.emailService = emailService;
            this.personListViewModel = personListViewModel;
            this.personViewModel = personViewModel;
            this.addNewCommand = new DelegateCommand(AddNewPerson, CanAddPerson);
            this.removeCommand = new DelegateCommand(RemovePerson, CanRemovePerson);
            this.createNewEmailCommand = new DelegateCommand(CreateNewEmail);
        }


        public void Initialize()
        {
            personViewModel.CreateNewEmailCommand = createNewEmailCommand;
            PropertyChangedEventManager.AddHandler(personViewModel, PersonViewModelPropertyChanged, "");
            
            personListViewModel.Persons = entityService.Persons;
            personListViewModel.AddNewCommand = addNewCommand;
            personListViewModel.RemoveCommand = removeCommand;
            personListViewModel.CreateNewEmailCommand = createNewEmailCommand;
            PropertyChangedEventManager.AddHandler(personListViewModel, PersonListViewModelPropertyChanged, "");

            shellService.PersonListView = personListViewModel.View;
            shellService.PersonView = personViewModel.View;

            personListViewModel.SelectedPerson = personListViewModel.Persons.FirstOrDefault();
        }

        private bool CanAddPerson() { return personListViewModel.IsValid && personViewModel.IsValid; }

        private void AddNewPerson()
        {
            Person person = new Person();
            person.Validate();
            entityService.Persons.Add(person);
            
            personListViewModel.SelectedPerson = person;
            personListViewModel.Focus();
        }

        private bool CanRemovePerson() 
        {
            // Unfortunately, it is necessary to deactivate the Remove command when a cell is invalid in the DataGrid. Otherwise, it might freeze.
            // See: https://connect.microsoft.com/VisualStudio/feedback/details/777761/wpf-datagrid-becomes-readonly-after-deleting-an-invalid-row
            return personListViewModel.SelectedPerson != null && personListViewModel.IsValid && personViewModel.IsValid; 
        }

        private void RemovePerson()
        {
            // Use the PersonCollectionView, which represents the sorted/filtered state of the persons, to determine the next person to select.
            var personsToExclude = personListViewModel.SelectedPersons.Except(new[] { personListViewModel.SelectedPerson });
            Person nextPerson = CollectionHelper.GetNextElementOrDefault(personListViewModel.PersonCollectionView.Except(personsToExclude),
                personListViewModel.SelectedPerson);

            foreach (Person person in personListViewModel.SelectedPersons.ToArray())
            {
                entityService.Persons.Remove(person);
            }

            personListViewModel.SelectedPerson = nextPerson ?? personListViewModel.PersonCollectionView.LastOrDefault();
            personListViewModel.Focus();
        }

        private void CreateNewEmail(object recipient)
        {
            Person person = (Person)recipient;
            
            if (string.IsNullOrEmpty(person.Email) || person.GetErrors("Email").Any())
            {
                messageService.ShowError(shellService.ShellView, Resources.CorrectEmailAddress);
                return;
            }

            emailService.CreateNewEmail(person.Email);
        }

        private void UpdateCommands()
        {
            addNewCommand.RaiseCanExecuteChanged();
            removeCommand.RaiseCanExecuteChanged();
        }

        private void PersonListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPerson")
            {
                personViewModel.Person = personListViewModel.SelectedPerson;
                UpdateCommands();
            }
            else if (e.PropertyName == "IsValid")
            {
                UpdateCommands();
            }
        }

        private void PersonViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsValid")
            {
                UpdateCommands();
            }
        }
    }
}
