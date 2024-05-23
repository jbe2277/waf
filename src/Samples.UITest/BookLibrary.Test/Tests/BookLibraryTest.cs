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
        
        bookListView.SearchBox.Text = "Ha";
        Assert.Equal(13, bookListView.BookDataGrid.RowCount);
        bookListView.SearchBox.Text = "Harr";
        Assert.Equal(7, bookListView.BookDataGrid.RowCount);
        var bookRow2 = bookListView.BookDataGrid.GetRowByIndex(1).As<BookGridRow>();
        bookRow2.Select();

        AssertEqual("Harry Potter and the Deathly Hallows", bookRow2.TitleCell.Name, bookView.TitleTextBox.Text);
        AssertEqual("J.K. Rowling", bookRow2.AuthorCell.Name, bookView.AuthorTextBox.Text);
        Assert.Equal("Bloomsbury", bookView.PublisherTextBox.Text);
        Assert.Equal("1/1/2007", bookRow2.PublishDateCell.Name);
        Assert.Equal(new DateTime(2007, 1, 1), bookView.PublishDatePicker.SelectedDate);
        Assert.Equal("9780747591054", bookView.IsbnTextBox.Text);
        Assert.Equal("English", bookView.LanguageComboBox.SelectedItem.Text);
        Assert.Equal("607", bookView.PagesTextBox.Text);
        AssertEqual("Ginny Weasley", bookRow2.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        bookView.TitleTextBox.Text = "Test Title";
        Assert.Equal("Test Title", bookRow2.TitleCell.Name);
        bookView.AuthorTextBox.Text = "TAuthor";
        Assert.Equal("TAuthor", bookRow2.AuthorCell.Name);
        bookView.PublishDatePicker.SelectedDate = new DateTime(2024, 3, 2);
        Assert.Equal("3/2/2024", bookRow2.PublishDateCell.Name);
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
        AssertEqual("", bookRow2.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        bookRow2.LendToCell.LendToButton.Click();
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
        AssertEqual("Harry Potter", bookRow2.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

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
        Assert.Equal(bookListView.BookDataGrid.Rows[0], newRow);

        // ItemStatus contains the validation error message or string.Empty if no error exists
        AssertEqual("", bookView.TitleTextBox.Text, newRow.TitleCell.Label.Text);
        AssertEqual("Title is mandatory.", bookView.TitleTextBox.ItemStatus, newRow.TitleCell.Label.ItemStatus);
        AssertEqual("", bookView.AuthorTextBox.Text, newRow.AuthorCell.Label.Text);
        AssertEqual("Author is mandatory.", bookView.AuthorTextBox.ItemStatus, newRow.AuthorCell.Label.ItemStatus);
        Assert.Equal("", bookView.PublisherTextBox.ItemStatus);

        bookView.TitleTextBox.Text = "TTitle";
        Assert.Equal("TTitle", newRow.TitleCell.Name);
        AssertEqual("", bookView.TitleTextBox.ItemStatus, newRow.TitleCell.Label.ItemStatus);
        bookView.AuthorTextBox.Text = "TAuthor";
        Assert.Equal("TAuthor", newRow.AuthorCell.Name);
        AssertEqual("", bookView.AuthorTextBox.ItemStatus, newRow.AuthorCell.Label.ItemStatus);

        var lastRow = bookListView.BookDataGrid.GetRowByIndex(bookListView.BookDataGrid.RowCount - 1).As<BookGridRow>();
        Assert.False(lastRow.IsOffscreen);
        Assert.StartsWith("WPF", lastRow.TitleCell.Name);

        bookListView.BookDataGrid.Select(bookListView.BookDataGrid.RowCount - 2);
        bookListView.BookDataGrid.AddToSelection(bookListView.BookDataGrid.RowCount - 1);
        bookListView.RemoveButton.Click();
        Assert.Equal(40, bookListView.BookDataGrid.RowCount);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to save the changes
        messageBox.Buttons[1].Click();  // No button
    });

    private static void AssertEqual(string expected, string actual1, string actual2)
    {
        Assert.Equal(expected, actual1);
        Assert.Equal(expected, actual2);
    }
}
