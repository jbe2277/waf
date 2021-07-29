using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications.Services;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels
{
    [TestClass]
    public class ShellViewModelTest : TestClassBase
    {
        [TestMethod]
        public void ShowAndClose()
        {
            var messageService = Get<MockMessageService>();
            var shellView = Get<MockShellView>();
            var shellViewModel = Get<ShellViewModel>();

            // Show the ShellView
            Assert.IsFalse(shellView.IsVisible);
            shellViewModel.Show();
            Assert.IsTrue(shellView.IsVisible);

            Assert.AreEqual(1d, shellViewModel.ShellService.ActiveZoomCommands.Zoom);

            // Show the About Dialog
            Assert.IsNull(messageService.Message);
            shellViewModel.AboutCommand.Execute(null);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);
            Assert.IsNotNull(messageService.Message);

            // Try to close the ShellView but cancel this operation through the closing event
            bool cancelClosing = true;
            shellViewModel.Closing += (sender, e) =>
            {
                e.Cancel = cancelClosing;
            };
            shellViewModel.Close();
            Assert.IsTrue(shellView.IsVisible);

            // Close the ShellView via the ExitCommand
            cancelClosing = false;
            shellViewModel.ExitCommand = new DelegateCommand(() => shellViewModel.Close());
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);
        }

        [TestMethod]
        public void SelectLanguageTest()
        {
            var shellViewModel = Get<ShellViewModel>();
            Assert.IsNull(shellViewModel.NewLanguage);

            shellViewModel.GermanCommand.Execute(null);
            Assert.AreEqual("de-DE", shellViewModel.NewLanguage!.Name);

            shellViewModel.EnglishCommand.Execute(null);
            Assert.AreEqual("en-US", shellViewModel.NewLanguage.Name);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSize()
        {
            var presentationService = Get<MockPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            var settingsService = Get<ISettingsService>();
            var settings = settingsService.Get<AppSettings>();
            SetSettingsValues(settings, 20, 10, 400, 300, true);

            var shellViewModel = Get<ShellViewModel>();
            var shellView = (MockShellView)shellViewModel.View;
            Assert.AreEqual(20, shellView.Left);
            Assert.AreEqual(10, shellView.Top);
            Assert.AreEqual(400, shellView.Width);
            Assert.AreEqual(300, shellView.Height);
            Assert.IsTrue(shellView.IsMaximized);

            shellView.Left = 25;
            shellView.Top = 15;
            shellView.Width = 450;
            shellView.Height = 350;
            shellView.IsMaximized = false;

            shellView.Close();
            AssertSettingsValues(settings, 25, 15, 450, 350, false);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSizeSpecial()
        {
            var messageService = Get<MockMessageService>();
            var fileService = Get<IFileService>();
            var presentationService = Get<MockPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            var shellView = Get<MockShellView>();
            var shellService = Get<IShellService>();
            var settingsService = Get<ISettingsService>();
            var settings = settingsService.Get<AppSettings>();
            shellView.SetNAForLocationAndSize();

            SetSettingsValues(settings);
            new ShellViewModel(shellView, messageService, presentationService, shellService, fileService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Height is 0 => don't apply the Settings values
            SetSettingsValues(settings, 0, 0, 1, 0);
            new ShellViewModel(shellView, messageService, presentationService, shellService, fileService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Left = 100 + Width = 901 > VirtualScreenWidth = 1000 => don't apply the Settings values
            SetSettingsValues(settings, 100, 100, 901, 100);
            new ShellViewModel(shellView, messageService, presentationService, shellService, fileService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Top = 100 + Height = 601 > VirtualScreenWidth = 600 => don't apply the Settings values
            SetSettingsValues(settings, 100, 100, 100, 601);
            new ShellViewModel(shellView, messageService, presentationService, shellService, fileService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Use the limit values => apply the Settings values
            SetSettingsValues(settings, 0, 0, 1000, 700);
            new ShellViewModel(shellView, messageService, presentationService, shellService, fileService, settingsService).Close();
            AssertSettingsValues(settings, 0, 0, 1000, 700, false);
        }

        private static void SetSettingsValues(AppSettings settings, double left = 0, double top = 0, double width = 0, double height = 0, bool isMaximized = false)
        {
            settings.Left = left;
            settings.Top = top;
            settings.Width = width;
            settings.Height = height;
            settings.IsMaximized = isMaximized;
        }

        private static void AssertSettingsValues(AppSettings settings, double left, double top, double width, double height, bool isMaximized)
        {
            Assert.AreEqual(left, settings.Left);
            Assert.AreEqual(top, settings.Top);
            Assert.AreEqual(width, settings.Width);
            Assert.AreEqual(height, settings.Height);
            Assert.AreEqual(isMaximized, settings.IsMaximized);
        }
    }
}
