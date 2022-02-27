namespace Waf.InformationManager.Infrastructure.Interfaces.Applications;

/// <summary>Exposes the navigation functionality of the shell.</summary>
public interface INavigationService
{
    /// <summary>Adds a navigation node in the navigation view of the shell.</summary>
    /// <param name="name">The name of the node.</param>
    /// <param name="showAction">The show action which is called when the user selects the node.</param>
    /// <param name="closeAction">The close action which is called when the node is deselected.</param>
    /// <param name="group">The group number defines the position in the navigation view. All items with the same group number are considered 
    /// to be in the same group. The navigation list is ordered from lower to higher numbers.</param>
    /// <param name="order">The order defines the position in the group. The navigation list is ordered from lower to higher numbers.</param>
    /// <returns>The created navigation node.</returns>
    INavigationNode AddNavigationNode(string name, Action showAction, Action closeAction, double group, double order);
}
