using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class EmailLayoutView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public EmailListView EmailListView => this.Find("EmailListView").As<EmailListView>();

    public EmailView EmailView => this.Find("EmailView").As<EmailView>();
}
