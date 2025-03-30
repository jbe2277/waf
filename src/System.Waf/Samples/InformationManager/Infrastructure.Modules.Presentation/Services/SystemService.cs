using System.IO;
using System.Waf.Applications;
using System.Windows.Input;
using System.Windows;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services;

internal sealed class SystemService : ISystemService
{
    private readonly Lazy<string> dataDirectory = new(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationInfo.Company, ApplicationInfo.ProductName));

    public string DataDirectory => dataDirectory.Value;

    public void CommitUIChanges()
    {
        var element = Keyboard.FocusedElement as FrameworkElement;
        element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        element?.Focus();
    }
}
