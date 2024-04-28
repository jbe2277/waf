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
}
