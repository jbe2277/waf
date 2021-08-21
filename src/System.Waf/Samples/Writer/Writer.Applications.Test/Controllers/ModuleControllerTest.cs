using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications.Services;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.Controllers;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : TestClassBase
    {
        protected override void OnCleanup()
        {
            MockSaveChangesView.ShowDialogAction = null;
            base.OnCleanup();
        }

        [TestMethod]
        public void ControllerLifecycle()
        {
            var controller = Get<ModuleController>();
            controller.Initialize();
            
            var shellView = Get<MockShellView>();
            var shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView)!;
            Assert.IsNotNull(shellViewModel.ExitCommand);

            controller.Run();

            Assert.IsTrue(shellView.IsVisible);
            var mainViewModel = Get<MainViewModel>();
            Assert.AreEqual(mainViewModel.View, shellViewModel.ContentView);

            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);

            controller.Shutdown();
        }

        [TestMethod]
        public void OpenFileViaCommandLine()
        {
            var environmentService = Get<MockEnvironmentService>();
            environmentService.DocumentFileName = "Document.mock";

            var controller = Get<ModuleController>();
            controller.Initialize();
            
            // Open the 'Document.mock' file
            controller.Run();

            // Open a file with an unknown file extension and check if an error message is shown.
            environmentService.DocumentFileName = "Unknown.fileExtension";
            var messageService = Get<MockMessageService>();
            messageService.Clear();
            
            controller.Run();

            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
        }

        [TestMethod]
        public void SaveChangesTest()
        {
            var controller = Get<ModuleController>();
            controller.Initialize();
            controller.Run();

            var shellViewModel = Get<ShellViewModel>();
            shellViewModel.FileService.NewCommand.Execute(null);
            
            var mainViewModel = Get<MainViewModel>();
            var richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>((IView)mainViewModel.ActiveDocumentView!)!;
            richTextViewModel.Document.Modified = true;

            bool showDialogCalled = false;
            MockSaveChangesView.ShowDialogAction = view =>
            {
                showDialogCalled = true;
                AssertHelper.SequenceEqual(new[] { richTextViewModel.Document }, ViewHelper.GetViewModel<SaveChangesViewModel>(view)!.Documents);
                view.Close();
            };

            // When we try to close the ShellView then the ApplicationController shows the SaveChangesView because the
            // modified document wasn't saved.
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            var shellView = Get<MockShellView>();
            Assert.IsTrue(shellView.IsVisible);

            showDialogCalled = false;
            MockSaveChangesView.ShowDialogAction = view =>
            {
                showDialogCalled = true;
                view.ViewModel.YesCommand.Execute(null);
            };

            var fileDialogService = Get<MockFileDialogService>();
            fileDialogService.Result = new FileDialogResult();

            // This time we let the SaveChangesView to save the modified document
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
            Assert.IsFalse(shellView.IsVisible);
        }

        [TestMethod]
        public void SettingsTest()
        {
            var settingsService = Get<MockSettingsService>();
            var settings = settingsService.Get<AppSettings>();
            settings.Culture = "de-DE";
            settings.UICulture = "de-AT";

            var controller = Get<ModuleController>();
            
            Assert.AreEqual(new CultureInfo("de-DE"), CultureInfo.CurrentCulture);
            Assert.AreEqual(new CultureInfo("de-AT"), CultureInfo.CurrentUICulture);

            controller.Initialize();
            controller.Run();

            var shellViewModel = Get<ShellViewModel>();
            shellViewModel.EnglishCommand.Execute(null);
            Assert.AreEqual(new CultureInfo("en-US"), shellViewModel.NewLanguage);

            shellViewModel.Close();
            controller.Shutdown();
            Assert.AreEqual("en-US", settings.UICulture);

            // Restore the culture settings
            CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        }
    }
}
