using FlaUI.Core.AutomationElements;
using FlaUI.Core;
using UITest.InformationManager.Controls;

namespace UITest.InformationManager.Views;

public class ContactListView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public SearchBox SearchBox => this.Find("SearchBox").As<SearchBox>();

    public ListBox ContactList => this.Find("ContactList").AsListBox();

    public IReadOnlyList<ContactListItem> ContactItems => ContactList.Items.Select(x => x.As<ContactListItem>()).ToArray();
}

public class ContactListItem(FrameworkAutomationElementBase element) : ListBoxItem(element)
{
    public Label FirstnameLabel => this.Find("FirstnameLabel").AsLabel();

    public Label LastnameLabel => this.Find("LastnameLabel").AsLabel();

    public Label EmailLabel => this.Find("EmailLabel").AsLabel();

    public Label PhoneLabel => this.Find("PhoneLabel").AsLabel();

    public void Deconstruct(out string firstname, out string lastname, out string email, out string phone) => (firstname, lastname, email, phone) = ToTuple();

    public (string firstname, string lastname, string email, string phone) ToTuple() => (FirstnameLabel.Text, LastnameLabel.Text, EmailLabel.Text, PhoneLabel.Text);
}