using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.Writer.Views;

public record ShellWindow(Window Element)
{
    public FileRibbonMenu FileRibbonMenu => new(Element.Find("FileRibbonMenu"));

    public ViewTab ViewTab => new(Element.Find("ViewTab").AsTabItem());

    public PrintPreviewTab PrintPreviewTab => new(Element.Find("PrintPreviewTab").AsTabItem());

    public Button AboutButton => Element.Find("AboutButton").AsButton();

    public StartView StartView => new(Element.Find("StartView"));

    public Tab DocumentTab => Element.Find("DocumentTabControl").AsTab();

    public IReadOnlyList<DocumentTabItem> DocumentTabItems => DocumentTab.FindAll(x => x.ByControlType(ControlType.TabItem)).Select(x => new DocumentTabItem(x.AsTabItem())).ToArray();

    public ComboBox ZoomComboBox => Element.Find("ZoomComboBox").AsComboBox();
}

public record FileRibbonMenu(AutomationElement Element)
{
    public ToggleButton MenuButton => Element.Find("PART_ToggleButton").AsToggleButton();

    public MenuItem NewMenuItem => Element.Find("NewMenuItem").AsMenuItem();

    public MenuItem PrintPreviewMenuItem => Element.Find("PrintPreviewMenuItem").AsMenuItem();

    public MenuItem ExitMenuItem => Element.Find("ExitMenuItem").AsMenuItem();
}

public record ViewTab(TabItem Element)
{
    public Button ZoomInButton => Element.Find("ZoomInButton").AsButton();

    public Button ZoomOutButton => Element.Find("ZoomOutButton").AsButton();
}

public record PrintPreviewTab(TabItem Element)
{
    public Button ClosePrintPreviewButton => Element.Find("ClosePrintPreviewButton").AsButton();

    public Button ZoomInButton => Element.Find("ZoomInButton").AsButton();

    public Button ZoomOutButton => Element.Find("ZoomOutButton").AsButton();
}

public record DocumentTabItem(TabItem Element)
{
    public string Name => Element.Find("TabName").Name;

    public Button CloseButton => Element.Find("CloseButton").AsButton();

    public RichTextView RichTextView => new(Element.Find("RichTextView"));
}