using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public class RichTextView(AutomationElement Element)
{
    public TextBox RichTextBox => Element.Find("RichTextBox").AsTextBox();
}
