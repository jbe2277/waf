using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using System.ComponentModel.Composition;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    [Export(typeof(INavigationService)), Export]
    public class MockNavigationService : INavigationService
    {
        private readonly List<MockNavigationNode> navigationNodes;

        public MockNavigationService()
        {
            navigationNodes = new List<MockNavigationNode>();
        }

        public IReadOnlyList<MockNavigationNode> NavigationNodes => navigationNodes;
        
        public INavigationNode AddNavigationNode(string name, Action showAction, Action closeAction, double group, double order)
        {
            var node = new MockNavigationNode(name, showAction, closeAction, group, order);
            navigationNodes.Add(node);
            return node;
        }
    }
}
