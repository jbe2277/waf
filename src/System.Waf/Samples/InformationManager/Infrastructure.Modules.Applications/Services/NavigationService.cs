using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services
{
    [Export(typeof(INavigationService)), Export]
    public class NavigationService : Model, INavigationService
    {
        private readonly ObservableCollection<NavigationNode> navigationNodes;

        public NavigationService()
        {
            navigationNodes = new ObservableCollection<NavigationNode>();
        }

        public IReadOnlyList<NavigationNode> NavigationNodes => navigationNodes;

        public INavigationNode AddNavigationNode(string name, Action showAction, Action closeAction, double group, double order)
        {
            var navigationNode = new NavigationNode(name, showAction, closeAction, group, order);

            // Insert the new navigation node at the right index.
            var sameGroup = navigationNodes.Where(x => DoubleEquals(x.Group, group, 1E-9)).ToArray();
            if (sameGroup.Any())
            {
                var nextNode = sameGroup.FirstOrDefault(x => x.Order > order);
                int nextNodeIndex = nextNode != null ? navigationNodes.IndexOf(nextNode) : navigationNodes.IndexOf(sameGroup.Last()) + 1;
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
            double oldGroup = navigationNodes.First().Order;
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

        private static bool DoubleEquals(double value1, double value2, double delta)
        {
            return Math.Abs(value1 - value2) <= delta;
        }
    }
}
