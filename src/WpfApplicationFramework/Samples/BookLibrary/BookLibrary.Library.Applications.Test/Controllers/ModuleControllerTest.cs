using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting.Mocks;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Test.BookLibrary.Library.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : TestClassBase
    {
        [TestMethod]
        public void ModuleControllerLifecycleTest()
        {
            MockPresentationService presentationService = Container.GetExportedValue<MockPresentationService>();
            MockEntityController entityController = Container.GetExportedValue<MockEntityController>();
            ModuleController moduleController = Container.GetExportedValue<ModuleController>();
            Assert.IsTrue(presentationService.InitializeCulturesCalled);

            // Initialize
            Assert.IsFalse(entityController.InitializeCalled);
            moduleController.Initialize();
            Assert.IsTrue(entityController.InitializeCalled);

            // Run
            MockShellView shellView = Container.GetExportedValue<MockShellView>();
            Assert.IsFalse(shellView.IsVisible);
            moduleController.Run();
            Assert.IsTrue(shellView.IsVisible);

            // Exit the ShellView
            ShellViewModel shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView);
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);

            // Shutdown
            Assert.IsFalse(entityController.ShutdownCalled);
            moduleController.Shutdown();
            Assert.IsTrue(entityController.ShutdownCalled);
        }

        [TestMethod]
        public void ModuleControllerHasChangesTest()
        {
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            MockEntityController entityController = Container.GetExportedValue<MockEntityController>();
            ModuleController moduleController = Container.GetExportedValue<ModuleController>();

            moduleController.Initialize();
            moduleController.Run();

            MockShellView shellView = Container.GetExportedValue<MockShellView>();
            ShellViewModel shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView);


            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            // When the question box asks us to save the changes we say "Yes" => true.
            messageService.ShowQuestionAction = (message) =>
            {
                Assert.AreEqual(Resources.SaveChangesQuestion, message);
                return true;
            };
            // Then we simulate that the EntityController wasn't able to save the changes.
            entityController.SaveResult = false;
            shellViewModel.ExitCommand.Execute(null);
            // The Save method must be called. Because the save operation failed the expect the ShellView to be
            // still visible.
            Assert.IsTrue(entityController.SaveCalled);
            Assert.IsTrue(shellView.IsVisible);


            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            entityController.SaveCalled = false;
            // When the question box asks us to save the changes we say "Cancel" => null.
            messageService.ShowQuestionAction = (message) => null;
            // This time the Save method must not be called. Because we have chosen "Cancel" the ShellView must still
            // be visible.
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(entityController.SaveCalled);
            Assert.IsTrue(shellView.IsVisible);


            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            entityController.SaveCalled = false;
            // When the question box asks us to save the changes we say "No" => false.
            messageService.ShowQuestionAction = (message) => false;
            // This time the Save method must not be called. Because we have chosen "No" the ShellView must still
            // be closed.
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(entityController.SaveCalled);
            Assert.IsFalse(shellView.IsVisible);
        }

        [TestMethod]
        public void ModuleControllerIsInvalidTest()
        {
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            MockEntityController entityController = Container.GetExportedValue<MockEntityController>();
            ModuleController moduleController = Container.GetExportedValue<ModuleController>();

            moduleController.Initialize();
            moduleController.Run();

            MockShellView shellView = Container.GetExportedValue<MockShellView>();
            ShellViewModel shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView);


            // Exit the application although we have unsaved changes.
            entityController.HasChangesResult = true;
            // Simulate UI errors.
            entityController.CanSaveResult = false;
            // When the question box asks us to loose our changes we say "No" => false.
            messageService.ShowYesNoQuestionAction = (message) =>
            {
                Assert.AreEqual(Resources.LoseChangesQuestion, message);
                return false;
            };
            shellViewModel.ExitCommand.Execute(null);
            // We expect the ShellView to stay open.
            Assert.IsTrue(shellView.IsVisible);

            
            // Exit the application again but this time we agree to loose our changes.
            messageService.ShowYesNoQuestionAction = (message) => true;
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);
        }
    }
}
