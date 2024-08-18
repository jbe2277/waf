using Xunit.Abstractions;
using Xunit;

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
        Assert.Equal(5, contactListView.ContactList.Items.Length);

        window.ExitCommand.Click();
    });
}
