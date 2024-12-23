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

        window.ExitButton.Click();
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

        window.ExitButton.Click();

        
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
        Assert.Equal(("Harry Thompson", "harry@example.com"), row0.ToTuple());
        emailAccountsWindow.EditButton.Click();
        var editAccountWindow = emailAccountsWindow.FirstModalWindow().As<EditEmailAccountWindow>();
        
        var accountView = editAccountWindow.BasicEmailAccountView;
        Assert.Equal("Harry Thompson", accountView.NameTextBox.Text);
        Assert.Equal("harry@example.com", accountView.EmailTextBox.Text);
        Assert.False(accountView.Pop3RadioButton.IsChecked);
        Assert.True(accountView.ExchangeRadioButton.IsChecked);
        accountView.NameTextBox.Text = "Luke Thompson";
        accountView.EmailTextBox.Text = "luke@example.com";
        editAccountWindow.NextButton.Click();

        var exchangeSettingsView = editAccountWindow.ExchangeSettingsView;
        Assert.Equal("exchange.example.com", exchangeSettingsView.ServerPathTextBox.Text);
        Assert.Equal("harry", exchangeSettingsView.UserNameTextBox.Text);
        exchangeSettingsView.UserNameTextBox.Text = "luke";
        editAccountWindow.NextButton.Click();

        row0 = emailAccountsWindow.EmailAccountsDataGrid.Rows[0].As<EmailAccountGridRow>();
        Assert.Equal(("Luke Thompson", "luke@example.com"), row0.ToTuple());

        Assert.False(row0.IsSelected);
        Assert.False(emailAccountsWindow.EditButton.IsEnabled);
        Assert.False(emailAccountsWindow.RemoveButton.IsEnabled);
        row0.Select();

        // Edit again but Cancel this time
        emailAccountsWindow.EditButton.Click();
        editAccountWindow = emailAccountsWindow.FirstModalWindow().As<EditEmailAccountWindow>();
        accountView = editAccountWindow.BasicEmailAccountView;
        accountView.NameTextBox.Text = "Han Thompson";
        accountView.EmailTextBox.Text = "han@example.com";
        editAccountWindow.CancelButton.Click();
        row0 = emailAccountsWindow.EmailAccountsDataGrid.Rows[0].As<EmailAccountGridRow>();
        Assert.Equal(("Luke Thompson", "luke@example.com"), row0.ToTuple());

        // Add a second email account with POP3
        emailAccountsWindow.NewButton.Click();
        editAccountWindow = emailAccountsWindow.FirstModalWindow().As<EditEmailAccountWindow>();
        Assert.False(editAccountWindow.NextButton.IsEnabled);
        accountView = editAccountWindow.BasicEmailAccountView;
        Assert.Equal("The Name field is required.", accountView.NameTextBox.ItemStatus);
        Assert.Equal("The Email Address field is required.", accountView.EmailTextBox.ItemStatus);
        accountView.NameTextBox.Text = "Lea Thompson";
        Assert.Empty(accountView.NameTextBox.ItemStatus);
        accountView.EmailTextBox.Text = "lea@example.com";
        Assert.Empty(accountView.EmailTextBox.ItemStatus);
        editAccountWindow.NextButton.Click();

        var pop3SettingsView = editAccountWindow.Pop3SettingsView;
        Assert.Equal("The POP3 Server field is required.", pop3SettingsView.Pop3ServerPathTextBox.ItemStatus);
        Assert.Equal("The Username field is required.", pop3SettingsView.Pop3UserNameTextBox.ItemStatus);
        Assert.Equal("The SMTP Server field is required.", pop3SettingsView.SmtpServerPathTextBox.ItemStatus);
        Assert.Equal("The Username field is required.", pop3SettingsView.SmtpUserNameTextBox.ItemStatus);
        Assert.False(editAccountWindow.NextButton.IsEnabled);
        Assert.Equal("Please correct the invalid fields first.", editAccountWindow.NextButton.HelpText);
        
        pop3SettingsView.Pop3ServerPathTextBox.Text = "pop3.example.com";
        Assert.Empty(pop3SettingsView.Pop3ServerPathTextBox.ItemStatus);
        pop3SettingsView.Pop3UserNameTextBox.Text = "lea";
        Assert.Empty(pop3SettingsView.Pop3UserNameTextBox.ItemStatus);
        pop3SettingsView.Pop3PasswordBox.Text = "secret";
        pop3SettingsView.SmtpServerPathTextBox.Text = "smtp.example.com";
        Assert.Empty(pop3SettingsView.SmtpServerPathTextBox.ItemStatus);
        pop3SettingsView.UseSameUserCreditsCheckBox.IsChecked = true;        
        Assert.Empty(pop3SettingsView.SmtpUserNameTextBox.ItemStatus);
        Assert.True(editAccountWindow.NextButton.IsEnabled);
        editAccountWindow.NextButton.Click();

        var row1 = emailAccountsWindow.EmailAccountsDataGrid.Rows[1].As<EmailAccountGridRow>();
        Assert.Equal(("Lea Thompson", "lea@example.com"), row1.ToTuple());
        emailAccountsWindow.CloseButton.Click();

        // Create email and select new email account
        window.NewEmailCommand.Click();
        var newEmailWindow = window.NewEmailWindows[0];
        Assert.Equal(2, newEmailWindow.EmailAccountsComboBox.Items.Length);
        newEmailWindow.EmailAccountsComboBox.Select(1);
        newEmailWindow.EmailAccountsComboBox.Click();  // To close the combo box popup
        Assert.Equal("Lea Thompson", newEmailWindow.EmailAccountsComboBox.SelectedItem.Text);
        newEmailWindow.CloseButton.Click();

        // Email accounts -> remove all of them
        window.EmailAccountsCommand.Click();
        emailAccountsWindow = window.FirstModalWindow().As<EmailAccountsWindow>();
        Assert.Equal(2, emailAccountsWindow.EmailAccountsDataGrid.RowCount);
        emailAccountsWindow.RemoveButton.Click();
        emailAccountsWindow.EmailAccountsDataGrid.Rows[0].Select();
        emailAccountsWindow.RemoveButton.Click();
        Assert.Equal(0, emailAccountsWindow.EmailAccountsDataGrid.RowCount);
        emailAccountsWindow.CloseButton.Click();

        window.NewEmailCommand.Click();
        var errorMessage = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Please create an email account first.", errorMessage.Message);
        errorMessage.Buttons[0].Click();

        window.ExitButton.Click();
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
