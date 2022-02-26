using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    public class MockNavigationNode : Model, INavigationNode
    {
        public MockNavigationNode(string name, Action showAction, Action closeAction, double group, double order)
        {
            Name = name;
            ShowAction = showAction;
            CloseAction = closeAction;
            Group = group;
            Order = order;
        }
        
        public string Name { get; }

        public Action ShowAction { get; }

        public Action CloseAction { get; }

        public double Group { get; }

        public double Order { get; }

        public int? ItemCount { get; set; }
    }
}
