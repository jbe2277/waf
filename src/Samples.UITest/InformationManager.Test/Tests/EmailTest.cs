using FlaUI.Core.AutomationElements;
using UITest.InformationManager.Views;
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

    private static void AssertEmail(bool received, EmailListItem? emailItem, EmailView? emailView, string from, string to, string sentDate, string sentTime, string title)
    {
        if (emailItem is not null)
        {
            if (received) Assert.Equal((from, sentDate, title), emailItem.ToReceivedTuple());
            else Assert.Equal((to, sentDate, title), emailItem.ToSendTuple());
        }
        if (emailView is not null)
        {
            Assert.Equal((from, to, sentDate + " " + sentTime, title), (emailView.FromLabel.Text, emailView.ToLabel.Text, emailView.SentLabel.Text, emailView.TitleLabel.Text));        }
    }
}
