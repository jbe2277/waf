using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.BookLibrary.Library.Applications.Properties;
using Waf.BookLibrary.Library.Applications.Services;
using Waf.BookLibrary.Library.Applications.ViewModels;

namespace Waf.BookLibrary.Library.Applications.Controllers
{
    /// <summary>
    /// Responsible for the module lifecycle.
    /// </summary>
    [Export(typeof(IModuleController)), Export]
    internal class ModuleController : IModuleController
    {
        private readonly IMessageService messageService;
        private readonly IEntityController entityController;
        private readonly BookController bookController;
        private readonly PersonController personController;
        private readonly ShellService shellService;
        private readonly Lazy<ShellViewModel> shellViewModel;
        private readonly DelegateCommand exitCommand;
        

        [ImportingConstructor]
        public ModuleController(IMessageService messageService, IPresentationService presentationService, 
            IEntityController entityController, BookController bookController, PersonController personController, 
            ShellService shellService, Lazy<ShellViewModel> shellViewModel)
        {
            presentationService.InitializeCultures();
            
            this.messageService = messageService;
            this.entityController = entityController;
            this.bookController = bookController;
            this.personController = personController;
            this.shellService = shellService;
            this.shellViewModel = shellViewModel;
            this.exitCommand = new DelegateCommand(Close);
        }


        private ShellViewModel ShellViewModel { get { return shellViewModel.Value; } }


        public void Initialize()
        {
            // Upgrade the settings from a previous version when the new version starts the first time.
            if (Settings.Default.IsUpgradeNeeded)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsUpgradeNeeded = false;
            }

            shellService.ShellView = ShellViewModel.View;
            ShellViewModel.ExitCommand = exitCommand;
            ShellViewModel.Closing += ShellViewModelClosing;

            entityController.Initialize();
            bookController.Initialize();
            personController.Initialize();
        }

        public void Run()
        {
            ShellViewModel.Show();
        }

        public void Shutdown()
        {
            entityController.Shutdown();

            try
            {
                Settings.Default.Save();
            }
            catch (Exception)
            {
                // When more application instances are closed at the same time then an exception occurs.
            }
        }

        private void ShellViewModelClosing(object sender, CancelEventArgs e)
        {
            if (entityController.HasChanges())
            {
                if (entityController.CanSave())
                {
                    bool? result = messageService.ShowQuestion(shellService.ShellView, Resources.SaveChangesQuestion);
                    if (result == true)
                    {
                        if (!entityController.Save())
                        {
                            e.Cancel = true;
                        }
                    }
                    else if (result == null)
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = !messageService.ShowYesNoQuestion(shellService.ShellView, Resources.LoseChangesQuestion);
                }
            }
        }

        private void Close()
        {
            ShellViewModel.Close();
        }
    }
}
