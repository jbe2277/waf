using FlaUI.Core.AutomationElements;
using UITest.BookLibrary.Views;
using Xunit.Abstractions;
using Xunit;
using UITest.SystemViews;

namespace UITest.BookLibrary.Tests;

public class AddressBookTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void SearchPersonListAndChangeEntriesTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.TabControl.AddressBookTabItem.Select();
        var personListView = window.TabControl.AddressBookTabItem.PersonListView;
        var personView = window.TabControl.AddressBookTabItem.PersonView;

        var rowCount = personListView.PersonDataGrid.RowCount;
        Assert.Equal(4, rowCount);
        Log.WriteLine($"List of Persons ({rowCount}):");
        for (int i = 0; i < rowCount; i++)
        {
            Log.WriteLine($"{i:00}: {personListView.PersonDataGrid.GetRowByIndex(i).As<PersonGridRow>().FirstnameCell.Label.Text}");
        }

        personListView.SearchBox.Text = "H";
        Assert.Equal(2, personListView.PersonDataGrid.RowCount);
        personListView.SearchBox.Text = "Ha";
        Assert.Equal(1, personListView.PersonDataGrid.RowCount);
        var personRow1 = personListView.PersonDataGrid.GetRowByIndex(0).As<PersonGridRow>();
        personRow1.Select();

        AssertEqual("Harry", personRow1.FirstnameCell.Name, personView.FirstnameTextBox.Text);
        AssertEqual("Potter", personRow1.LastnameCell.Name, personView.LastnameTextBox.Text);
        AssertEqual("harry.potter@hogwarts.edu", personRow1.EmailCell.Label.Text, personView.EmailTextBox.Text);

        personView.FirstnameTextBox.Text = "TFirstname";
        Assert.Equal("TFirstname", personRow1.FirstnameCell.Name);
        personView.LastnameTextBox.Text = "TLastname";
        Assert.Equal("TLastname", personRow1.LastnameCell.Name);
        personView.EmailTextBox.Text = "TEmail@mail.com";
        Assert.Equal("TEmail@mail.com", personRow1.EmailCell.Label.Text);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to save the changes
        messageBox.Buttons[1].Click();  // No button
    });

    [Fact]
    public void AddAndRemoveEntriesTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.TabControl.AddressBookTabItem.Select();
        var personListView = window.TabControl.AddressBookTabItem.PersonListView;
        var personView = window.TabControl.AddressBookTabItem.PersonView;

        Assert.Equal(4, personListView.PersonDataGrid.RowCount);
        personListView.AddButton.Click();
        Assert.Equal(5, personListView.PersonDataGrid.RowCount);
        var newRow = personListView.PersonDataGrid.SelectedItem.As<PersonGridRow>();
        Assert.Equal(personListView.PersonDataGrid.Rows[0], newRow);

        // ItemStatus contains the validation error message or string.Empty if no error exists
        AssertEqual("", personView.FirstnameTextBox.Text, newRow.FirstnameCell.Label.Text);
        AssertEqual("Firstname is mandatory.", personView.FirstnameTextBox.ItemStatus, newRow.FirstnameCell.Label.ItemStatus);
        AssertEqual("", personView.LastnameTextBox.Text, newRow.LastnameCell.Label.Text);
        AssertEqual("Lastname is mandatory.", personView.LastnameTextBox.ItemStatus, newRow.LastnameCell.Label.ItemStatus);
        AssertEqual("", personView.EmailTextBox.ItemStatus, newRow.EmailCell.Label.Text);

        personView.FirstnameTextBox.Text = "AFirstname";
        Assert.Equal("AFirstname", newRow.FirstnameCell.Name);
        AssertEqual("", personView.FirstnameTextBox.ItemStatus, newRow.FirstnameCell.Label.ItemStatus);
        personView.LastnameTextBox.Text = "ALastname";
        Assert.Equal("ALastname", newRow.LastnameCell.Name);
        AssertEqual("", personView.LastnameTextBox.ItemStatus, newRow.LastnameCell.Label.ItemStatus);

        var lastRow = personListView.PersonDataGrid.GetRowByIndex(personListView.PersonDataGrid.RowCount - 1).As<PersonGridRow>();
        Assert.False(lastRow.IsOffscreen);
        Assert.StartsWith("Ron", lastRow.FirstnameCell.Name);

        var lastNotRemovedRow = personListView.PersonDataGrid.GetRowByIndex(personListView.PersonDataGrid.RowCount - 3);
        personListView.PersonDataGrid.Select(personListView.PersonDataGrid.RowCount - 2);
        personListView.PersonDataGrid.AddToSelection(personListView.PersonDataGrid.RowCount - 1);
        personListView.RemoveButton.Click();
        Assert.Equal(3, personListView.PersonDataGrid.RowCount);
        Assert.Equal(lastNotRemovedRow, personListView.PersonDataGrid.SelectedItem);
        Assert.Equal(lastNotRemovedRow, personListView.PersonDataGrid.Rows[^1]);

        window.DataMenu.Click();
        window.DataMenu.SaveMenuItem.Click();
        window.Close();


        Launch(resetSettings: false, resetDatabase: false);
        window = GetShellWindow();
        window.TabControl.AddressBookTabItem.Select();
        personListView = window.TabControl.AddressBookTabItem.PersonListView;
        Assert.Equal(3, personListView.PersonDataGrid.RowCount);

        personListView.PersonDataGrid.Select(0);
        for (int i = 1; i < personListView.PersonDataGrid.RowCount; i++) personListView.PersonDataGrid.AddToSelection(i);
        Assert.Equal(3, personListView.PersonDataGrid.SelectedItems.Length);
        Assert.True(personListView.RemoveButton.IsEnabled);
        personListView.RemoveButton.Click();
        Assert.False(personListView.RemoveButton.IsEnabled);
        Assert.Null(personListView.PersonDataGrid.SelectedItem);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to save the changes
        messageBox.Buttons[1].Click();  // No button
    });

    [Fact]
    public void ValidateFieldsTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.TabControl.AddressBookTabItem.Select();
        var personListView = window.TabControl.AddressBookTabItem.PersonListView;
        var personView = window.TabControl.AddressBookTabItem.PersonView;
        var row1 = personListView.PersonDataGrid.SelectedItem.As<PersonGridRow>();

        var text31 = new string('a', 31);
        var text101 = new string('a', 101);

        // Note: "Required" validation of Title and Author are covered by the AddAndRemoveEntriesTest test
        // ItemStatus contains the validation error message or string.Empty if no error exists
        personView.FirstnameTextBox.Text = text31;
        AssertEqual("Firstname can contain 30 characters at maximum.", personView.FirstnameTextBox.ItemStatus, row1.FirstnameCell.Label.ItemStatus);

        personView.LastnameTextBox.Text = text31;
        AssertEqual("Lastname can contain 30 characters at maximum.", personView.LastnameTextBox.ItemStatus, row1.LastnameCell.Label.ItemStatus);

        personView.EmailTextBox.Text = text101;
        Assert.Equal("Email can contain 100 characters at maximum.\r\nThe provided email address is invalid.", personView.EmailTextBox.ItemStatus);
        personView.EmailTextBox.Text = "test";
        Assert.Equal("The provided email address is invalid.", personView.EmailTextBox.ItemStatus);
        personView.EmailTextBox.Text = "test@mail.com";
        Assert.Empty(personView.EmailTextBox.ItemStatus);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to really close the app although unsafed changes exist
        messageBox.Buttons[0].Click();  // Yes button
    });

    private static void AssertEqual(string expected, string actual1, string actual2)
    {
        Assert.Equal(expected, actual1);
        Assert.Equal(expected, actual2);
    }
}
