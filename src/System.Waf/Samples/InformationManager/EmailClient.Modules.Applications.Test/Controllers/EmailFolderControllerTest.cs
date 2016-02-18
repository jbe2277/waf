using System.Linq;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Test.InformationManager.EmailClient.Modules.Applications.Controllers
{
    [TestClass]
    public class EmailFolderControllerTest : EmailClientTest
    {
        [TestMethod]
        public void DeleteEmails()
        {
            var root = new EmailClientRoot();
            root.Inbox.AddEmail(new Email()); 
            root.Inbox.AddEmail(new Email());

            // Create the controller

            var controller = Container.GetExportedValue<EmailFolderController>();
            var emailLayoutViewModel = Container.GetExportedValue<EmailLayoutViewModel>();
            var emailListViewModel = controller.EmailListViewModel;
            var emailListView = (MockEmailListView)emailListViewModel.View;
            var emailViewModel = controller.EmailViewModel;
            var emailView = (MockEmailView)emailViewModel.View;

            // Initialize the controller

            Assert.IsNull(emailLayoutViewModel.EmailListView);
            Assert.IsNull(emailLayoutViewModel.EmailView);

            controller.EmailFolder = root.Inbox;
            controller.Initialize();

            Assert.AreEqual(emailListView, emailLayoutViewModel.EmailListView);
            Assert.AreEqual(emailView, emailLayoutViewModel.EmailView);

            // Run the controller

            var shellService = Container.GetExportedValue<MockShellService>();
            Assert.IsNull(shellService.ContentView);

            controller.Run();

            Assert.AreEqual(emailLayoutViewModel.View, shellService.ContentView);

            // Delete command is disabled when no Email is selected

            Assert.IsNull(emailListViewModel.SelectedEmail);
            Assert.IsFalse(controller.DeleteEmailCommand.CanExecute(null));
            
            // Select the first email and delete it

            emailListViewModel.EmailCollectionView = emailListViewModel.Emails;

            bool focusItemCalled = false;
            emailListView.FocusItemAction = view =>
            {
                focusItemCalled = true;
            };

            var emailToDelete = root.Inbox.Emails.First();
            AssertHelper.PropertyChangedEvent(emailViewModel, x => x.Email, () => emailListViewModel.SelectedEmail = emailToDelete);
            Assert.AreEqual(emailToDelete, emailViewModel.Email);

            controller.DeleteEmailCommand.Execute(null);

            Assert.IsFalse(root.Inbox.Emails.Contains(emailToDelete));
            Assert.IsTrue(focusItemCalled);

            // Remove selection => delete command gets disabled

            AssertHelper.CanExecuteChangedEvent(controller.DeleteEmailCommand, () => emailListViewModel.SelectedEmail = null);
            Assert.IsFalse(controller.DeleteEmailCommand.CanExecute(null));

            // Select the second and last email and delete it

            emailListViewModel.SelectedEmail = root.Inbox.Emails.Single();
            controller.DeleteEmailCommand.Execute(null);
            Assert.IsFalse(root.Inbox.Emails.Any());
            Assert.IsNull(emailListViewModel.SelectedEmail);

            // Shutdown the controller

            controller.Shutdown();

            Assert.IsNull(emailLayoutViewModel.EmailListView);
            Assert.IsNull(emailLayoutViewModel.EmailView);
        }
    }
}
