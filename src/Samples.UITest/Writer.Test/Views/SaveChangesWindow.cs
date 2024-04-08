using FlaUI.Core.AutomationElements;

namespace UITest.Writer.Views;

public record SaveChangesWindow(Window Element)
{
    public ListBox FilesToSaveList => Element.Find("FilesToSaveList").AsListBox();

    public Button NoButton => Element.Find("NoButton").AsButton();
}
