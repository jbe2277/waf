using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class ContactListView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public ListBox ContactList => this.Find("ContactList").AsListBox();
}
