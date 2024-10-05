using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.InformationManager.Views;

public class ShellWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public Button NewEmailCommand => this.Find("NewEmailCommand").AsButton();

    public Button DeleteEmailCommand => this.Find("DeleteEmailCommand").AsButton();

    public Button EmailAccountsCommand => this.Find("EmailAccountsCommand").AsButton();

    public Button NewContactCommand => this.Find("NewContactCommand").AsButton();

    public Button DeleteCommand => this.Find("DeleteCommand").AsButton();

    public Button AboutCommand => this.Find("AboutCommand").AsButton();

    public Button ExitCommand => this.Find("ExitCommand").AsButton();


    public NavigationRootTreeItem RootTreeItem => this.Find("RootTreeItem").As<NavigationRootTreeItem>();

    public ContactLayoutView ContactLayoutView => this.Find("ContactLayoutView").As<ContactLayoutView>();

    
    public WindowVisualState WindowState
    {
        get => Patterns.Window.Pattern.WindowVisualState;
        set => Patterns.Window.Pattern.SetWindowVisualState(value);
    }
}

public class NavigationRootTreeItem(FrameworkAutomationElementBase element) : TreeItem(element)
{
    public TreeItem ContactsNode => this.Find("ContactsNode").AsTreeItem();
}