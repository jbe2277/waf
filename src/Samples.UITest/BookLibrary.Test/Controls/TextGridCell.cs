using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.BookLibrary.Controls;

public class TextGridCell(FrameworkAutomationElementBase element) : GridCell(element)
{
    public Label Label => this.Find(x => x.ByControlType(FlaUI.Core.Definitions.ControlType.Text)).AsLabel();
}
