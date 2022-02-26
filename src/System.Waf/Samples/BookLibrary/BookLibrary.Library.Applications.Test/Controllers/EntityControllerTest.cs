using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.Controllers
{
    [TestClass]
    public class EntityControllerTest : TestClassBase
    {
        [TestMethod]
        public void ValidateBeforeSave()
        {
            var controller = Get<EntityController>();
            controller.Initialize();
            Assert.IsFalse(controller.HasChanges());
            var entityService = Get<EntityService>();
            entityService.Persons.Add(new Person() { Firstname = "Harry", Lastname = "Potter" });
            entityService.Persons[^1].Validate();
            Assert.IsTrue(controller.HasChanges());

            Assert.IsTrue(controller.CanSave());
            controller.Save();
            Assert.IsFalse(controller.HasChanges());

            var messageService = Get<MockMessageService>();
            Assert.AreEqual(MessageType.None, messageService.MessageType);

            entityService.Persons.Add(new Person());
            entityService.Persons[^1].Validate();
            Assert.IsTrue(controller.HasChanges());
            var shellViewModel = Get<ShellViewModel>();
            shellViewModel.SaveCommand!.Execute(null);

            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsTrue(controller.HasChanges());
            controller.Shutdown();
        }

        [TestMethod]
        public void DisableSaveWhenShellIsInvalid()
        {
            var controller = Get<EntityController>();
            controller.Initialize();
            var shellViewModel = Get<ShellViewModel>();
            Assert.IsTrue(shellViewModel.SaveCommand!.CanExecute(null));
            AssertHelper.CanExecuteChangedEvent(shellViewModel.SaveCommand, () => shellViewModel.IsValid = false);
            Assert.IsFalse(shellViewModel.SaveCommand.CanExecute(null));
            controller.Shutdown();
        }

        [TestMethod]
        public void EntityToStringTest()
        {
            var entity = new Entity() { ToStringValue = "Test1" };
            Assert.AreEqual("Test1", EntityController.EntityToString(entity));

            var entity2 = new FormattableEntity() { ToStringValue = "Test2" };
            Assert.AreEqual("Test2", EntityController.EntityToString(entity2));
        }


        private class Entity
        {
            public string? ToStringValue;

            public override string? ToString() => ToStringValue;
        }

        private class FormattableEntity : IFormattable
        {
            public string ToStringValue = "";

            public string ToString(string? format, IFormatProvider? formatProvider) => ToStringValue;
        }
    }
}
