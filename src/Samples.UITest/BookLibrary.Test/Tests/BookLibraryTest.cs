using FlaUI.Core.AutomationElements;
using UITest.BookLibrary.Views;
using UITest.SystemViews;
using Xunit;
using Xunit.Abstractions;

namespace UITest.BookLibrary.Tests;

public class BookLibraryTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void SearchBookListAndChangeEntriesTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        var bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        var bookView = window.TabControl.BookLibraryTabItem.BookView;

        var rowCount = bookListView.BookDataGrid.RowCount;
        Assert.Equal(41, rowCount);
        Log.WriteLine($"List of Books ({rowCount}):");
        for (int i = 0; i < rowCount; i++)
        {
            Log.WriteLine($"{i:00}: {bookListView.BookDataGrid.GetRowByIndex(i).As<BookGridRow>().TitleCell.Label.Text}");
        }
        var firstBook = bookListView.BookDataGrid.GetRowByIndex(0).As<BookGridRow>();
        var lastBook = bookListView.BookDataGrid.GetRowByIndex(rowCount - 1).As<BookGridRow>();
        // GetRowByIndex scrolls to the item -> let's scroll back to the first book
        firstBook.ScrollIntoView();
        Assert.False(firstBook.IsOffscreen);

        bookListView.SearchBox.Text = "Ha";
        Assert.Equal(13, bookListView.BookDataGrid.RowCount);
        bookListView.SearchBox.Text = "Harr";
        Assert.Equal(7, bookListView.BookDataGrid.RowCount);
        var bookRow1 = bookListView.BookDataGrid.GetRowByIndex(0).As<BookGridRow>();
        bookRow1.Select();

        AssertEqual("Harry Potter and the Deathly Hallows", bookRow1.TitleCell.Name, bookView.TitleTextBox.Text);
        AssertEqual("J.K. Rowling", bookRow1.AuthorCell.Name, bookView.AuthorTextBox.Text);
        Assert.Equal("Bloomsbury", bookView.PublisherTextBox.Text);
        Assert.Equal("1/1/2007", bookRow1.PublishDateCell.Name);
        Assert.Equal(new DateTime(2007, 1, 1), bookView.PublishDatePicker.SelectedDate);
        Assert.Equal("9780747591054", bookView.IsbnTextBox.Text);
        Assert.Equal("English", bookView.LanguageComboBox.SelectedItem.Text);
        Assert.Equal("607", bookView.PagesTextBox.Text);
        AssertEqual("Ginny Weasley", bookRow1.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        bookRow1.TitleCell.Text = "Test Title";
        Assert.Equal("Test Title", bookView.TitleTextBox.Text);
        bookView.AuthorTextBox.Text = "TAuthor";
        Assert.Equal("TAuthor", bookRow1.AuthorCell.Name);
        bookView.PublishDatePicker.SelectedDate = new DateTime(2024, 3, 2);
        Assert.Equal("3/2/2024", bookRow1.PublishDateCell.Name);
        Assert.Equal(["Undefined", "English", "German", "French", "Spanish", "Chinese", "Japanese"], bookView.LanguageComboBox.Items.Select(x => x.Name));
        bookView.LanguageComboBox.Select(2);
        bookView.LanguageComboBox.Click();  // To close the combo box popup
        Assert.Equal("German", bookView.LanguageComboBox.SelectedItem.Text);

        bookView.LendToButton.Click();
        var lendToWindow = window.FirstModalWindow().As<LendToWindow>();
        Assert.False(lendToWindow.WasReturnedRadioButton.IsChecked);
        Assert.True(lendToWindow.LendToRadioButton.IsChecked);
        Assert.True(lendToWindow.PersonListBox.IsEnabled);
        Assert.Equal("Ginny", lendToWindow.PersonListBox.SelectedItem.Text);
        lendToWindow.WasReturnedRadioButton.Click();
        Assert.False(lendToWindow.PersonListBox.IsEnabled);
        lendToWindow.OkButton.Click();
        AssertEqual("", bookRow1.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        bookRow1.LendToCell.LendToButton.Click();
        lendToWindow = window.FirstModalWindow().As<LendToWindow>();
        Assert.True(lendToWindow.WasReturnedRadioButton.IsChecked);
        Assert.False(lendToWindow.LendToRadioButton.IsChecked);
        Assert.False(lendToWindow.PersonListBox.IsEnabled);
        Assert.Null(lendToWindow.PersonListBox.SelectedItem);
        lendToWindow.LendToRadioButton.Click();
        Assert.True(lendToWindow.PersonListBox.IsEnabled);
        Assert.Equal(["Ginny", "Hermione", "Harry", "Ron"], lendToWindow.PersonListBox.Items.Select(x => x.Text));
        lendToWindow.PersonListBox.Items[2].Select();
        lendToWindow.OkButton.Click();
        AssertEqual("Harry Potter", bookRow1.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to save the changes
        messageBox.Buttons[1].Click();  // No button
    });

    [Fact]
    public void AddAndRemoveEntriesTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        var bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        var bookView = window.TabControl.BookLibraryTabItem.BookView;

        Assert.Equal(41, bookListView.BookDataGrid.RowCount);
        bookListView.AddButton.Click();
        Assert.Equal(42, bookListView.BookDataGrid.RowCount);
        var newRow = bookListView.BookDataGrid.SelectedItem.As<BookGridRow>();
        Assert.Equal(bookListView.BookDataGrid.Rows[^1], newRow);

        // ItemStatus contains the validation error message or string.Empty if no error exists
        AssertEqual("", bookView.TitleTextBox.Text, newRow.TitleCell.Label.Text);
        AssertEqual("Title is mandatory.", bookView.TitleTextBox.ItemStatus, newRow.TitleCell.Label.ItemStatus);
        AssertEqual("", bookView.AuthorTextBox.Text, newRow.AuthorCell.Label.Text);
        AssertEqual("Author is mandatory.", bookView.AuthorTextBox.ItemStatus, newRow.AuthorCell.Label.ItemStatus);
        Assert.Equal("", bookView.PublisherTextBox.ItemStatus);

        bookView.TitleTextBox.Text = "ATitle";
        Assert.Equal("ATitle", newRow.TitleCell.Name);
        AssertEqual("", bookView.TitleTextBox.ItemStatus, newRow.TitleCell.Label.ItemStatus);
        newRow.AuthorCell.Text = "TAuthor";
        Assert.Equal("TAuthor", bookView.AuthorTextBox.Text);
        AssertEqual("", bookView.AuthorTextBox.ItemStatus, newRow.AuthorCell.Label.ItemStatus);

        var secondLastRow = bookListView.BookDataGrid.GetRowByIndex(bookListView.BookDataGrid.RowCount - 2).As<BookGridRow>();
        Assert.False(secondLastRow.IsOffscreen);
        Assert.StartsWith("WPF", secondLastRow.TitleCell.Name);

        var lastNotRemovedRow = bookListView.BookDataGrid.GetRowByIndex(bookListView.BookDataGrid.RowCount - 3);
        bookListView.BookDataGrid.Select(bookListView.BookDataGrid.RowCount - 2);
        bookListView.BookDataGrid.AddToSelection(bookListView.BookDataGrid.RowCount - 1);
        bookListView.RemoveButton.Click();
        Assert.Equal(40, bookListView.BookDataGrid.RowCount);
        Assert.Equal(lastNotRemovedRow, bookListView.BookDataGrid.SelectedItem);
        Assert.Equal(lastNotRemovedRow, bookListView.BookDataGrid.Rows[^1]);

        window.DataMenu.Click();
        window.DataMenu.SaveMenuItem.Click();
        window.Close();


        Launch(resetSettings: false, resetDatabase: false);
        window = GetShellWindow();
        bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        Assert.Equal(40, bookListView.BookDataGrid.RowCount);

        bookListView.BookDataGrid.Select(0);
        for (int i = 1; i < bookListView.BookDataGrid.RowCount; i++) bookListView.BookDataGrid.AddToSelection(i);
        Assert.Equal(40, bookListView.BookDataGrid.SelectedItems.Length);
        Assert.True(bookListView.RemoveButton.IsEnabled);
        bookListView.RemoveButton.Click();
        Assert.False(bookListView.RemoveButton.IsEnabled);
        Assert.Null(bookListView.BookDataGrid.SelectedItem);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to save the changes
        messageBox.Buttons[1].Click();  // No button
    });

    [Fact]
    public void ValidateFieldsTest() => Run(() =>
    {        
        Launch();
        var window = GetShellWindow();
        var bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        var bookView = window.TabControl.BookLibraryTabItem.BookView;
        var row1 = bookListView.BookDataGrid.SelectedItem.As<BookGridRow>();

        var text101 = new string('a', 101);

        // Note: "Required" validation of Title and Author are covered by the AddAndRemoveEntriesTest test
        // ItemStatus contains the validation error message or string.Empty if no error exists
        bookView.TitleTextBox.Text = text101;
        AssertEqual("Title can contain 100 characters at maximum.", bookView.TitleTextBox.ItemStatus, row1.TitleCell.Label.ItemStatus);
        
        row1.AuthorCell.Text = text101;
        AssertEqual("Author can contain 100 characters at maximum.", bookView.AuthorTextBox.ItemStatus, row1.AuthorCell.Label.ItemStatus);
        
        bookView.PublisherTextBox.Text = text101;
        Assert.Equal("Publisher can contain 100 characters at maximum.", bookView.PublisherTextBox.ItemStatus);
        
        bookView.PublishDatePicker.SelectedDate = new DateTime(1752, 12, 31);
        Assert.Equal("Value for 12/31/1752 12:00:00 AM must be between 1/1/1753 12:00:00 AM and 12/31/9999 12:00:00 AM.", bookView.PublishDatePicker.ItemStatus);
        bookView.PublishDatePicker.SelectedDate = new DateTime(2025, 1, 1);
        Assert.Empty(bookView.PublishDatePicker.ItemStatus);
        bookView.PublishDatePicker.SelectedDate = new DateTime(9999, 12, 31, 23, 59, 59);
        Assert.Equal("Value for 12/31/9999 11:59:59 PM must be between 1/1/1753 12:00:00 AM and 12/31/9999 12:00:00 AM.", bookView.PublishDatePicker.ItemStatus);

        bookView.IsbnTextBox.Text = new string('a', 15);
        Assert.Equal("Isbn can contain 14 characters at maximum.", bookView.IsbnTextBox.ItemStatus);

        bookView.PagesTextBox.Text = "-1";
        Assert.Equal("Pages must be equal or larger than 0.", bookView.PagesTextBox.ItemStatus);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to really close the app although unsafed changes exist
        messageBox.Buttons[0].Click();  // Yes button
    });

    [Fact]
    public void RemoveLendToPersonTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        var bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        var bookView = window.TabControl.BookLibraryTabItem.BookView;

        bookListView.SearchBox.Text = "Harr";
        var bookRow1 = bookListView.BookDataGrid.GetRowByIndex(0).As<BookGridRow>();
        bookRow1.Select();

        AssertEqual("Ginny Weasley", bookRow1.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        window.TabControl.AddressBookTabItem.Select();
        var personListView = window.TabControl.AddressBookTabItem.PersonListView;
        personListView.SearchBox.Text = "Ginny";
        var personRow = personListView.PersonDataGrid.GetRowByIndex(0).As<PersonGridRow>();
        personRow.Select();
        personListView.RemoveButton.Click();

        window.TabControl.BookLibraryTabItem.Select();
        AssertEqual("", bookRow1.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        window.DataMenu.Click();
        window.DataMenu.SaveMenuItem.Click();
        window.Close();


        Launch(resetSettings: false, resetDatabase: false);
        window = GetShellWindow();
        bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        bookView = window.TabControl.BookLibraryTabItem.BookView;

        bookListView.SearchBox.Text = "Harr";
        bookRow1 = bookListView.BookDataGrid.GetRowByIndex(1).As<BookGridRow>();
        bookRow1.Select();

        window.TabControl.BookLibraryTabItem.Select();
        AssertEqual("", bookRow1.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        window.Close();
    });

    private static void AssertEqual(string expected, string actual1, string actual2)
    {
        Assert.Equal(expected, actual1);
        Assert.Equal(expected, actual2);
    }
}
