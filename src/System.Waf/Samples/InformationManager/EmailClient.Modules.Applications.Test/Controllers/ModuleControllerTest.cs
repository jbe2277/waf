using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;

namespace Test.InformationManager.EmailClient.Modules.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : EmailClientTest
    {
        protected override void OnCleanup()
        {
            MockNewEmailView.ShowAction = null;
            base.OnCleanup();
        }

        [TestMethod]
        public void SaveAndLoad()
        {
            using var stream = new MasterMemoryStream();
            var documentService = Get<MockDocumentService>();
            documentService.GetStreamAction = (_, _, _) =>
            {
                stream.Position = 0;
                return stream;
            };

            var controller = Get<ModuleController>();
            controller.Initialize();
            controller.Run();
            var root1 = controller.Root;
            root1.EmailAccounts[0].Name = "Test name";
            controller.Shutdown();

            controller.Initialize();
            controller.Run();
            var root2 = controller.Root;
            Assert.AreNotSame(root1, root2);
            Assert.AreEqual("Test name", root2.EmailAccounts[0].Name);
            controller.Shutdown();

            stream.MasterDispose();
        }
        
        [TestMethod]
        public void ShowAndCloseEmailViews()
        {
            var controller = Get<ModuleController>();

            // Initialize the controller

            controller.Initialize();

            Assert.IsTrue(controller.Root.EmailAccounts.Any());
            Assert.IsTrue(controller.Root.Inbox.Emails.Any());
            Assert.IsTrue(controller.Root.Sent.Emails.Any());
            Assert.IsTrue(controller.Root.Drafts.Emails.Any());

            var navigationService = Get<MockNavigationService>();
            Assert.AreEqual(5, navigationService.NavigationNodes.Count);
            Assert.AreEqual("Inbox", navigationService.NavigationNodes[0].Name);
            Assert.AreEqual("Outbox", navigationService.NavigationNodes[1].Name);
            Assert.AreEqual("Sent", navigationService.NavigationNodes[2].Name);
            Assert.AreEqual("Drafts", navigationService.NavigationNodes[3].Name);
            Assert.AreEqual("Deleted", navigationService.NavigationNodes[4].Name);

            Assert.AreEqual(controller.Root.Inbox.Emails.Count, navigationService.NavigationNodes[0].ItemCount);
            Assert.AreEqual(controller.Root.Outbox.Emails.Count, navigationService.NavigationNodes[1].ItemCount);
            Assert.AreEqual(controller.Root.Sent.Emails.Count, navigationService.NavigationNodes[2].ItemCount);
            Assert.AreEqual(controller.Root.Drafts.Emails.Count, navigationService.NavigationNodes[3].ItemCount);
            Assert.AreEqual(controller.Root.Deleted.Emails.Count, navigationService.NavigationNodes[4].ItemCount);

            // Run the controller

            controller.Run();

            // Show the inbox

            var shellService = Get<MockShellService>();
            Assert.IsNull(shellService.ContentView);
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            navigationService.NavigationNodes[0].ShowAction();

            Assert.IsNotNull(shellService.ContentView);
            Assert.AreEqual(3, shellService.ToolBarCommands.Count);

            navigationService.NavigationNodes[0].CloseAction();

            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the outbox

            navigationService.NavigationNodes[1].ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes[1].CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the sent emails

            navigationService.NavigationNodes[2].ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes[2].CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the drafts

            navigationService.NavigationNodes[3].ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes[3].CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the deleted emails

            navigationService.NavigationNodes[4].ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes[4].CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Shutdown the controller

            controller.Shutdown();
        }

        [TestMethod]
        public void ItemCountSynchronizerTest()
        {
            var controller = Get<ModuleController>();
            controller.Initialize();

            var inbox = controller.Root.Inbox;
            var navigationService = Get<MockNavigationService>();
            var inboxNode = navigationService.NavigationNodes[0];

            Assert.AreEqual(inbox.Emails.Count, inboxNode.ItemCount);
            inbox.RemoveEmail(inbox.Emails[0]);
            Assert.AreEqual(inbox.Emails.Count, inboxNode.ItemCount);
        }

        [TestMethod]
        public void NewEmail()
        {
            var controller = Get<ModuleController>();
            controller.Initialize();

            var navigationService = Get<MockNavigationService>();
            var inboxNode = navigationService.NavigationNodes[0];
            
            inboxNode.ShowAction();
            
            var shellService = Get<MockShellService>();

            bool isShowCalled = false;
            MockNewEmailView.ShowAction = _ => isShowCalled = true;
            
            shellService.ToolBarCommands[0].Command.Execute(null);
            Assert.IsTrue(isShowCalled);
        }


        private class MasterMemoryStream : MemoryStream
        {
            protected override void Dispose(bool disposing)
            {
                // Do not dispose the stream now.
            }

            public void MasterDispose() => base.Dispose(true);
        }
    }
}
