using System.Waf.Foundation;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

public class MockNavigationNode(string name, Action showAction, Action closeAction, double group, double order) : Model, INavigationNode
{
    public string Name { get; } = name;

    public Action ShowAction { get; } = showAction;

    public Action CloseAction { get; } = closeAction;

    public double Group { get; } = group;

    public double Order { get; } = order;

    public int? ItemCount { get; set; }
}
