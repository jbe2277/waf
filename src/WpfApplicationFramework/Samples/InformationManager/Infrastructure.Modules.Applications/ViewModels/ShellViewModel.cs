using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Properties;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels
{
    [Export, Export(typeof(IShellViewModel))]
    public class ShellViewModel : ViewModel<IShellView>, IShellViewModel
    {
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly NavigationService navigationService;
        private readonly DelegateCommand exitCommand;
        private readonly DelegateCommand aboutCommand;

        
        [ImportingConstructor]
        public ShellViewModel(IShellView view, IMessageService messageService, ShellService shellService, NavigationService navigationService)
            : base(view)
        {
            this.messageService = messageService;
            this.shellService = shellService;
            this.navigationService = navigationService;
            this.exitCommand = new DelegateCommand(Close);
            this.aboutCommand = new DelegateCommand(ShowAboutMessage);

            view.Closed += ViewClosed;

            // Restore the window size when the values are valid.
            if (Settings.Default.Left >= 0 && Settings.Default.Top >= 0 && Settings.Default.Width > 0 && Settings.Default.Height > 0
                && Settings.Default.Left + Settings.Default.Width <= view.VirtualScreenWidth
                && Settings.Default.Top + Settings.Default.Height <= view.VirtualScreenHeight)
            {
                ViewCore.Left = Settings.Default.Left;
                ViewCore.Top = Settings.Default.Top;
                ViewCore.Height = Settings.Default.Height;
                ViewCore.Width = Settings.Default.Width;
            }
            ViewCore.IsMaximized = Settings.Default.IsMaximized;
        }


        public string Title { get { return ApplicationInfo.ProductName; } }

        public IShellService ShellService { get { return shellService; } }

        public NavigationService NavigationService { get { return navigationService; } }

        public ICommand ExitCommand { get { return exitCommand; } }

        public ICommand AboutCommand { get { return aboutCommand; } }


        public void Show()
        {
            ViewCore.Show();
        }

        public void Close()
        {
            ViewCore.Close();
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
            messageService.ShowMessage(View, string.Format(CultureInfo.CurrentCulture, 
                "{0} {1}\n\nThis software is a reference sample of the WPF Application Framework (WAF).\n\nhttp://waf.codeplex.com",
                ApplicationInfo.ProductName, ApplicationInfo.Version));
        }


        public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands)
        {
            ViewCore.AddToolBarCommands(commands);
        }

        public void ClearToolBarCommands()
        {
            ViewCore.ClearToolBarCommands();
        }
    }
}
