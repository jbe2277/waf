using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class SaveChangesWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public ListBox FilesToSaveList => this.Find("FilesToSaveList").AsListBox();

    public Button YesButton => this.Find("YesButton").AsButton();

    public Button NoButton => this.Find("NoButton").AsButton();
}
