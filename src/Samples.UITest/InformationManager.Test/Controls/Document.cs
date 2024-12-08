using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.InformationManager.Controls;

public class Document(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public string GetText(int maxLenght) => Patterns.Text.Pattern.DocumentRange.GetText(maxLenght);
}
