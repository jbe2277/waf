using FlaUI.Core.AutomationElements;
using UITest.InformationManager.Views;
using UITest.SystemViews;
using Xunit;
using Xunit.Abstractions;

namespace UITest.InformationManager.Tests;

public class EmailTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void SearchEmailsAndAssertLists() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        Assert.True(window.RootTreeItem.InboxNode.IsSelected);

        var emailListView = window.EmailLayoutView.EmailListView;
        var emailView = window.EmailLayoutView.EmailView;
        var count = emailListView.EmailItems.Count;
        Assert.Equal(10, count);
        Log.WriteLine($"List of Contacts ({count})");
        for (int i = 0; i < count; i++) Log.WriteLine($"{i:00}: {emailListView.EmailItems[i].ToReceivedTuple()}");

        AssertEmail(true, emailListView.EmailItems[0], emailView, "user-2@fabrikam.com", "harry@example.com", "8/9/2012", "5:58:21 AM", "Nunc sed dis suscipit");
        Assert.Equal("Scelerisque est odio", emailView.Document.GetText(20));

        Assert.Equal("Search", emailListView.SearchBox.SearchHintLabel.Text);
        emailListView.SearchBox.SearchTextBox.Text = "!";
        Assert.Empty(emailListView.EmailItems);

        emailListView.SearchBox.SearchTextBox.Text = "someone";
        Assert.Equal(3, emailListView.EmailItems.Count);
        Assert.Null(emailListView.EmailList.SelectedItem);
        emailListView.EmailList.Select(1);
        var item = emailListView.EmailList.SelectedItem.As<EmailListItem>();
        AssertEmail(true, item, emailView, "someone-2@adventure-works.com", "harry@example.com", "9/5/2005", "4:34:45 PM", "Taciti enim");

        window.RootTreeItem.OutboxNode.Select();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Empty(emailListView.EmailItems);
        Assert.Empty(emailView.TitleLabel.Text);


        window.RootTreeItem.SentNode.Select();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Equal(5, emailListView.EmailItems.Count);
        AssertEmail(false, emailListView.EmailList.SelectedItem.As<EmailListItem>(), emailView, "harry@example.com", "user-2@fabrikam.com", "4/7/2012", "10:07:05 AM", "Massa sed");


        window.RootTreeItem.DraftsNode.Select();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Single(emailListView.EmailItems);
        AssertEmail(false, emailListView.EmailList.SelectedItem.As<EmailListItem>(), emailView, "harry@example.com", "", "7/1/2006", "3:40:49 AM", "Sociis nunc vivamus sagittis");


        window.RootTreeItem.DeletedNode.Select();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Empty(emailListView.EmailItems);
        Assert.Empty(emailView.TitleLabel.Text);

        window.ExitCommand.Click();
    });

    [Fact]
    public void RemoveEmailsTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        var emailListView = window.EmailLayoutView.EmailListView;
        var emailView = window.EmailLayoutView.EmailView;
        Assert.Equal(10, emailListView.EmailItems.Count);

        Assert.True(window.DeleteEmailCommand.IsEnabled);
        window.DeleteEmailCommand.Click();
        Assert.Equal(9, emailListView.EmailItems.Count);
        AssertEmail(true, emailListView.EmailItems[0], emailView, "user-2@fabrikam.com", "harry@example.com", "1/19/2010", "7:13:04 AM", "Egestas nisi mattis");

        window.RootTreeItem.SentNode.Select();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Equal(5, emailListView.EmailItems.Count);
        for (int i = 0; i < 5; i++) window.DeleteEmailCommand.Click();
        Assert.Empty(emailListView.EmailItems);

        window.ExitCommand.Click();

        
        // Restart application and assert that first email was deleted
        Launch(resetSettings: false, resetContainer: false);
        window = GetShellWindow();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Equal(9, emailListView.EmailItems.Count);
        AssertEmail(true, emailListView.EmailItems[0], emailView, "user-2@fabrikam.com", "harry@example.com", "1/19/2010", "7:13:04 AM", "Egestas nisi mattis");

        window.RootTreeItem.SentNode.Select();
        emailListView = window.EmailLayoutView.EmailListView;
        emailView = window.EmailLayoutView.EmailView;
        Assert.Empty(emailListView.EmailItems);
    });

    [Fact]
    public void NewEmailTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.RootTreeItem.SentNode.Select();
        var emailListView = window.EmailLayoutView.EmailListView;
        var emailView = window.EmailLayoutView.EmailView;
        Assert.Equal(5, emailListView.EmailItems.Count);

        window.NewEmailCommand.Click();
        var newEmailWindow = window.NewEmailWindows[0];
        newEmailWindow.SendButton.Click();

        // ItemStatus contains the validation error message or string.Empty if no error exists
        Assert.Equal("", newEmailWindow.ToTextBox.Text);
        Assert.Equal("This email doesn't define a recipient.", newEmailWindow.ToTextBox.ItemStatus);
        Assert.Equal("", newEmailWindow.CCTextBox.Text);
        Assert.Empty(newEmailWindow.CCTextBox.ItemStatus);

        newEmailWindow.SendButton.Click();
        var errorBox = newEmailWindow.FirstModalWindow().As<MessageBox>();
        Assert.Contains("One or more fields are not valid.", errorBox.Message);
        errorBox.Buttons[0].Click();

        newEmailWindow.ToTextBox.Text = "luke@example.com";
        newEmailWindow.CCTextBox.Click();
        Assert.Empty(newEmailWindow.ToTextBox.ItemStatus);

        newEmailWindow.CCSelectContactButton.Click();
        var contactWindow = newEmailWindow.FirstModalWindow().As<SelectContactWindow>();
        Assert.Equal(5, contactWindow.ContactListView.ContactItems.Count);
        Assert.True(contactWindow.ContactListView.ContactItems[0].IsSelected);
        contactWindow.ContactListView.SearchBox.SearchTextBox.Text = "Mi";
        Assert.Equal(2, contactWindow.ContactListView.ContactItems.Count);
        Assert.False(contactWindow.ContactListView.ContactItems[0].IsSelected);
        contactWindow.ContactListView.ContactList.Items[0].Select();
        contactWindow.OkButton.Click();
        Assert.Equal("michael.pfeiffer@fabrikam.com", newEmailWindow.CCTextBox.Text);

        newEmailWindow.BccSelectContactButton.Click();
        contactWindow = newEmailWindow.FirstModalWindow().As<SelectContactWindow>();
        contactWindow.CancelButton.Click();
        Assert.Empty(newEmailWindow.BccTextBox.Text);
        newEmailWindow.BccSelectContactButton.Click();
        contactWindow = newEmailWindow.FirstModalWindow().As<SelectContactWindow>();
        contactWindow.OkButton.Click();
        Assert.Equal("jesper.aaberg@example.com", newEmailWindow.BccTextBox.Text);

        newEmailWindow.TitleTextBox.Text = "ATitle";
        newEmailWindow.MessageTextBox.Text = "AMessage";
        newEmailWindow.SendButton.Click();

        Assert.Equal(6, emailListView.EmailItems.Count);
        emailListView.EmailItems[0].Select();
        AssertEmail(false, emailListView.EmailItems[0], emailView, "harry@example.com", "luke@example.com", DateTime.Now.ToShortDateString(), null, "ATitle");
        Assert.Equal("AMessage", emailView.Document.GetText(20));


        // Create email but do not send it -> Close
        window.NewEmailCommand.Click();
        newEmailWindow = window.NewEmailWindows[0];
        newEmailWindow.ToTextBox.Text = "luke@example.com";
        newEmailWindow.TitleTextBox.Text = "Don't send";
        newEmailWindow.CloseButton.Click();
        Assert.Equal(6, emailListView.EmailItems.Count);
    });

    [Fact]
    public void EmailAccountsTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.EmailAccountsCommand.Click();
        var emailAccountsWindow = window.FirstModalWindow().As<EmailAccountsWindow>();
        var row0 = emailAccountsWindow.EmailAccountsDataGrid.Rows[0].As<EmailAccountGridRow>();

        // TODO: Add test EmailAccountsTest()
        //       2. Select email account (just one in ComboBox)

    });
    
    private static void AssertEmail(bool received, EmailListItem? emailItem, EmailView? emailView, string from, string to, string sentDate, string? sentTime, string title)
    {
        if (emailItem is not null)
        {
            if (received) Assert.Equal((from, sentDate, title), emailItem.ToReceivedTuple());
            else Assert.Equal((to, sentDate, title), emailItem.ToSendTuple());
        }
        if (emailView is not null)
        {
            var expectedDate = sentTime is null ? "" : sentDate + " " + sentTime;
            var actualDate = sentTime is null ? "" : emailView.SentLabel.Text;
            Assert.Equal((from, to, expectedDate, title), (emailView.FromLabel.Text, emailView.ToLabel.Text, actualDate, emailView.TitleLabel.Text));        
            if (sentTime is null)
            {
                Assert.Contains(expectedDate, actualDate);
            }
        }
    }
}
