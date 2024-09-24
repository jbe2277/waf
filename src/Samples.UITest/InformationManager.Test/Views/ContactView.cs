using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class ContactView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox FirstnameBox => this.Find("FirstnameBox").AsTextBox();

    public TextBox LastnameBox => this.Find("LastnameBox").AsTextBox();

    public TextBox CompanyBox => this.Find("CompanyBox").AsTextBox();

    public TextBox EmailBox => this.Find("EmailBox").AsTextBox();

    public TextBox PhoneBox => this.Find("PhoneBox").AsTextBox();

    public TextBox StreetBox => this.Find("StreetBox").AsTextBox();

    public TextBox CityBox => this.Find("CityBox").AsTextBox();

    public TextBox StateBox => this.Find("StateBox").AsTextBox();

    public TextBox PostalCodeBox => this.Find("PostalCodeBox").AsTextBox();

    public TextBox CountryBox => this.Find("CountryBox").AsTextBox();
}
