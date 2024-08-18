namespace Waf.InformationManager.Infrastructure.Interfaces.Applications;

/// <summary>Represents a navigation node.</summary>
public interface INavigationNode : INotifyPropertyChanged
{
    /// <summary>ID used for UI Automation.</summary>
    string AutomationId { get; }

    /// <summary>Gets the name.</summary>
    string Name { get; }

    /// <summary>Gets or sets the item count. If ItemCount is not relevant for this node then the value is null.</summary>
    int? ItemCount { get; set; }
}
