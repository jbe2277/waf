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
        var count = contactListView.ContactItems.Count;
        Assert.Equal(5, count);
        Log.WriteLine($"List of Contacts ({count})");
        for (int i = 0; i < count; i++) Log.WriteLine($"{i:00}: {contactListView.ContactItems[i].ToTuple()}");
        
        AssertContactItem(contactListView.ContactItems[0], contactView, "Jesper", "Aaberg", "jesper.aaberg@example.com", "(111) 555-0100");
        AssertContactItem(contactListView.ContactItems[^1], null, "Miles", "Reid", "miles.reid@adventure-works.com", "(444) 555-0123");
        Assert.Equal(contactListView.ContactList.SelectedItem.As<ContactListItem>().ToTuple(), contactListView.ContactItems[0].ToTuple());
        Assert.Equal("Main St. 4567", contactView.StreetBox.Text);
        Assert.Equal("Buffalo", contactView.CityBox.Text);
        Assert.Equal("New York", contactView.StateBox.Text);
        Assert.Equal("98052", contactView.PostalCodeBox.Text);
        Assert.Equal("United States", contactView.CountryBox.Text);

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
        Assert.Equal((firstName, lastName, email, phone), contactItem.ToTuple());
        if (contactView is not null)
        {
            Assert.Equal(contactItem.ToTuple(), (contactView.FirstnameBox.Text, contactView.LastnameBox.Text, contactView.EmailBox.Text, contactView.PhoneBox.Text));
        }    
    }
}
