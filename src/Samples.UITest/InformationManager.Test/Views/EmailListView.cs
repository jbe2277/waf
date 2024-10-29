using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using UITest.InformationManager.Controls;

namespace UITest.InformationManager.Views;

public class EmailListView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public SearchBox SearchBox => this.Find("SearchBox").As<SearchBox>();

    public ListBox EmailList => this.Find("EmailList").AsListBox();

    public IReadOnlyList<EmailListItem> EmailItems => EmailList.Items.Select(x => x.As<EmailListItem>()).ToArray();
}

public class EmailListItem(FrameworkAutomationElementBase element) : ListBoxItem(element)
{
    public Label ToLabel => this.Find("ToLabel").AsLabel();

    public Label FromLabel => this.Find("FromLabel").AsLabel();

    public Label SentLabel => this.Find("SentLabel").AsLabel();

    public Label TitleLabel => this.Find("TitleLabel").AsLabel();

    public (string to, string sent, string title) ToReceivedTuple() => (FromLabel.Text, SentLabel.Text, TitleLabel.Text);

    public (string to, string sent, string title) ToSendTuple() => (ToLabel.Text, SentLabel.Text, TitleLabel.Text);
}