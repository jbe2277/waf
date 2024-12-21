using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.InformationManager.Views;

public class SelectContactWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public ContactListView ContactListView => this.Find("ContactListView").As<ContactListView>();

    public Button OkButton => this.Find("OkButton").AsButton();

    public Button CancelButton => this.Find("CancelButton").AsButton();
}
