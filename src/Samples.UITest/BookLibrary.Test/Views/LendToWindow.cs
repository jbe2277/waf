using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.BookLibrary.Views;

public class LendToWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public RadioButton WasReturnedRadioButton => this.Find("WasReturnedRadioButton").AsRadioButton();

    public RadioButton LendToRadioButton => this.Find("LendToRadioButton").AsRadioButton();

    public ListBox PersonListBox => this.Find("PersonListBox").AsListBox();

    public Button OkButton => this.Find("OkButton").AsButton();

    public Button CancelButton => this.Find("CancelButton").AsButton();
}
