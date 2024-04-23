using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace UITest.Writer.Views;

public class SaveFileDialog(FrameworkAutomationElementBase element) : Window(element)
{
    private ComboBox FileNameComboBox => this.Find("FileNameControlHost").AsComboBox();

    private TextBox FileNameTextBox => FileNameComboBox.Find(x => x.ByControlType(ControlType.Edit)).AsTextBox();

    public Button SaveButton => this.Find(x => x.ByControlType(ControlType.Button).And(x.ByAutomationId("1"))).AsButton();

    public Button CancelButton => this.Find(x => x.ByControlType(ControlType.Button).And(x.ByAutomationId("2"))).AsButton();

    public void SetFileName(string fileName)
    {
        // See https://stackoverflow.com/a/74182242
        FileNameTextBox.Click();        
        Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.KEY_A);
        Keyboard.Type(fileName);
    }
}
