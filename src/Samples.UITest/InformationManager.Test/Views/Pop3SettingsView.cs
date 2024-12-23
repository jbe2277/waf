using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Views;

public class Pop3SettingsView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox Pop3ServerPathTextBox => this.Find("Pop3ServerPathTextBox").AsTextBox();

    public TextBox Pop3UserNameTextBox => this.Find("Pop3UserNameTextBox").AsTextBox();

    public TextBox Pop3PasswordBox => this.Find("Pop3PasswordBox").AsTextBox();

    public TextBox SmtpServerPathTextBox => this.Find("SmtpServerPathTextBox").AsTextBox();

    public TextBox SmtpUserNameTextBox => this.Find("SmtpUserNameTextBox").AsTextBox();

    public TextBox SmtpPasswordBox => this.Find("SmtpPasswordBox").AsTextBox();

    public CheckBox UseSameUserCreditsCheckBox => this.Find("UseSameUserCreditsCheckBox").AsCheckBox();
}
