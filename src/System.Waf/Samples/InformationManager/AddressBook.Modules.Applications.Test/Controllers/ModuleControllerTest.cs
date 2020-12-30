using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Waf.Applications;
using Test.InformationManager.AddressBook.Modules.Applications.Views;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.AddressBook.Modules.Applications.Controllers;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

namespace Test.InformationManager.AddressBook.Modules.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : AddressBookTest
    {
        [TestMethod]
        public void SaveAndLoad()
        {
            var stream = new MasterMemoryStream();
            var documentService = Get<MockDocumentService>();
            documentService.GetStreamAction = (documentPartPath, contentType, fileMode) =>
            {
                stream.Position = 0;
                return stream;
            };

            var controller = Get<ModuleController>();
            controller.Initialize();
            controller.Run();
            var root1 = controller.Root;
            root1.Contacts.First().Firstname = "Test name";
            controller.Shutdown();

            controller.Initialize();
            controller.Run();
            var root2 = controller.Root;
            Assert.AreNotSame(root1, root2);
            Assert.AreEqual("Test name", root2.Contacts.First().Firstname);
            controller.Shutdown();

            stream.MasterDispose();
        }
        
        [TestMethod]
        public void ShowAndCloseAddressBook()
        {
            var controller = Get<ModuleController>();
            
            // Initialize the controller

            controller.Initialize();

            Assert.IsTrue(controller.Root.Contacts.Any());
            var navigationService = Get<MockNavigationService>();
            var node = navigationService.NavigationNodes.Single();
            Assert.AreEqual("Contacts", node.Name);

            // Run the controller

            controller.Run();

            // Show the address book

            var shellService = Get<MockShellService>();
            Assert.IsNull(shellService.ContentView);
            Assert.IsFalse(shellService.ToolBarCommands.Any());
            
            node.ShowAction();

            Assert.IsNotNull(shellService.ContentView);
            Assert.AreEqual(2, shellService.ToolBarCommands.Count);

            // Close the address book

            node.CloseAction();

            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Shutdown the controller

            controller.Shutdown();
        }

        [TestMethod]
        public void ShowSelectContactViewTest()
        {
            var controller = Get<ModuleController>();
            controller.Initialize();
            controller.Run();

            // Show the select contact view

            MockSelectContactView.ShowDialogAction = view =>
            {
                var viewModel = ViewHelper.GetViewModel<SelectContactViewModel>(view)!;
                viewModel.OkCommand.Execute(null);
            };
            var ownerView = new object();
            var contactDto = controller.ShowSelectContactView(ownerView);

            Assert.IsNotNull(contactDto);

            MockSelectContactView.ShowDialogAction = null;

            controller.Shutdown();
        }


        private class MasterMemoryStream : MemoryStream
        {
            protected override void Dispose(bool disposing)
            {
                // Do not dispose the stream now.
            }

            public void MasterDispose()
            {
                base.Dispose(true);
            }
        }
    }
}
