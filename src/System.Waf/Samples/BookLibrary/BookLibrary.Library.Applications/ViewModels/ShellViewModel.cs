using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Views;
using System;
using Waf.BookLibrary.Library.Applications.Services;

namespace Waf.BookLibrary.Library.Applications.ViewModels
{
    [Export]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private readonly IMessageService messageService;
        private readonly DelegateCommand aboutCommand;
        private ICommand saveCommand;
        private ICommand exitCommand;
        private bool isValid = true;
        private string databasePath = Resources.NotAvailable;

        
        [ImportingConstructor]
        public ShellViewModel(IShellView view, IMessageService messageService, IPresentationService presentationService,
            IShellService shellService) : base(view)
        {
            this.messageService = messageService;
            this.ShellService = shellService;
            this.aboutCommand = new DelegateCommand(ShowAboutMessage);
            view.Closing += ViewClosing;
            view.Closed += ViewClosed;

            // Restore the window size when the values are valid.
            if (Settings.Default.Left >= 0 && Settings.Default.Top >= 0 && Settings.Default.Width > 0 && Settings.Default.Height > 0
                && Settings.Default.Left + Settings.Default.Width <= presentationService.VirtualScreenWidth
                && Settings.Default.Top + Settings.Default.Height <= presentationService.VirtualScreenHeight)
            {
                ViewCore.Left = Settings.Default.Left;
                ViewCore.Top = Settings.Default.Top;
                ViewCore.Height = Settings.Default.Height;
                ViewCore.Width = Settings.Default.Width;
            }
            ViewCore.IsMaximized = Settings.Default.IsMaximized;
        }


        public string Title => ApplicationInfo.ProductName;

        public IShellService ShellService { get; }

        public ICommand AboutCommand => aboutCommand;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set { SetProperty(ref saveCommand, value); }
        }

        public ICommand ExitCommand
        {
            get { return exitCommand; }
            set { SetProperty(ref exitCommand, value); }
        }

        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }

        public string DatabasePath
        {
            get { return databasePath; }
            set { SetProperty(ref databasePath, value); }
        }


        public event CancelEventHandler Closing;


        public void Show()
        {
            ViewCore.Show();
        }

        public void Close()
        {
            ViewCore.Close();
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            OnClosing(e);
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            Settings.Default.Left = ViewCore.Left;
            Settings.Default.Top = ViewCore.Top;
            Settings.Default.Height = ViewCore.Height;
            Settings.Default.Width = ViewCore.Width;
            Settings.Default.IsMaximized = ViewCore.IsMaximized;
        }

        private void ShowAboutMessage()
        {
            messageService.ShowMessage(View, string.Format(CultureInfo.CurrentCulture, Resources.AboutText,
                ApplicationInfo.ProductName, ApplicationInfo.Version));
        }
    }
}
