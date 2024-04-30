using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class StartView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Button NewButton => this.Find("NewButton").AsButton();

    public ListBox RecentFileList => this.Find("RecentFileList").AsListBox();

    public RecentFileListItem[] RecentFileListItems => RecentFileList.Items.Select(x => x.As<RecentFileListItem>()).ToArray();
}

public class RecentFileListItem(FrameworkAutomationElementBase element) : ListBoxItem(element)
{
    public string FileName => Label.Name;

    public string ToolTip => Label.HelpText;

    public Label Label => this.Find("RecentItemLabel").AsLabel();

    public Button OpenFileButton => this.Find("RecentItemOpenLink").AsButton();

    public ToggleButton PinButton => this.Find("PinToggleButton").AsToggleButton();

    public RecentFileListItemContextMenu ShowContextMenu() => OpenFileButton.ShowContextMenu().As<RecentFileListItemContextMenu>();
}

public class RecentFileListItemContextMenu(FrameworkAutomationElementBase element) : Menu(element)
{
    public MenuItem OpenFileMenuItem => this.Find("OpenFileMenuItem").AsMenuItem();
    
    public MenuItem PinFileMenuItem => this.Find("PinFileMenuItem").AsMenuItem();
    
    public MenuItem UnpinFileMenuItem => this.Find("UnpinFileMenuItem").AsMenuItem();

    public MenuItem RemoveFileMenuItem => this.Find("RemoveFileMenuItem").AsMenuItem();
}
