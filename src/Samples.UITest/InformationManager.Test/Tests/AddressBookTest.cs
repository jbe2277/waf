using FlaUI.Core.AutomationElements;
using Xunit.Abstractions;
using Xunit;
using UITest.InformationManager.Views;

namespace UITest.InformationManager.Tests;

public class AddressBookTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void ShowContactsTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.RootTreeItem.ContactsNode.Click();

        var contactListView = window.ContactLayoutView.ContactListView;
        var contactView = window.ContactLayoutView.ContactView;
        Assert.Equal(5, contactListView.ContactItems.Count);
        Assert.Equal(contactListView.ContactList.SelectedItem.As<ContactListItem>().FirstnameLabel.Text, contactListView.ContactItems[0].FirstnameLabel.Text);
        AssertContactItem(contactListView.ContactItems[0], contactView, "Jesper", "Aaberg", "jesper.aaberg@example.com", "(111) 555-0100");
        AssertContactItem(contactListView.ContactItems[^1], null, "Miles", "Reid", "miles.reid@adventure-works.com", "(444) 555-0123");
        
        Assert.Equal("Search", contactListView.SearchBox.SearchHintLabel.Text);
        contactListView.SearchBox.SearchTextBox.Text = "!";
        Assert.Empty(contactListView.ContactItems);

        contactListView.SearchBox.SearchTextBox.Text = "Mi";
        Assert.Equal(2, contactListView.ContactItems.Count);
        Assert.Null(contactListView.ContactList.SelectedItem);
        contactListView.ContactList.Select(0);
        AssertContactItem(contactListView.ContactList.SelectedItem.As<ContactListItem>(), contactView, "Michael", "Pfeiffer", "michael.pfeiffer@fabrikam.com", "(222) 555-0105");

        window.ExitCommand.Click();
    });

    private static void AssertContactItem(ContactListItem contactItem, ContactView? contactView, string firstName, string lastName, string email, string phone)
    {
        Assert.Equal(firstName, contactItem.FirstnameLabel.Text);        
        Assert.Equal(lastName, contactItem.LastnameLabel.Text);
        Assert.Equal(email, contactItem.EmailLabel.Text);
        Assert.Equal(phone, contactItem.PhoneLabel.Text);

        if (contactView is not null)
        {
            Assert.Equal(firstName, contactView.FirstnameBox.Text);
            Assert.Equal(lastName, contactView.LastnameBox.Text);
            Assert.Equal(email, contactView.EmailBox.Text);
            Assert.Equal(phone, contactView.PhoneBox.Text);
        }    
    }
}
