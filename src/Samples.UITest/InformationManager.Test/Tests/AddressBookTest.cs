using FlaUI.Core.AutomationElements;
using Xunit;
using UITest.InformationManager.Views;
using FlaUI.Core.Definitions;

namespace UITest.InformationManager.Tests;

public class AddressBookTest() : UITest()
{
    [Fact]
    public void SearchContactsAndChangeEntriesTest()
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
        Assert.Equal(contactListView.ContactList.SelectedItem!.As<ContactListItem>().ToTuple(), contactListView.ContactItems[0].ToTuple());
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
        var item = contactListView.ContactList.SelectedItem!.As<ContactListItem>();
        AssertContactItem(item, contactView, "Michael", "Pfeiffer", "michael.pfeiffer@fabrikam.com", "(222) 555-0105");
        contactView.FirstnameBox.Text = "TFirstname";
        Assert.Equal("TFirstname", item.FirstnameLabel.Text);
        contactView.PhoneBox.Text = "TPhone";
        Assert.Equal("TPhone", item.PhoneLabel.Text);

        window.ExitButton.Click();
    }

    [Fact]
    public void AddAndRemoveEntriesTest()
    {
        Launch();
        var window = GetShellWindow();
        window.WindowState = WindowVisualState.Maximized;
        window.RootTreeItem.ContactsNode.Click();
        var contactListView = window.ContactLayoutView.ContactListView;
        var contactView = window.ContactLayoutView.ContactView;
        
        Assert.Equal(5, contactListView.ContactItems.Count);
        window.NewContactCommand.Click();
        Assert.Equal(6, contactListView.ContactItems.Count);
        var newItem = contactListView.ContactList.SelectedItem!.As<ContactListItem>();

        // ItemStatus contains the validation error message or string.Empty if no error exists
        AssertContactItem(null, contactView, "", "", "", "");
        AssertContactItem(newItem, null, "(none)", "(none)", "(none)", "(none)");
        Assert.Equal("The Firstname field is required.", contactView.FirstnameBox.ItemStatus);

        contactView.FirstnameBox.Text = "AFirstname";
        Assert.Equal("AFirstname", newItem.FirstnameLabel.Text);
        Assert.Equal("", contactView.FirstnameBox.ItemStatus);
        contactView.LastnameBox.Text = "ALastname";
        contactView.EmailBox.Text = "AEmail@mail.com";
        contactView.PhoneBox.Text = "1234";
        AssertContactItem(newItem, contactView, "AFirstname", "ALastname", "AEmail@mail.com", "1234");

        var secondItem = contactListView.ContactItems[1];
        contactListView.ContactList.Select(0);
        window.DeleteCommand.Click();
        Assert.Equal(5, contactListView.ContactItems.Count);
        Assert.Equal(secondItem.FirstnameLabel.Text, contactListView.ContactList.SelectedItem!.As<ContactListItem>().FirstnameLabel.Text);

        window.ExitButton.Click();

        // Restart application and assert that new contact was saved
        Launch(resetSettings: false, resetContainer: false);
        window = GetShellWindow();
        Assert.Equal(WindowVisualState.Maximized, window.WindowState);
        window.RootTreeItem.ContactsNode.Click();
        contactListView = window.ContactLayoutView.ContactListView;
        contactView = window.ContactLayoutView.ContactView;
        Assert.Equal(5, contactListView.ContactItems.Count);
        newItem = contactListView.ContactItems[^1];
        Assert.Equal("AFirstname", newItem.FirstnameLabel.Text);
        contactListView.ContactList.Select(contactListView.ContactItems.Count - 1);
        AssertContactItem(newItem, contactView, "AFirstname", "ALastname", "AEmail@mail.com", "1234");

        // Invalid Firstname > Save > Restart > Validation error should be here again
        contactView.FirstnameBox.Text = "";
        window.ExitButton.Click();

        Launch(resetSettings: false, resetContainer: false);
        window = GetShellWindow();
        Assert.Equal(WindowVisualState.Maximized, window.WindowState);
        window.RootTreeItem.ContactsNode.Click();
        contactListView = window.ContactLayoutView.ContactListView;
        contactView = window.ContactLayoutView.ContactView;
        contactListView.ContactList.Select(contactListView.ContactItems.Count - 1);
        Assert.Equal("The Firstname field is required.", contactView.FirstnameBox.ItemStatus);

        // Remove all contact items
        var count = contactListView.ContactItems.Count;
        for (int i = 0; i < count; i++) window.DeleteCommand.Click();
        Assert.False(window.DeleteCommand.IsEnabled);
        Assert.Null(contactListView.ContactList.SelectedItem);
        window.Close();
    }

    private static void AssertContactItem(ContactListItem? contactItem, ContactView? contactView, string firstName, string lastName, string email, string phone)
    {
        if (contactItem is not null)
        {
            Assert.Equal((firstName, lastName, email, phone), contactItem.ToTuple());
        }        
        if (contactView is not null)
        {
            Assert.Equal((firstName, lastName, email, phone), (contactView.FirstnameBox.Text, contactView.LastnameBox.Text, contactView.EmailBox.Text, contactView.PhoneBox.Text));
        }    
    }
}
