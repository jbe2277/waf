using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.Writer.Views;

public record MessageBox(Window Element)
{
    public string Title => Element.Name;

    public string Message => Element.Find(x => x.ByControlType(ControlType.Text)).Name;

    public Button OkButton => Element.Find(x => x.ByControlType(ControlType.Button)).AsButton();
}
