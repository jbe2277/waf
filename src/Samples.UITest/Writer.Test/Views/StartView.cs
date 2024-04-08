using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public record StartView(AutomationElement Element)
{
    public Button NewButton => Element.Find("NewButton").AsButton();
}
