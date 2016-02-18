using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Properties;
using Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Controllers
{
    [Export(typeof(IModuleController)), Export]
    internal class ModuleController : IModuleController
    {
        private readonly Lazy<DocumentController> documentController;
        private readonly Lazy<ShellViewModel> shellViewModel;

        
        [ImportingConstructor]
        public ModuleController(Lazy<DocumentController> documentController, Lazy<ShellViewModel> shellViewModel)
        {
            this.documentController = documentController;
            this.shellViewModel = shellViewModel;
        }


        private ShellViewModel ShellViewModel => shellViewModel.Value;
        
        private DocumentController DocumentController => documentController.Value;


        public void Initialize()
        {
            // Upgrade the settings from a previous version when the new version starts the first time.
            if (Settings.Default.IsUpgradeNeeded)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsUpgradeNeeded = false;
            }

            DocumentController.Initialize();
        }

        public void Run()
        {
            ShellViewModel.Show();
        }

        public void Shutdown()
        {
            DocumentController.Shutdown();
            
            try
            {
                Settings.Default.Save();
            }
            catch (Exception)
            {
                // When more application instances are closed at the same time then an exception occurs.
            }
        }
    }
}
