using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels
{
    [Export]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private readonly IMessageService messageService;
        private object contentView;
        private bool isPrintPreviewVisible;
        private CultureInfo newLanguage;

        [ImportingConstructor]
        public ShellViewModel(IShellView view, IMessageService messageService, IPresentationService presentationService, IShellService shellService, IFileService fileService)
            : base(view)
        {
            this.messageService = messageService;
            ShellService = shellService;
            FileService = fileService;
            EnglishCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("en-US")));
            GermanCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("de-DE")));
            AboutCommand = new DelegateCommand(ShowAboutMessage);

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

        public static string Title => ApplicationInfo.ProductName;

        public IShellService ShellService { get; }

        public IFileService FileService { get; }

        public CultureInfo NewLanguage => newLanguage;

        public ICommand EnglishCommand { get; }

        public ICommand GermanCommand { get; }

        public ICommand AboutCommand { get; }

        public ICommand PrintPreviewCommand { get; set; }

        public ICommand ClosePrintPreviewCommand { get; set; }

        public ICommand PrintCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }

        public bool IsPrintPreviewVisible
        {
            get { return isPrintPreviewVisible; }
            set { SetProperty(ref isPrintPreviewVisible, value); }
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

        private void SelectLanguage(CultureInfo uiCulture)
        {
            if (!uiCulture.Equals(CultureInfo.CurrentUICulture))
            {
                messageService.ShowMessage(ShellService.ShellView, Resources.RestartApplication + "\n\n" +
                    Resources.ResourceManager.GetString("RestartApplication", uiCulture));
            }
            newLanguage = uiCulture;
        }

        private void ShowAboutMessage()
        {
            messageService.ShowMessage(ShellService.ShellView, string.Format(CultureInfo.CurrentCulture, Resources.AboutText,
                ApplicationInfo.ProductName, ApplicationInfo.Version));
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
    }
}
