using System.ComponentModel.Composition;
using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services;

[Export(typeof(INavigationService)), Export]
public class NavigationService : Model, INavigationService
{
    private readonly ObservableList<NavigationNode> navigationNodes = [];

    public IReadOnlyObservableList<NavigationNode> NavigationNodes => navigationNodes;

    public INavigationNode AddNavigationNode(string automationId, string name, Action showAction, Action closeAction, double group, double order)
    {
        var navigationNode = new NavigationNode(automationId, name, showAction, closeAction, group, order);

        // Insert the new navigation node at the right index.
        var sameGroup = navigationNodes.Where(x => DoubleEquals(x.Group, group, 1E-9)).ToArray();
        if (sameGroup.Any())
        {
            var nextNode = sameGroup.FirstOrDefault(x => x.Order > order);
            int nextNodeIndex = nextNode != null ? navigationNodes.IndexOf(nextNode) : navigationNodes.IndexOf(sameGroup[^1]) + 1;
            navigationNodes.Insert(nextNodeIndex, navigationNode);
        }
        else
        {
            var nextNode = navigationNodes.FirstOrDefault(x => x.Group > group);
            if (nextNode != null)
            {
                int nextNodeIndex = navigationNodes.IndexOf(nextNode);
                navigationNodes.Insert(nextNodeIndex, navigationNode);
            }
            else
            {
                navigationNodes.Add(navigationNode);
            }
        }

        // Mark the first nodes of a new group. But don't mark the very first node.
        double oldGroup = navigationNodes[0].Order;
        foreach (var node in navigationNodes.Skip(1))
        {
            if (!DoubleEquals(oldGroup, node.Group, 1E-9))
            {
                node.IsFirstItemOfNewGroup = true;
                oldGroup = node.Group;
            }
            else
            {
                node.IsFirstItemOfNewGroup = false;
            }
        }
        return navigationNode;
    }

    private static bool DoubleEquals(double value1, double value2, double delta) => Math.Abs(value1 - value2) <= delta;
}
