using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Controllers;

[TestClass]
public class PersonControllerTest : TestClassBase
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        Get<EntityService>().BookLibraryContext = Get<MockDBContextService>().GetBookLibraryContext(out _);
    }

    [TestMethod]
    public void SelectionTest()
    {
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(new Person() { Firstname = "Harry" });
        entityService.Persons.Add(new Person() { Firstname = "Ron" });
        var personController = Get<PersonController>();
        personController.Initialize();

        // Check that Initialize shows the PersonListView and PersonView
        var shellService = Get<ShellService>();
        Assert.IsInstanceOfType(shellService.PersonListView, typeof(IPersonListView));
        Assert.IsInstanceOfType(shellService.PersonView, typeof(IPersonView));

        // Check that the first Person is selected
        var personListView = Get<IPersonListView>();
        var personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView)!;
        Assert.AreEqual(entityService.Persons[0], personListViewModel.SelectedPerson);

        // Change the selection
        var personViewModel = Get<PersonViewModel>();
        personListViewModel.SelectedPerson = entityService.Persons[^1];
        Assert.AreEqual(entityService.Persons[^1], personViewModel.Person);
    }

    [TestMethod]
    public void AddAndRemoveTest()
    {
        var harry = new Person() { Firstname = "Harry" };
        var ron = new Person() { Firstname = "Ron" };
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(harry);
        entityService.Persons.Add(ron);
        var personController = Get<PersonController>();
        personController.Initialize();
        var personListView = Get<MockPersonListView>();
        var personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView)!;
        var personView = Get<MockPersonView>();
        var personViewModel = ViewHelper.GetViewModel<PersonViewModel>(personView)!;

        // Add a new Person
        Assert.AreEqual(2, entityService.Persons.Count);
        Assert.IsTrue(personListViewModel.AddNewCommand!.CanExecute(null));
        personListViewModel.AddNewCommand.Execute(null);
        Assert.AreEqual(3, entityService.Persons.Count);

        // Check that the new Person is selected and the first control gets the focus
        Assert.AreEqual(entityService.Persons[^1], personViewModel.Person);
        Assert.IsTrue(personListView.FirstCellHasFocus);

        // Simulate an invalid UI input state => the user can't add more persons
        AssertHelper.CanExecuteChangedEvent(personListViewModel.AddNewCommand, () => personViewModel.IsValid = false);
        Assert.IsFalse(personListViewModel.AddNewCommand.CanExecute(null));

        // Remove the last two Persons at once
        personViewModel.IsValid = true;
        personListView.FirstCellHasFocus = false;
        personListViewModel.AddSelectedPerson(ron);
        personListViewModel.AddSelectedPerson(entityService.Persons[^1]);
        Assert.IsTrue(personListViewModel.RemoveCommand!.CanExecute(null));
        personListViewModel.RemoveCommand.Execute(null);
        AssertHelper.SequenceEqual(new[] { harry }, entityService.Persons);
        Assert.AreEqual(harry, personViewModel.Person);
        Assert.IsTrue(personListView.FirstCellHasFocus);

        // Deselect all Persons => the Remove command must be deactivated
        AssertHelper.CanExecuteChangedEvent(personListViewModel.RemoveCommand, () =>
        {
            personListViewModel.SelectedPersons.ToList().ForEach(x => personListViewModel.RemoveSelectedPerson(x));
            personListViewModel.SelectedPerson = null;
        });
        Assert.IsFalse(personListViewModel.RemoveCommand.CanExecute(null));
    }

    [TestMethod]
    public void CreateNewEmailTest()
    {
        var harry = new Person() { Firstname = "Harry", Email = "harry.potter@hogwarts.edu" };
        var ron = new Person() { Firstname = "Ron", Email = "Wrong Address" };
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(harry);
        entityService.Persons.Add(ron);
        var personController = Get<PersonController>();
        personController.Initialize();
        var personListView = Get<MockPersonListView>();
        var personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView)!;
        var personView = Get<MockPersonView>();
        var personViewModel = ViewHelper.GetViewModel<PersonViewModel>(personView)!;

        var command = personListViewModel.CreateNewEmailCommand!;
        Assert.AreEqual(command, personViewModel.CreateNewEmailCommand);

        var emailService = Get<MockEmailService>();
        command.Execute(harry);
        Assert.AreEqual(harry.Email, emailService.ToEmailAddress);

        // An error message should occur when the email address is invalid.

        var messageService = Get<MockMessageService>();
        messageService.Clear();
        emailService.ToEmailAddress = null;
        command.Execute(ron);
        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.AreEqual(Resources.CorrectEmailAddress, messageService.Message);
        Assert.IsNull(emailService.ToEmailAddress);

        // An error message should occur when no email address was entered.

        messageService.Clear();
        emailService.ToEmailAddress = null;
        ron.Email = null;
        command.Execute(ron);
        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.AreEqual(Resources.CorrectEmailAddress, messageService.Message);
        Assert.IsNull(emailService.ToEmailAddress);
    }

    [TestMethod]
    public void AddAndRemoveDisableTest()
    {
        var harry = new Person() { Firstname = "Harry" };
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(harry);
        var personController = Get<PersonController>();
        personController.Initialize();
        var personListViewModel = Get<PersonListViewModel>();
        personListViewModel.AddSelectedPerson(personListViewModel.Persons!.Single());
        var personViewModel = Get<PersonViewModel>();

        var addNewCommand = personListViewModel.AddNewCommand!;
        Assert.IsTrue(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => personListViewModel.IsValid = false);
        Assert.IsFalse(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => personListViewModel.IsValid = true);
        Assert.IsTrue(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => personViewModel.IsValid = false);
        Assert.IsFalse(addNewCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(addNewCommand, () => personViewModel.IsValid = true);
        Assert.IsTrue(addNewCommand.CanExecute(null));

        var removeCommand = personListViewModel.RemoveCommand!;
        Assert.IsTrue(removeCommand.CanExecute(null));
        AssertHelper.CanExecuteChangedEvent(removeCommand, () => personListViewModel.SelectedPerson = null);
        Assert.IsFalse(removeCommand.CanExecute(null));
    }

    [TestMethod]
    public void RemoveAndSelection1Test()
    {
        var harry = new Person() { Firstname = "Harry" };
        var ron = new Person() { Firstname = "Ron" };
        var ginny = new Person() { Firstname = "Ginny" };
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(harry);
        entityService.Persons.Add(ron);
        entityService.Persons.Add(ginny);
        var personController = Get<PersonController>();
        personController.Initialize();
        var personListView = Get<MockPersonListView>();
        var personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView)!;
        // Set the sorting to: "Ginny", "Harry", "Ron"
        personController.PersonsView!.Sort = x => x.OrderBy(p => p.Firstname);

        // Remove the first person and check that the second one is selected.
        personListViewModel.SelectedPerson = ginny;
        personListViewModel.AddSelectedPerson(personListViewModel.SelectedPerson);
        personListViewModel.RemoveCommand!.Execute(null);
        AssertHelper.SequenceEqual(new[] { harry, ron }, entityService.Persons);
        Assert.AreEqual(harry, personListViewModel.SelectedPerson);
    }

    [TestMethod]
    public void RemoveAndSelection2Test()
    {
        var harry = new Person() { Firstname = "Harry" };
        var ron = new Person() { Firstname = "Ron" };
        var ginny = new Person() { Firstname = "Ginny" };
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(harry);
        entityService.Persons.Add(ron);
        entityService.Persons.Add(ginny);
        var personController = Get<PersonController>();
        personController.Initialize();
        var personListView = Get<MockPersonListView>();
        var personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView)!;
        // Set the sorting to: "Ginny", "Harry", "Ron"
        personController.PersonsView!.Sort = x => x.OrderBy(p => p.Firstname);

        // Remove the last person and check that the last one is selected again.
        personListViewModel.SelectedPerson = ron;
        personListViewModel.AddSelectedPerson(personListViewModel.SelectedPerson);
        personListViewModel.RemoveCommand!.Execute(null);
        AssertHelper.SequenceEqual(new[] { harry, ginny }, entityService.Persons);
        Assert.AreEqual(harry, personListViewModel.SelectedPerson);
    }

    [TestMethod]
    public void RemoveAndSelection3Test()
    {
        var harry = new Person() { Firstname = "Harry" };
        var ron = new Person() { Firstname = "Ron" };
        var ginny = new Person() { Firstname = "Ginny" };
        var entityService = Get<IEntityService>();
        entityService.Persons.Add(harry);
        entityService.Persons.Add(ron);
        entityService.Persons.Add(ginny);
        var personController = Get<PersonController>();
        personController.Initialize();
        var personListView = Get<MockPersonListView>();
        var personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView)!;

        // Remove all persons and check that nothing is selected anymore
        personListViewModel.SelectedPerson = harry;
        personListViewModel.AddSelectedPerson(harry);
        personListViewModel.AddSelectedPerson(ron);
        personListViewModel.AddSelectedPerson(ginny);
        personListViewModel.RemoveCommand!.Execute(null);
        Assert.IsFalse(entityService.Persons.Any());
        Assert.IsNull(personListViewModel.SelectedPerson);
    }
}
