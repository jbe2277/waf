using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.BookLibrary.Views;

public class ShellWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public DataMenu DataMenu => this.Find("DataMenu").As<DataMenu>();

    public HelpMenu HelpMenu => this.Find("HelpMenu").As<HelpMenu>();
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