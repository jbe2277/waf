using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class RichTextView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox RichTextBox => this.Find("RichTextBox").AsTextBox();
}
