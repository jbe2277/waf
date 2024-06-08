using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.BookLibrary.Controls;

public class TextGridCell(FrameworkAutomationElementBase element) : GridCell(element)
{
    public Label Label => this.Find(x => x.ByControlType(ControlType.Text)).AsLabel();
}
