using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class StartView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Button NewButton => this.Find("NewButton").AsButton();

    public ListBox RecentFilesList => this.Find("RecentFilesList").AsListBox();

    public RecentFilesListItem[] RecentFilesListItems => RecentFilesList.Items.Select(x => x.As<RecentFilesListItem>()).ToArray();
}

public class RecentFilesListItem(FrameworkAutomationElementBase element) : ListBoxItem(element)
{
    public string FileName => OpenFile.Name;

    public Button OpenFile => this.Find("RecentItemOpenLink").AsButton();
}
