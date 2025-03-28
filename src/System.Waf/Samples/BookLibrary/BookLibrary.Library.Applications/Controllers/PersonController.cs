using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.Controllers;

/// <summary>Responsible for the person management and the master / detail views.</summary>
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

    public PersonController(IMessageService messageService, IShellService shellService, IEntityService entityService, IEmailService emailService,
        PersonListViewModel personListViewModel, PersonViewModel personViewModel)
    {
        this.messageService = messageService;
        this.shellService = shellService;
        this.entityService = entityService;
        this.emailService = emailService;
        this.personListViewModel = personListViewModel;
        this.personViewModel = personViewModel;
        addNewCommand = new(AddNewPerson, CanAddPerson);
        removeCommand = new(RemovePerson, CanRemovePerson);
        createNewEmailCommand = new(CreateNewEmail!);
    }

    internal ObservableListViewCore<Person>? PersonsView { get; private set; }

    public void Initialize()
    {
        personViewModel.CreateNewEmailCommand = createNewEmailCommand;
        personViewModel.PropertyChanged += PersonViewModelPropertyChanged;
        
        PersonsView = new(entityService.Persons, null, personListViewModel.Filter, null);
        personListViewModel.Persons = PersonsView;
        personListViewModel.AddNewCommand = addNewCommand;
        personListViewModel.RemoveCommand = removeCommand;
        personListViewModel.CreateNewEmailCommand = createNewEmailCommand;
        personListViewModel.PropertyChanged += PersonListViewModelPropertyChanged;
        
        shellService.PersonListView = personListViewModel.View;
        shellService.PersonView = personViewModel.View;
    }

    public async void Run()
    {
        try
        {
            await entityService.LoadPersons();
        }
        catch (Exception ex)
        {
            Log.Default.Error(ex, "LoadPersons");
            messageService.ShowError(shellService.ShellView, Resources.LoadErrorPersons);
        }
        personListViewModel.SelectedPerson = personListViewModel.Persons?.FirstOrDefault();
    }

    private bool CanAddPerson() => personListViewModel.IsValid && personViewModel.IsValid;

    private void AddNewPerson()
    {
        var person = new Person();
        person.Validate();
        entityService.Persons.Add(person);
        personListViewModel.SelectedPerson = person;
        personListViewModel.Focus();
    }

    private bool CanRemovePerson() => personListViewModel.SelectedPerson != null;

    private void RemovePerson()
    {
        var personsToExclude = personListViewModel.SelectedPersons.Except(new[] { personListViewModel.SelectedPerson });
        var nextPerson = personListViewModel.Persons!.Except(personsToExclude).GetNextElementOrDefault(personListViewModel.SelectedPerson);
        foreach (var x in personListViewModel.SelectedPersons.ToArray()) entityService.Persons.Remove(x);
        personListViewModel.SelectedPerson = nextPerson ?? personListViewModel.Persons!.LastOrDefault();
        personListViewModel.Focus();
    }

    private void CreateNewEmail(object recipient)
    {
        var person = (Person)recipient;
        if (string.IsNullOrEmpty(person.Email) || person.GetErrors(nameof(person.Email)).Any())
        {
            messageService.ShowError(shellService.ShellView, Resources.CorrectEmailAddress);
            return;
        }
        emailService.CreateNewEmail(person.Email!);
    }

    private void UpdateCommands() => DelegateCommand.RaiseCanExecuteChanged(addNewCommand, removeCommand);

    private void PersonListViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PersonListViewModel.SelectedPerson))
        {
            personViewModel.Person = personListViewModel.SelectedPerson;
            UpdateCommands();
        }
        else if (e.PropertyName == nameof(PersonListViewModel.IsValid))
        {
            UpdateCommands();
        }
        else if (e.PropertyName == nameof(PersonListViewModel.FilterText))
        {
            PersonsView!.Update();
        }
        else if (e.PropertyName == nameof(PersonListViewModel.Sort))
        {
            PersonsView!.Sort = personListViewModel.Sort;
        }
    }

    private void PersonViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PersonViewModel.IsValid)) UpdateCommands();
    }
}
