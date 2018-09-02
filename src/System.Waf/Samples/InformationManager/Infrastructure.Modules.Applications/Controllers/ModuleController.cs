using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
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
            DocumentController.Initialize();
        }

        public void Run()
        {
            ShellViewModel.Show();
        }

        public void Shutdown()
        {
            DocumentController.Shutdown();
        }
    }
}
