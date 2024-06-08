using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.BookLibrary.Views;

public class ShellWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public DataMenu DataMenu => this.Find("DataMenu").As<DataMenu>();

    public HelpMenu HelpMenu => this.Find("HelpMenu").As<HelpMenu>();

    public TabControl TabControl => this.Find("TabControl").As<TabControl>();

    public void SetState(WindowVisualState state) => Patterns.Window.Pattern.SetWindowVisualState(state);
}

public class DataMenu(FrameworkAutomationElementBase element) : Menu(element)
{
    public MenuItem SaveMenuItem => this.Find("SaveMenuItem").AsMenuItem();

    public MenuItem ExitMenuItem => this.Find("ExitMenuItem").AsMenuItem();
}

public class HelpMenu(FrameworkAutomationElementBase element) : Menu(element)
{
    public MenuItem AboutMenuItem => this.Find("AboutMenuItem").AsMenuItem();
}

public class TabControl(FrameworkAutomationElementBase element) : Tab(element)
{
    public BookLibraryTabItem BookLibraryTabItem => this.Find("BookLibraryTabItem").As<BookLibraryTabItem>();

    public AddressBookTabItem AddressBookTabItem => this.Find("AddressBookTabItem").As<AddressBookTabItem>();

    public ReportTabItem ReportingTabItem => this.Find("ReportingTabItem").As<ReportTabItem>();
}

public class BookLibraryTabItem(FrameworkAutomationElementBase element) : TabItem(element)
{
    public BookListView BookListView => this.Find("BookListView").As<BookListView>();

    public BookView BookView => this.Find("BookView").As<BookView>();
}

public class AddressBookTabItem(FrameworkAutomationElementBase element) : TabItem(element)
{
    public PersonListView PersonListView => this.Find("PersonListView").As<PersonListView>();

    public PersonView PersonView => this.Find("PersonView").As<PersonView>();
}

public class ReportTabItem(FrameworkAutomationElementBase element) : TabItem(element)
{
    public ReportView ReportView => this.Find("ReportView").As<ReportView>();
}