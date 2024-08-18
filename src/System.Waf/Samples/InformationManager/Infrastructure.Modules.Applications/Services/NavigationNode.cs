using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services;

public class NavigationNode : Model, INavigationNode
{
    private readonly Action showAction;
    private readonly Action closeAction;
    private int? itemCount;
    private bool isSelected;
    private bool isFirstItemOfNewGroup;

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

    public int? ItemCount
    {
        get => itemCount;
        set => SetProperty(ref itemCount, value);
    }

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            if (isSelected == value) return;
            if (isSelected) closeAction();
            isSelected = value;
            RaisePropertyChanged();
            if (isSelected) showAction();
        }
    }

    public bool IsFirstItemOfNewGroup
    {
        get => isFirstItemOfNewGroup;
        set => SetProperty(ref isFirstItemOfNewGroup, value);
    }
}
