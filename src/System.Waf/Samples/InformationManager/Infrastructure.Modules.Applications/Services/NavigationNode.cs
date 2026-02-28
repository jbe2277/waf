using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services;

public class NavigationNode : Model, INavigationNode
{
    private readonly Action showAction;
    private readonly Action closeAction;

    public NavigationNode(string automationId, string name, Action showAction, Action closeAction, double group, double order)
    {
        ArgumentException.ThrowIfNullOrEmpty(automationId);
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfLessThan(group, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(order, 0);
        AutomationId = automationId;
        Name = name;
        this.showAction = showAction ?? throw new ArgumentNullException(nameof(showAction));
        this.closeAction = closeAction ?? throw new ArgumentNullException(nameof(closeAction));
        Group = group;
        Order = order;
    }

    public string AutomationId { get; }

    public string Name { get; }

    public double Group { get; }

    public double Order { get; }

    public int? ItemCount { get; set => SetProperty(ref field, value); }

    public bool IsSelected
    {
        get;
        set
        {
            if (field == value) return;
            if (field) closeAction();
            field = value;
            RaisePropertyChanged();
            if (field) showAction();
        }
    }

    public bool IsFirstItemOfNewGroup { get; set => SetProperty(ref field, value); }
}
