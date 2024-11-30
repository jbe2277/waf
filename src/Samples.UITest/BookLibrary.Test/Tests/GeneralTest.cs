using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using UITest.SystemViews;
using Xunit;
using Xunit.Abstractions;

namespace UITest.BookLibrary.Tests;

public class GeneralTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void AboutTest() => Run(() =>
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
        Capture.Screen().ToFile(GetScreenshotFile("About"));
        messageBox.Buttons[0].Click();

        var dataMenu = window.DataMenu;
        dataMenu.Click();
        dataMenu.ExitMenuItem.Click();
    });

    [Fact]
    public void LoadCorruptDatabaseTest() => Run(() =>
    {
        if (File.Exists(AppInfo.DatabaseFile)) File.Delete(AppInfo.DatabaseFile);
        Directory.CreateDirectory(Path.GetDirectoryName(AppInfo.DatabaseFile)!);
        File.AppendAllText(AppInfo.DatabaseFile, "42");

        Launch(resetDatabase: false);
        var window = GetShellWindow();
        var bookListView = window.TabControl.BookLibraryTabItem.BookListView;
        Assert.Equal(0, bookListView.BookDataGrid.RowCount);

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Could not load the Books from the database.", messageBox.Message);
        messageBox.Buttons[0].Click();

        messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Could not load the Persons from the database.", messageBox.Message);
        messageBox.Buttons[0].Click();

        Assert.Equal(0, bookListView.BookDataGrid.RowCount);

        window.TabControl.AddressBookTabItem.Select();
        var personListView = window.TabControl.AddressBookTabItem.PersonListView;
        Assert.Equal(0, personListView.PersonDataGrid.RowCount);

        window.Close();
    });
}
