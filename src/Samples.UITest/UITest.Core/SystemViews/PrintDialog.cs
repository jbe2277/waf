using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using System.Diagnostics;

namespace UITest.SystemViews;

public class PrintDialog(FrameworkAutomationElementBase element) : Window(element)
{
    public static PrintDialog? TryGetDialog(AutomationBase automation)
    {
        var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.Contains("PrintDialog", StringComparison.OrdinalIgnoreCase));
        if (p is null) return null;
        var desktop = automation.GetDesktop();
        var topLevel = desktop.FindAllChildren(x => x.ByControlType(ControlType.Window));
        AutomationElement? printDialog = null;
        foreach (var x in topLevel)
        {
            printDialog = x.FindFirstChild(x => x.ByProcessId(p.Id));  // It is a Window on the second level with process name "PrintDialog"
            if (printDialog is not null) break;
        }
        return printDialog?.As<PrintDialog>();
    }

    public ComboBox PrinterSelector => this.Find("printerSelector").AsComboBox();

    public ComboBoxItem PrintToPdf => PrinterSelector.Items.First(x => x.Name == "Microsoft Print to PDF");

    public ComboBox PageOrientationComboBox => this.Find("PageOrientation_ItemList").AsComboBox();

    public CheckBox UseAppFeaturesCheckBox => this.Find("UseAppFeaturesCheckBox").AsCheckBox();

    public Button PrintButton => this.Find("PrintButton").AsButton();

    public Button CloseButton => this.Find("CloseButton").AsButton();
}
