using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class ShellViewModelTest : TestClassBase
    {
        [TestMethod]
        public void ShellViewModelBasicTest()
        {
            var shellView = Get<MockShellView>();
            var messageService = Get<MockMessageService>();
            var shellService = Get<IShellService>();
            var shellViewModel = Get<ShellViewModel>();

            Assert.AreEqual(shellService, shellViewModel.ShellService);

            // Show the ShellView
            shellViewModel.Show();
            Assert.IsTrue(shellView.IsVisible);

            // Show the about message box
            messageService.Clear();
            shellViewModel.AboutCommand.Execute(null);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
            Assert.AreEqual(MessageType.Message, messageService.MessageType);

            // Close the ShellView
            bool closingEventRaised = false;
            shellViewModel.Closing += (_, _) =>
            {
                closingEventRaised = true;
            };
            shellViewModel.Close();
            Assert.IsFalse(shellView.IsVisible);
            Assert.IsTrue(closingEventRaised);
        }

        [TestMethod]
        public void ShellViewModelPropertiesTest()
        {
            var shellViewModel = Get<ShellViewModel>();

            Assert.IsTrue(shellViewModel.IsValid);
            AssertHelper.PropertyChangedEvent(shellViewModel, x => x.IsValid, () => shellViewModel.IsValid = false);
            Assert.IsFalse(shellViewModel.IsValid);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSize()
        {
            var settingsService = Get<ISettingsService>();
            var settings = settingsService.Get<AppSettings>();
            SetSettingsValues(settings, 20, 10, 400, 300, true);

            var shellView = Get<MockShellView>();
            shellView.VirtualScreenWidth = 1000;
            shellView.VirtualScreenHeight = 700;
            Get<ShellViewModel>();
            
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
            var messageService = Get<IMessageService>();
            var shellService = Get<IShellService>();

            var shellView = Get<MockShellView>();
            shellView.VirtualScreenWidth = 1000;
            shellView.VirtualScreenHeight = 700;
            var settingsService = Get<ISettingsService>();
            var settings = settingsService.Get<AppSettings>();
            shellView.SetNAForLocationAndSize();

            SetSettingsValues(settings);
            new ShellViewModel(shellView, messageService, shellService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Height is 0 => don't apply the Settings values
            SetSettingsValues(settings, 0, 0, 1, 0);
            new ShellViewModel(shellView, messageService, shellService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Left = 100 + Width = 901 > VirtualScreenWidth = 1000 => don't apply the Settings values
            SetSettingsValues(settings, 100, 100, 901, 100);
            new ShellViewModel(shellView, messageService, shellService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Top = 100 + Height = 601 > VirtualScreenWidth = 600 => don't apply the Settings values
            SetSettingsValues(settings, 100, 100, 100, 601);
            new ShellViewModel(shellView, messageService, shellService, settingsService).Close();
            AssertSettingsValues(settings, double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Use the limit values => apply the Settings values
            SetSettingsValues(settings, 0, 0, 1000, 700);
            new ShellViewModel(shellView, messageService, shellService, settingsService).Close();
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
