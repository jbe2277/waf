using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.navigationNodes = new List<MockNavigationNode>();
        }


        public IReadOnlyList<MockNavigationNode> NavigationNodes { get { return navigationNodes; } }
        

        public INavigationNode AddNavigationNode(string name, Action showAction, Action closeAction, double group, double order)
        {
            var node = new MockNavigationNode(name, showAction, closeAction, group, order);
            navigationNodes.Add(node);
            return node;
        }
    }
}
