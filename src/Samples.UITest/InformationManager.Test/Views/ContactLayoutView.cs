using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class ContactLayoutView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public ContactListView ContactListView => this.Find("ContactListView").As<ContactListView>();

    public ContactView ContactView => this.Find("ContactView").As<ContactView>();
}
