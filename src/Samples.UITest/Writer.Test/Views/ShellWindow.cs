using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.Writer.Views;

public class ShellWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public FileRibbonMenu FileRibbonMenu => this.Find("FileRibbonMenu").As<FileRibbonMenu>();

    public ViewTab ViewTab => this.Find("ViewTab").As<ViewTab>();

    public PrintPreviewTab PrintPreviewTab => this.Find("PrintPreviewTab").As<PrintPreviewTab>();

    public Button AboutButton => this.Find("AboutButton").AsButton();

    public StartView StartView => this.Find("StartView").As<StartView>();

    public Tab DocumentTab => this.Find("DocumentTabControl").AsTab();

    public IReadOnlyList<DocumentTabItem> DocumentTabItems => DocumentTab.FindAll(x => x.ByControlType(ControlType.TabItem)).Select(x => x.As<DocumentTabItem>()).ToArray();

    public ComboBox ZoomComboBox => this.Find("ZoomComboBox").AsComboBox();

    public void SetState(WindowVisualState state) => Patterns.Window.Pattern.SetWindowVisualState(state);
}

public class FileRibbonMenu(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public ToggleButton MenuButton => this.Find("PART_ToggleButton").AsToggleButton();

    public MenuItem NewMenuItem => this.Find("NewMenuItem").AsMenuItem();

    public MenuItem PrintPreviewMenuItem => this.Find("PrintPreviewMenuItem").AsMenuItem();

    public MenuItem ExitMenuItem => this.Find("ExitMenuItem").AsMenuItem();
}

public class ViewTab(FrameworkAutomationElementBase element) : TabItem(element)
{
    public Button ZoomInButton => this.Find("ZoomInButton").AsButton();

    public Button ZoomOutButton => this.Find("ZoomOutButton").AsButton();
}

public class PrintPreviewTab(FrameworkAutomationElementBase element) : TabItem(element)
{
    public Button ClosePrintPreviewButton => this.Find("ClosePrintPreviewButton").AsButton();

    public Button ZoomInButton => this.Find("ZoomInButton").AsButton();

    public Button ZoomOutButton => this.Find("ZoomOutButton").AsButton();
}

public class DocumentTabItem(FrameworkAutomationElementBase element) : TabItem(element)
{
    public string TabName => this.Find("TabName").Name;

    public Button CloseButton => this.Find("CloseButton").AsButton();

    public RichTextView RichTextView => this.Find("RichTextView").As<RichTextView>();
}