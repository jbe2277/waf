using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.Writer.Views;

public class MessageBox(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public string Title => Name;

    public string Message => this.Find(x => x.ByControlType(ControlType.Text)).Name;

    public Button OkButton => this.Find(x => x.ByControlType(ControlType.Button)).AsButton();
}
