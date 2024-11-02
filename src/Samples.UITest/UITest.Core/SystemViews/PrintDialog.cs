using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;

namespace UITest.SystemViews;

public class PrintDialog(FrameworkAutomationElementBase element) : Window(element)
{
    public static PrintDialog GetDialog(AutomationBase automation)
    {
        var desktop = automation.GetDesktop();
        return desktop.Find(x => new AndCondition(x.ByControlType(ControlType.Window), x.ByName("Windows Print"))).As<PrintDialog>();
    }

    public ComboBox PrinterSelector => this.Find("printerSelector").AsComboBox();

    public ComboBoxItem PrintToPdf => PrinterSelector.Items.First(x => x.Name == "Microsoft Print to PDF");

    public ComboBox PageOrientationComboBox => this.Find("PageOrientation_ItemList").AsComboBox();

    public CheckBox UseAppFeaturesCheckBox => this.Find("UseAppFeaturesCheckBox").AsCheckBox();

    public Button PrintButton => this.Find("PrintButton").AsButton();

    public Button CloseButton => this.Find("CloseButton").AsButton();
}
