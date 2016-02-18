using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using System.Windows.Input;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Controllers
{
    [TestClass]
    public class PersonControllerTest : TestClassBase
    {
        [TestMethod]
        public void SelectionTest()
        {
            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(new Person() { Firstname = "Harry"});
            entityService.Persons.Add(new Person() { Firstname = "Ron" });
            
            PersonController personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            // Check that Initialize shows the PersonListView and PersonView
            ShellService shellService = Container.GetExportedValue<ShellService>();
            Assert.IsInstanceOfType(shellService.PersonListView, typeof(IPersonListView));
            Assert.IsInstanceOfType(shellService.PersonView, typeof(IPersonView));

            // Check that the first Person is selected
            IPersonListView personListView = Container.GetExportedValue<IPersonListView>();
            PersonListViewModel personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView);
            Assert.AreEqual(entityService.Persons.First(), personListViewModel.SelectedPerson);
            
            // Change the selection
            PersonViewModel personViewModel = Container.GetExportedValue<PersonViewModel>();
            personListViewModel.SelectedPerson = entityService.Persons.Last();
            Assert.AreEqual(entityService.Persons.Last(), personViewModel.Person);
        }

        [TestMethod]
        public void AddAndRemoveTest()
        {
            Person harry = new Person() { Firstname = "Harry" };
            Person ron = new Person() { Firstname = "Ron" };
            
            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);

            PersonController personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            MockPersonListView personListView = Container.GetExportedValue<MockPersonListView>();
            PersonListViewModel personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView);
            personListViewModel.PersonCollectionView = personListViewModel.Persons;
            MockPersonView personView = Container.GetExportedValue<MockPersonView>();
            PersonViewModel personViewModel = ViewHelper.GetViewModel<PersonViewModel>(personView);

            // Add a new Person
            Assert.AreEqual(2, entityService.Persons.Count);
            Assert.IsTrue(personListViewModel.AddNewCommand.CanExecute(null));
            personListViewModel.AddNewCommand.Execute(null);
            Assert.AreEqual(3, entityService.Persons.Count);

            // Check that the new Person is selected and the first control gets the focus
            Assert.AreEqual(entityService.Persons.Last(), personViewModel.Person);
            Assert.IsTrue(personListView.FirstCellHasFocus);

            // Simulate an invalid UI input state => the user can't add more persons
            AssertHelper.CanExecuteChangedEvent(personListViewModel.AddNewCommand, () => 
                personViewModel.IsValid = false);
            Assert.IsFalse(personListViewModel.AddNewCommand.CanExecute(null));
            Assert.IsFalse(personListViewModel.RemoveCommand.CanExecute(null));

            // Remove the last two Persons at once
            personViewModel.IsValid = true;
            personListView.FirstCellHasFocus = false;
            personListViewModel.AddSelectedPerson(ron);
            personListViewModel.AddSelectedPerson(entityService.Persons.Last());
            Assert.IsTrue(personListViewModel.RemoveCommand.CanExecute(null));
            personListViewModel.RemoveCommand.Execute(null);
            Assert.IsTrue(entityService.Persons.SequenceEqual(new Person[] { harry }));
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
            Person harry = new Person() { Firstname = "Harry", Email = "harry.potter@hogwarts.edu" };
            Person ron = new Person() { Firstname = "Ron", Email = "Wrong Address" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);

            PersonController personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            MockPersonListView personListView = Container.GetExportedValue<MockPersonListView>();
            PersonListViewModel personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView);
            MockPersonView personView = Container.GetExportedValue<MockPersonView>();
            PersonViewModel personViewModel = ViewHelper.GetViewModel<PersonViewModel>(personView);

            ICommand command = personListViewModel.CreateNewEmailCommand;
            Assert.AreEqual(command, personViewModel.CreateNewEmailCommand);

            MockEmailService emailService = Container.GetExportedValue<MockEmailService>();
            command.Execute(harry);
            Assert.AreEqual(harry.Email, emailService.ToEmailAddress);

            // An error message should occur when the email address is invalid.

            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
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

            var entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(harry);

            var personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            var personListViewModel = Container.GetExportedValue<PersonListViewModel>();
            personListViewModel.AddSelectedPerson(personListViewModel.Persons.Single());
            var personViewModel = Container.GetExportedValue<PersonViewModel>();

            CheckCommand(personListViewModel.AddNewCommand, personListViewModel, personViewModel);
            CheckCommand(personListViewModel.RemoveCommand, personListViewModel, personViewModel);
        }

        private void CheckCommand(ICommand command, PersonListViewModel personListViewModel, PersonViewModel personViewModel)
        {
            Assert.IsTrue(command.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(command, () => personListViewModel.IsValid = false);
            Assert.IsFalse(command.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(command, () => personListViewModel.IsValid = true);
            Assert.IsTrue(command.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(command, () => personViewModel.IsValid = false);
            Assert.IsFalse(command.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(command, () => personViewModel.IsValid = true);
            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        public void RemoveAndSelection1Test()
        {
            Person harry = new Person() { Firstname = "Harry" };
            Person ron = new Person() { Firstname = "Ron" };
            Person ginny = new Person() { Firstname = "Ginny" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);
            entityService.Persons.Add(ginny);

            PersonController personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            MockPersonListView personListView = Container.GetExportedValue<MockPersonListView>();
            PersonListViewModel personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView);
            // Set the sorting to: "Ginny", "Harry", "Ron"
            personListViewModel.PersonCollectionView = personListViewModel.Persons.OrderBy(p => p.Firstname);

            // Remove the first person and check that the second one is selected.
            personListViewModel.SelectedPerson = ginny;
            personListViewModel.AddSelectedPerson(personListViewModel.SelectedPerson);
            personListViewModel.RemoveCommand.Execute(null);
            Assert.IsTrue(entityService.Persons.SequenceEqual(new[] { harry, ron }));
            Assert.AreEqual(harry, personListViewModel.SelectedPerson);
        }

        [TestMethod]
        public void RemoveAndSelection2Test()
        {
            Person harry = new Person() { Firstname = "Harry" };
            Person ron = new Person() { Firstname = "Ron" };
            Person ginny = new Person() { Firstname = "Ginny" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);
            entityService.Persons.Add(ginny);

            PersonController personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            MockPersonListView personListView = Container.GetExportedValue<MockPersonListView>();
            PersonListViewModel personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView);
            // Set the sorting to: "Ginny", "Harry", "Ron"
            personListViewModel.PersonCollectionView = personListViewModel.Persons.OrderBy(p => p.Firstname);

            // Remove the last person and check that the last one is selected again.
            personListViewModel.SelectedPerson = ron;
            personListViewModel.AddSelectedPerson(personListViewModel.SelectedPerson);
            personListViewModel.RemoveCommand.Execute(null);
            Assert.IsTrue(entityService.Persons.SequenceEqual(new[] { harry, ginny }));
            Assert.AreEqual(harry, personListViewModel.SelectedPerson);
        }

        [TestMethod]
        public void RemoveAndSelection3Test()
        {
            Person harry = new Person() { Firstname = "Harry" };
            Person ron = new Person() { Firstname = "Ron" };
            Person ginny = new Person() { Firstname = "Ginny" };

            IEntityService entityService = Container.GetExportedValue<IEntityService>();
            entityService.Persons.Add(harry);
            entityService.Persons.Add(ron);
            entityService.Persons.Add(ginny);

            PersonController personController = Container.GetExportedValue<PersonController>();
            personController.Initialize();

            MockPersonListView personListView = Container.GetExportedValue<MockPersonListView>();
            PersonListViewModel personListViewModel = ViewHelper.GetViewModel<PersonListViewModel>(personListView);
            personListViewModel.PersonCollectionView = personListViewModel.Persons;

            // Remove all persons and check that nothing is selected anymore
            personListViewModel.SelectedPerson = harry;
            personListViewModel.AddSelectedPerson(harry);
            personListViewModel.AddSelectedPerson(ron);
            personListViewModel.AddSelectedPerson(ginny);
            personListViewModel.RemoveCommand.Execute(null);
            Assert.IsFalse(entityService.Persons.Any());
            Assert.IsNull(personListViewModel.SelectedPerson);
        }
    }
}
