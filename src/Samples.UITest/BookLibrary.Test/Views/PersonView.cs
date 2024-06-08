using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.BookLibrary.Views;

public class PersonView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox FirstnameTextBox => this.Find("FirstnameTextBox").AsTextBox();

    public TextBox LastnameTextBox => this.Find("LastnameTextBox").AsTextBox();

    public TextBox EmailTextBox => this.Find("EmailTextBox").AsTextBox();

    public Button CreateNewEmailButton => this.Find("CreateNewEmailButton").AsButton();
}
