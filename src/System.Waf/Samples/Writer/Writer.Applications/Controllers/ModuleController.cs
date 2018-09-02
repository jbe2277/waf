using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers
{
    /// <summary>
    /// Responsible for the application lifecycle.
    /// </summary>
    [Export(typeof(IModuleController)), Export]
    internal class ModuleController : IModuleController
    {
        private readonly IEnvironmentService environmentService;
        private readonly FileController fileController;
        private readonly RichTextDocumentController richTextDocumentController;
        private readonly PrintController printController;
        private readonly ShellViewModel shellViewModel;
        private readonly MainViewModel mainViewModel;
        private readonly StartViewModel startViewModel;
        private readonly DelegateCommand exitCommand;

        [ImportingConstructor]
        public ModuleController(IEnvironmentService environmentService, ISettingsService settingsService, IPresentationService presentationService, 
            ShellService shellService, Lazy<FileController> fileController, Lazy<RichTextDocumentController> richTextDocumentController, 
            Lazy<PrintController> printController, Lazy<ShellViewModel> shellViewModel, Lazy<MainViewModel> mainViewModel, 
            Lazy<StartViewModel> startViewModel)
        {
            // Initializing the cultures must be done first. Therefore, we inject the Controllers and ViewModels lazy.
            settingsService.ErrorOccurred += (sender, e) => Log.Default.Error(e.Error, "Error in SettingsService");
            InitializeCultures(settingsService.Get<AppSettings>());
            presentationService.InitializeCultures();

            this.environmentService = environmentService;
            this.fileController = fileController.Value;
            this.richTextDocumentController = richTextDocumentController.Value;
            this.printController = printController.Value;
            this.shellViewModel = shellViewModel.Value;
            this.mainViewModel = mainViewModel.Value;
            this.startViewModel = startViewModel.Value;

            shellService.ShellView = this.shellViewModel.View;
            this.shellViewModel.Closing += ShellViewModelClosing;
            exitCommand = new DelegateCommand(Close);
        }

        public void Initialize()
        {
            shellViewModel.ExitCommand = exitCommand;
            mainViewModel.StartView = startViewModel.View;
            
            printController.Initialize();
            fileController.Initialize();
        }

        public void Run()
        {
            shellViewModel.ContentView = mainViewModel.View;
            
            if (!string.IsNullOrEmpty(environmentService.DocumentFileName))
            {
                fileController.Open(environmentService.DocumentFileName);
            }
            
            shellViewModel.Show();
        }

        public void Shutdown()
        {
            fileController.Shutdown();
            printController.Shutdown();
        }

        private static void InitializeCultures(AppSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.Culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
            }
            if (!string.IsNullOrEmpty(settings.UICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(settings.UICulture);
            }
        }

        private void Close()
        {
            shellViewModel.Close();
        }

        private void ShellViewModelClosing(object sender, CancelEventArgs e)
        {
            // Try to close all documents and see if the user has already saved them.
            e.Cancel = !fileController.CloseAll();
        }
    }
}
