using System.ComponentModel.Composition.Hosting;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Applications.Controllers;
using Waf.BookLibrary.Library.Applications.Properties;
using Test.BookLibrary.Library.Applications.Controllers;
using Test.BookLibrary.Library.Applications.Services;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Applications.Services;
using System.Waf.UnitTesting.Mocks;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class ShellViewModelTest : TestClassBase
    {
        [TestMethod]
        public void ShellViewModelBasicTest()
        {
            MockShellView shellView = Container.GetExportedValue<MockShellView>();
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            IShellService shellService = Container.GetExportedValue<IShellService>();
            ShellViewModel shellViewModel = Container.GetExportedValue<ShellViewModel>();

            // The title isn't available in the unit test environment.
            Assert.AreEqual("", shellViewModel.Title);

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
            shellViewModel.Closing += (sender, e) =>
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
            var shellViewModel = Container.GetExportedValue<ShellViewModel>();

            Assert.IsTrue(shellViewModel.IsValid);
            AssertHelper.PropertyChangedEvent(shellViewModel, x => x.IsValid, () =>
                shellViewModel.IsValid = false);
            Assert.IsFalse(shellViewModel.IsValid);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSize()
        {
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            SetSettingsValues(20, 10, 400, 300, true);

            ShellViewModel shellViewModel = Container.GetExportedValue<ShellViewModel>();
            MockShellView shellView = (MockShellView)Container.GetExportedValue<IShellView>();
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
            AssertSettingsValues(25, 15, 450, 350, false);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSizeSpecial()
        {
            IMessageService messageService = Container.GetExportedValue<IMessageService>();
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            IShellService shellService = Container.GetExportedValue<IShellService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            MockShellView shellViewMock = (MockShellView)Container.GetExportedValue<IShellView>();
            shellViewMock.SetNAForLocationAndSize();

            SetSettingsValues();
            new ShellViewModel(shellViewMock, messageService, presentationService, shellService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Height is 0 => don't apply the Settings values
            SetSettingsValues(0, 0, 1, 0);
            new ShellViewModel(shellViewMock, messageService, presentationService, shellService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Left = 100 + Width = 901 > VirtualScreenWidth = 1000 => don't apply the Settings values
            SetSettingsValues(100, 100, 901, 100);
            new ShellViewModel(shellViewMock, messageService, presentationService, shellService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Top = 100 + Height = 601 > VirtualScreenWidth = 600 => don't apply the Settings values
            SetSettingsValues(100, 100, 100, 601);
            new ShellViewModel(shellViewMock, messageService, presentationService, shellService).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Use the limit values => apply the Settings values
            SetSettingsValues(0, 0, 1000, 700);
            new ShellViewModel(shellViewMock, messageService, presentationService, shellService).Close();
            AssertSettingsValues(0, 0, 1000, 700, false);
        }


        private void SetSettingsValues(double left = 0, double top = 0, double width = 0, double height = 0, bool isMaximized = false)
        {
            Settings.Default.Left = left;
            Settings.Default.Top = top;
            Settings.Default.Width = width;
            Settings.Default.Height = height;
            Settings.Default.IsMaximized = isMaximized;
        }

        private void AssertSettingsValues(double left, double top, double width, double height, bool isMaximized)
        {
            Assert.AreEqual(left, Settings.Default.Left);
            Assert.AreEqual(top, Settings.Default.Top);
            Assert.AreEqual(width, Settings.Default.Width);
            Assert.AreEqual(height, Settings.Default.Height);
            Assert.AreEqual(isMaximized, Settings.Default.IsMaximized);
        }
    }
}
