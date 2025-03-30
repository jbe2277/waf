using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

public class MockNavigationService : INavigationService
{
    private readonly List<MockNavigationNode> navigationNodes = [];

    public IReadOnlyList<MockNavigationNode> NavigationNodes => navigationNodes;

    public INavigationNode AddNavigationNode(string automationId, string name, Action showAction, Action closeAction, double group, double order)
    {
        var node = new MockNavigationNode(automationId, name, showAction, closeAction, group, order);
        navigationNodes.Add(node);
        return node;
    }
}
