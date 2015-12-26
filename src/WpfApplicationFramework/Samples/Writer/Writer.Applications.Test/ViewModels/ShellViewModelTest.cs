using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using System.Windows.Input;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Test.Writer.Applications.Services;
using Test.Writer.Applications.Views;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels
{
    [TestClass]
    public class ShellViewModelTest : TestClassBase
    {
        [TestMethod]
        public void PropertiesWithNotification()
        {
            ShellViewModel shellViewModel = Container.GetExportedValue<ShellViewModel>();

            ICommand printPreviewCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(shellViewModel, x => x.PrintPreviewCommand, () => shellViewModel.PrintPreviewCommand = printPreviewCommand);
            Assert.AreEqual(printPreviewCommand, shellViewModel.PrintPreviewCommand);

            ICommand printCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(shellViewModel, x => x.PrintCommand, () => shellViewModel.PrintCommand = printCommand);
            Assert.AreEqual(printCommand, shellViewModel.PrintCommand);

            ICommand exitCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(shellViewModel, x => x.ExitCommand, () => shellViewModel.ExitCommand = exitCommand);
            Assert.AreEqual(exitCommand, shellViewModel.ExitCommand);
        }
        
        [TestMethod]
        public void ShowAndClose()
        {
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            MockShellView shellView = Container.GetExportedValue<MockShellView>();
            ShellViewModel shellViewModel = Container.GetExportedValue<ShellViewModel>();
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();

            // Show the ShellView
            Assert.IsFalse(shellView.IsVisible);
            shellViewModel.Show();
            Assert.IsTrue(shellView.IsVisible);

            // In this case it tries to get the title of the unit test framework which is ""
            Assert.AreEqual("", shellViewModel.Title);

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
            AssertHelper.PropertyChangedEvent(shellViewModel, x => x.ExitCommand, () =>
                shellViewModel.ExitCommand = new DelegateCommand(() => shellViewModel.Close()));
            shellViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);
        }

        [TestMethod]
        public void SelectLanguageTest()
        {
            ShellViewModel shellViewModel = Container.GetExportedValue<ShellViewModel>();
            Assert.IsNull(shellViewModel.NewLanguage);

            shellViewModel.GermanCommand.Execute(null);
            Assert.AreEqual("de-DE", shellViewModel.NewLanguage.Name);

            shellViewModel.EnglishCommand.Execute(null);
            Assert.AreEqual("en-US", shellViewModel.NewLanguage.Name);
        }

        [TestMethod]
        public void RestoreWindowLocationAndSize()
        {
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            SetSettingsValues(20, 10, 400, 300, true);

            ShellViewModel shellViewModel = Container.GetExportedValue<ShellViewModel>();
            MockShellView shellView = Container.GetExportedValue<MockShellView>();
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
            MockPresentationService presentationService = (MockPresentationService)Container.GetExportedValue<IPresentationService>();
            presentationService.VirtualScreenWidth = 1000;
            presentationService.VirtualScreenHeight = 700;

            MockShellView shellView = Container.GetExportedValue<MockShellView>();
            IShellService shellService = Container.GetExportedValue<IShellService>();
            shellView.SetNAForLocationAndSize();

            SetSettingsValues();
            new ShellViewModel(shellView, null, presentationService, shellService, null).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Height is 0 => don't apply the Settings values
            SetSettingsValues(0, 0, 1, 0);
            new ShellViewModel(shellView, null, presentationService, shellService, null).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Left = 100 + Width = 901 > VirtualScreenWidth = 1000 => don't apply the Settings values
            SetSettingsValues(100, 100, 901, 100);
            new ShellViewModel(shellView, null, presentationService, shellService, null).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Top = 100 + Height = 601 > VirtualScreenWidth = 600 => don't apply the Settings values
            SetSettingsValues(100, 100, 100, 601);
            new ShellViewModel(shellView, null, presentationService, shellService, null).Close();
            AssertSettingsValues(double.NaN, double.NaN, double.NaN, double.NaN, false);

            // Use the limit values => apply the Settings values
            SetSettingsValues(0, 0, 1000, 700);
            new ShellViewModel(shellView, null, presentationService, shellService, null).Close();
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
