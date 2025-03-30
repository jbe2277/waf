using System.Waf.Applications;
using System.Waf.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;

internal class ModuleController : IModuleController
{
    private readonly Lazy<ShellViewModel> shellViewModel;

    public ModuleController(ISettingsService settingsService, Lazy<ShellViewModel> shellViewModel)
    {
        this.shellViewModel = shellViewModel;
        settingsService.ErrorOccurred += (sender, e) => Log.Default.Error(e.Error, "Error in SettingsService");
    }

    private ShellViewModel ShellViewModel => shellViewModel.Value;


    public void Initialize() { }

    public void Run() => ShellViewModel.Show();

    public void Shutdown() { }
}
