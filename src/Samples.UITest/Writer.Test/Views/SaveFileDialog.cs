using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class SaveFileDialog(FrameworkAutomationElementBase element) : Window(element)
{
    public ComboBox FileName => this.Find("FileNameControlHost").AsComboBox();

    public Button SaveButton => this.Find("1").AsButton();

    public Button CancelButton => this.Find("2").AsButton();
}
