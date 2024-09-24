using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Controls;

public class SearchBox(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Label SearchHintLabel => this.Find("SearchHintLabel").AsLabel();

    public TextBox SearchTextBox => this.Find("SearchTextBox").AsTextBox();
}
