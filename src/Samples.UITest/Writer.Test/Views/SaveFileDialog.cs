using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.Writer.Views;

public class SaveFileDialog(FrameworkAutomationElementBase element) : Window(element)
{
    public ComboBox FileNameBox => this.Find("FileNameControlHost").AsComboBox();

    public Button SaveButton => this.Find(x => x.ByControlType(ControlType.Button).And(x.ByAutomationId("1"))).AsButton();

    public Button CancelButton => this.Find(x => x.ByControlType(ControlType.Button).And(x.ByAutomationId("2"))).AsButton();
}
