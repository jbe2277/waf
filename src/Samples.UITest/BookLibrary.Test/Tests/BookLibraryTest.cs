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
        
        Assert.Equal(41, bookListView.BookDataGrid.RowCount);
        bookListView.SearchBox.Text = "Ha";
        Assert.Equal(13, bookListView.BookDataGrid.RowCount);
        bookListView.SearchBox.Text = "Harr";
        Assert.Equal(7, bookListView.BookDataGrid.RowCount);
        var bookRow2 = bookListView.BookDataGrid.GetRowByIndex(1).As<BookGridRow>();
        bookRow2.Select();

        Assert.Equal("Harry Potter and the Deathly Hallows", bookRow2.TitleCell.Name);
        Assert.Equal("J.K. Rowling", bookRow2.AuthorCell.Name);
        Assert.Equal("1/1/2007", bookRow2.PublishDateCell.Name);
        Assert.Equal("Ginny Weasley", bookRow2.LendToCell.LendToLabel.Name);

        window.Close();
    }
}
