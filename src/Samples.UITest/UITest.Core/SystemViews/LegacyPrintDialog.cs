using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.SystemViews;

public class LegacyPrintDialog(FrameworkAutomationElementBase element) : Window(element)
{
    public Button PrintButton => this.Find("1").AsButton();

    public Button CancelButton => this.Find("2").AsButton();
}
