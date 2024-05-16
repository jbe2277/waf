using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using UITest.BookLibrary.Views;
using UITest.SystemViews;
using Xunit;
using Xunit.Abstractions;

namespace UITest.BookLibrary.Tests;

public class BookLibraryTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void AboutTest()
    {
        Launch();
        var window = GetShellWindow();

        var helpMenu = window.HelpMenu;
        helpMenu.Click();
        helpMenu.AboutMenuItem.Click();

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Waf Book Library", messageBox.Title);
        Log.WriteLine(messageBox.Message);
        Assert.StartsWith("Waf Book Library ", messageBox.Message);
        Capture.Screen().ToFile(GetScreenshotFile("About.png"));
        messageBox.Buttons[0].Click();

        var dataMenu = window.DataMenu;
        dataMenu.Click();
        dataMenu.ExitMenuItem.Click();
    }

    [Fact]
    public void SearchBookListAndChangeEntriesTest()
    {
        Launch();
        var window = GetShellWindow();
        var bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        var bookView = window.TabControl.BookLibraryTabItem.BookView;
        
        Assert.Equal(41, bookListView.BookDataGrid.RowCount);
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
        Assert.True(lendToWindow.WasReturnedRadioButton.IsChecked);
        Assert.False(lendToWindow.LendToRadioButton.IsChecked);
        Assert.False(lendToWindow.PersonListBox.IsEnabled);
        lendToWindow.LendToRadioButton.Click();
        Assert.True(lendToWindow.PersonListBox.IsEnabled);
        Assert.Equal(["Ginny", "Hermione", "Harry", "Ron"], lendToWindow.PersonListBox.Items.Select(x => x.Text));
        lendToWindow.PersonListBox.Items[2].Select();
        lendToWindow.OkButton.Click();
        AssertEqual("Harry Potter", bookRow2.LendToCell.LendToLabel.Name, bookView.LendToTextBox.Text);

        window.Close();
        var messageBox = window.FirstModalWindow().As<MessageBox>();  // MessageBox that asks user to save the changes
        messageBox.Buttons[1].Click();  // No button
        
        void AssertEqual(string expected, string actual1, string actual2)
        {
            Assert.Equal(expected, actual1);
            Assert.Equal(expected, actual2);
        }
    }
}
