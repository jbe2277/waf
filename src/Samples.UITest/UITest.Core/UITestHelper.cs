using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using System.Text;

namespace UITest;

public class ElementNotFoundException(string message, Exception? innerException = null) : Exception(message, innerException) { }

public static class UITestHelper
{
    public static AutomationElement[] FindAll(this AutomationElement element, string automationId) => element.FindAllDescendants(x => x.ByAutomationId(automationId));

    public static AutomationElement[] FindAll(this AutomationElement element, Func<ConditionFactory, ConditionBase> conditionFunc) => element.FindAllDescendants(conditionFunc);

    public static AutomationElement? TryFind(this AutomationElement element, string automationId) => element.FindFirstDescendant(automationId);

    public static AutomationElement? TryFind(this AutomationElement element, Func<ConditionFactory, ConditionBase> conditionFunc) => element.FindFirstDescendant(conditionFunc);

    public static AutomationElement Find(this AutomationElement element, string automationId, TimeSpan? timeout = null)
    {
        var result = Retry.WhileNull(() => element.TryFind(automationId), timeout);
        return result.Result ?? throw new ElementNotFoundException($"Element '{automationId}' was not found as descendant of element '{element.TryAutomationId()}'"
                + Environment.NewLine + Environment.NewLine + element.GetTree());
    }

    public static AutomationElement Find(this AutomationElement element, Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan? timeout = null)
    {
        var result = Retry.WhileNull(() => element.TryFind(conditionFunc), timeout);
        return result.Result ?? throw new ElementNotFoundException($"Element was not found as descendant of element '{element.TryAutomationId()}'"
                + Environment.NewLine + Environment.NewLine + element.GetTree());
    }

    public static Window GetWindow(this AutomationElement element)
    {
        var e = element;
        while (true)
        {
            if (e.ControlType == ControlType.Window) return e.AsWindow();
            e = e.Parent;
            if (e is null) throw new ElementNotFoundException($"The owner window was not found of the element '{element.TryAutomationId()}'");
        }
    }

    public static Menu ShowContextMenu(this AutomationElement element)
    {
        element.RightClick();
        return element.GetWindow().ContextMenu.AsMenu();
    }

    public static Window FirstModalWindow(this Window window, TimeSpan? timeout = null)
    {
        var result = Retry.WhileEmpty(() => window.ModalWindows, timeout);
        return result?.Result?.FirstOrDefault() ?? throw new ElementNotFoundException($"First modal dialog was not found for window '{window.TryAutomationId()}'");
    }

    public static string GetTree(this AutomationElement element)
    {
        var sb = new StringBuilder();
        GetTreeCore(sb, element, "");
        return sb.ToString();

        static void GetTreeCore(StringBuilder sb, AutomationElement element, string padding)
        {
            var automationId = element.TryAutomationId();
            if (!string.IsNullOrEmpty(automationId))
            {
                sb.AppendLine($"{padding}{automationId}: {element.ControlType}");
            }

            foreach (var x in element.FindAllChildren()) GetTreeCore(sb, x, padding + "  ");
        }
    }

    private static string? TryAutomationId(this AutomationElement element) 
    { 
        try { return element.Properties.AutomationId.ValueOrDefault; } 
        catch { } return null; 
    }
}
