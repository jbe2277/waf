using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.InformationManager.Views;

// TODO: Consider to rename ..Command to ..Button
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

    public EmailLayoutView EmailLayoutView => this.Find("EmailLayoutView").As<EmailLayoutView>();

    public ContactLayoutView ContactLayoutView => this.Find("ContactLayoutView").As<ContactLayoutView>();


    public IReadOnlyList<NewEmailWindow> NewEmailWindows => this.FindAll("NewEmailWindow").Select(x => x.As<NewEmailWindow>()).ToArray();
    
    public WindowVisualState WindowState
    {
        get => Patterns.Window.Pattern.WindowVisualState;
        set => Patterns.Window.Pattern.SetWindowVisualState(value);
    }
}

public class NavigationRootTreeItem(FrameworkAutomationElementBase element) : TreeItem(element)
{
    public TreeItem InboxNode => this.Find("InboxNode").AsTreeItem();

    public TreeItem OutboxNode => this.Find("OutboxNode").AsTreeItem();

    public TreeItem SentNode => this.Find("SentNode").AsTreeItem();

    public TreeItem DraftsNode => this.Find("DraftsNode").AsTreeItem();

    public TreeItem DeletedNode => this.Find("DeletedNode").AsTreeItem();

    public TreeItem ContactsNode => this.Find("ContactsNode").AsTreeItem();
}