using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class StartView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Button NewButton => this.Find("NewButton").AsButton();
}
