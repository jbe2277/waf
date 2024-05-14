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
        messageBox.OkButton.Click();

        var dataMenu = window.DataMenu;
        dataMenu.Click();
        dataMenu.ExitMenuItem.Click();
    }

    [Fact]
    public void SearchBookListTest()
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

        window.Close();

        void AssertEqual(string expected, string actual1, string actual2)
        {
            Assert.Equal(expected, actual1);
            Assert.Equal(expected, actual2);
        }
    }
}
