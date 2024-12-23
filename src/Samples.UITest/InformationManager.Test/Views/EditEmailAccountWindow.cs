using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.InformationManager.Views;

public class EditEmailAccountWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public BasicEmailAccountView BasicEmailAccountView => this.Find("BasicEmailAccountView").As<BasicEmailAccountView>();

    public ExchangeSettingsView ExchangeSettingsView => this.Find("ExchangeSettingsView").As<ExchangeSettingsView>();

    public Pop3SettingsView Pop3SettingsView => this.Find("Pop3SettingsView").As<Pop3SettingsView>();

    public Button BackButton => this.Find("BackButton").AsButton();

    public Button NextButton => this.Find("NextButton").AsButton();

    public Button CancelButton => this.Find("CancelButton").AsButton();
}
