using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace UITest.Controls;

public class TextGridCell(FrameworkAutomationElementBase element) : GridCell(element)
{
    public Label Label => this.Find(x => x.ByControlType(ControlType.Text)).AsLabel();

    public TextBox TextBox => this.Find(x => x.ByControlType(ControlType.Edit)).AsTextBox();

    public string Text
    {
        get => Name;
        set
        {
            Label.DoubleClick();
            TextBox.Text = value;
            Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.ENTER);
        }
    }
}
