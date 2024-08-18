using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.Infrastructure.Modules.Applications.Services;
using System.Waf.UnitTesting;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services;

[TestClass]
public class NavigationServiceTest
{
    [TestMethod]
    public void AddNavigationNodesWithWrongParameters()
    {
        var navigationService = new NavigationService();
        AssertHelper.ExpectedException<ArgumentNullException>(() => navigationService.AddNavigationNode(null!, null!, null!, null!, 0, 0));
        AssertHelper.ExpectedException<ArgumentException>(() => navigationService.AddNavigationNode("", null!, null!, null!, 0, 0));
        AssertHelper.ExpectedException<ArgumentNullException>(() => navigationService.AddNavigationNode("N1", null!, null!, null!, 0, 0));
        AssertHelper.ExpectedException<ArgumentException>(() => navigationService.AddNavigationNode("N1", "", null!, null!, 0, 0));
        AssertHelper.ExpectedException<ArgumentNullException>(() => navigationService.AddNavigationNode("N1", "Node 1", null!, null!, 0, 0));
        AssertHelper.ExpectedException<ArgumentNullException>(() => navigationService.AddNavigationNode("N1", "Node 1", () => { }, null!, 0, 0));
        AssertHelper.ExpectedException<ArgumentOutOfRangeException>(() => navigationService.AddNavigationNode("N1", "Node 1", () => { }, () => { }, -1, -1));
        AssertHelper.ExpectedException<ArgumentOutOfRangeException>(() => navigationService.AddNavigationNode("N1", "Node 1", () => { }, () => { }, 0, -1));
    }

    [TestMethod]
    public void AddNavigationNode()
    {
        var navigationService = new NavigationService();

        bool showActionCalled = false;
        void ShowAction() => showActionCalled = true;
        bool closeActionCalled = false;
        void CloseAction() => closeActionCalled = true;

        var node = (NavigationNode)navigationService.AddNavigationNode("N1", "Node 1", ShowAction, CloseAction, 3, 7);

        Assert.AreEqual("Node 1", node.Name);
        Assert.AreEqual(3, node.Group);
        Assert.AreEqual(7, node.Order);

        Assert.IsNull(node.ItemCount);
        AssertHelper.PropertyChangedEvent(node, x => x.ItemCount, () => node.ItemCount = 5);
        Assert.AreEqual(5, node.ItemCount);
        AssertHelper.ExpectedException<AssertException>(() => AssertHelper.PropertyChangedEvent(node, x => x.ItemCount, () => node.ItemCount = 5));

        Assert.IsFalse(node.IsSelected);
        Assert.IsFalse(showActionCalled);
        Assert.IsFalse(closeActionCalled);

        node.IsSelected = true;
        Assert.IsTrue(showActionCalled);
        Assert.IsFalse(closeActionCalled);

        showActionCalled = false;
        node.IsSelected = false;
        Assert.IsFalse(showActionCalled);
        Assert.IsTrue(closeActionCalled);

        Assert.IsFalse(node.IsFirstItemOfNewGroup);
        AssertHelper.PropertyChangedEvent(node, x => x.IsFirstItemOfNewGroup, () => node.IsFirstItemOfNewGroup = true);
        Assert.IsTrue(node.IsFirstItemOfNewGroup);
    }

    [TestMethod]
    public void AddNavigationNodes()
    {
        var navigationService = new NavigationService();

        static void ShowAction() { }
        static void CloseAction() { }

        var nodeB2 = navigationService.AddNavigationNode("B2", "Node B2", ShowAction, CloseAction, 1, 1);
        var nodeA1 = navigationService.AddNavigationNode("A1", "Node A1", ShowAction, CloseAction, 0, 0);
        var nodeA2 = navigationService.AddNavigationNode("A2", "Node A2", ShowAction, CloseAction, 0, 1);
        var nodeB1 = navigationService.AddNavigationNode("B1", "Node B1", ShowAction, CloseAction, 1, 0);

        AssertHelper.SequenceEqual(new[] { nodeA1, nodeA2, nodeB1, nodeB2 }, navigationService.NavigationNodes);
        Assert.IsFalse(((NavigationNode)nodeA1).IsFirstItemOfNewGroup);
        Assert.IsFalse(((NavigationNode)nodeA2).IsFirstItemOfNewGroup);
        Assert.IsTrue(((NavigationNode)nodeB1).IsFirstItemOfNewGroup);
        Assert.IsFalse(((NavigationNode)nodeB2).IsFirstItemOfNewGroup);
    }
}
