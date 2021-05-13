using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Test.BookLibrary.Library.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : TestClassBase
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            Get<EntityService>().BookLibraryContext = Get<MockDBContextService>().GetBookLibraryContext(out _);
        }

        [TestMethod]
        public void ModuleControllerLifecycleTest()
        {
            var presentationService = Get<MockPresentationService>();
            var entityController = Get<MockEntityController>();
            var moduleController = new ModuleController(Get<IMessageService>(), presentationService, entityController, Get<BookController>(),
                Get<PersonController>(), Get<ShellService>(), GetLazy<ShellViewModel>());
            Assert.IsTrue(presentationService.InitializeCulturesCalled);

            // Initialize
            Assert.IsFalse(entityController.InitializeCalled);
            moduleController.Initialize();
            Assert.IsTrue(entityController.InitializeCalled);

            // Run
            var shellView = Get<MockShellView>();
            Assert.IsFalse(shellView.IsVisible);
            moduleController.Run();
            Assert.IsTrue(shellView.IsVisible);

            // Exit the ShellView
            var shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView)!;
            shellViewModel.ExitCommand!.Execute(null);
            Assert.IsFalse(shellView.IsVisible);

            // Shutdown
            Assert.IsFalse(entityController.ShutdownCalled);
            moduleController.Shutdown();
            Assert.IsTrue(entityController.ShutdownCalled);
        }

        [TestMethod]
        public void ModuleControllerHasChangesTest()
        {
            var messageService = Get<MockMessageService>();
            var entityController = Get<MockEntityController>();
            var moduleController = new ModuleController(messageService, Get<IPresentationService>(), entityController, Get<BookController>(),
                Get<PersonController>(), Get<ShellService>(), GetLazy<ShellViewModel>());
            moduleController.Initialize();
            moduleController.Run();
            var shellView = Get<MockShellView>();
            var shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView)!;

            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            // When the question box asks us to save the changes we say "Yes" => true.
            messageService.ShowQuestionStub = (_, message) =>
            {
                Assert.AreEqual(Resources.SaveChangesQuestion, message);
                return true;
            };
            // Then we simulate that the EntityController wasn't able to save the changes.
            entityController.SaveResult = false;
            shellViewModel.ExitCommand!.Execute(null);
            // The Save method must be called. Because the save operation failed the expect the ShellView to be
            // still visible.
            Assert.IsTrue(entityController.SaveCalled);
            Assert.IsTrue(shellView.IsVisible);

            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            entityController.SaveCalled = false;
            // When the question box asks us to save the changes we say "Cancel" => null.
            messageService.ShowQuestionStub = (_, message) => null;
            // This time the Save method must not be called. Because we have chosen "Cancel" the ShellView must still
            // be visible.
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(entityController.SaveCalled);
            Assert.IsTrue(shellView.IsVisible);

            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            entityController.SaveCalled = false;
            // When the question box asks us to save the changes we say "No" => false.
            messageService.ShowQuestionStub = (_, message) => false;
            // This time the Save method must not be called. Because we have chosen "No" the ShellView must still
            // be closed.
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(entityController.SaveCalled);
            Assert.IsFalse(shellView.IsVisible);
        }

        [TestMethod]
        public void ModuleControllerIsInvalidTest()
        {
            var messageService = Get<MockMessageService>();
            var entityController = Get<MockEntityController>();
            var moduleController = new ModuleController(messageService, Get<IPresentationService>(), entityController, Get<BookController>(),
                Get<PersonController>(), Get<ShellService>(), GetLazy<ShellViewModel>());
            moduleController.Initialize();
            moduleController.Run();
            var shellView = Get<MockShellView>();
            var shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView)!;

            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            // Simulate UI errors.
            entityController.CanSaveResult = false;
            // When the question box asks us to loose our changes we say "No" => false.
            messageService.ShowYesNoQuestionStub = (_, message) =>
            {
                Assert.AreEqual(Resources.LoseChangesQuestion, message);
                return false;
            };
            shellViewModel.ExitCommand!.Execute(null);
            // We expect the ShellView to stay open.
            Assert.IsTrue(shellView.IsVisible);

            // Exit the application again but this time we agree to loose our changes.
            messageService.ShowYesNoQuestionStub = (_, message) => true;
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);
        }
    }
}
