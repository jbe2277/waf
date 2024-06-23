using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.Writer.Views;

public class ShellWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public FileRibbonMenu FileRibbonMenu => this.Find("FileRibbonMenu").As<FileRibbonMenu>();

    public HomeTab HomeTab => this.Find("HomeTab").As<HomeTab>();

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

    public ListBox RecentFileList => this.Find("RecentFileList").AsListBox();

    public RecentFileMenuItem[] RecentFileListItems => RecentFileList.Items.Select(x => x.As<RecentFileMenuItem>()).ToArray();
}

public class RecentFileMenuItem(FrameworkAutomationElementBase element) : ListBoxItem(element)
{
    public string FileName => OpenFileButton.Name;

    public string ToolTip => OpenFileButton.HelpText;

    public Button OpenFileButton => this.Find("OpenItemButton").AsButton();

    public ToggleButton PinButton => this.Find("PinToggleButton").AsToggleButton();
}

public class HomeTab(FrameworkAutomationElementBase element) : TabItem(element)
{
    public ToggleButton ToggleBoldButton => this.Find("ToggleBoldButton").AsToggleButton();

    public ToggleButton ToggleItalicButton => this.Find("ToggleItalicButton").AsToggleButton();

    public ToggleButton ToggleUnderlineButton => this.Find("ToggleUnderlineButton").AsToggleButton();

    public ToggleButton ToggleNumberingButton => this.Find("ToggleNumberingButton").AsToggleButton();

    public ToggleButton ToggleBulletsButton => this.Find("ToggleBulletsButton").AsToggleButton();

    public Button DecreaseIndentationButton => this.Find("DecreaseIndentationButton").AsButton();

    public Button IncreaseIndentationButton => this.Find("IncreaseIndentationButton").AsButton();
}

public class ViewTab(FrameworkAutomationElementBase element) : TabItem(element)
{
    public Button ZoomInButton => this.Find("ZoomInButton").AsButton();

    public Button ZoomOutButton => this.Find("ZoomOutButton").AsButton();
}

public class PrintPreviewTab(FrameworkAutomationElementBase element) : TabItem(element)
{
    public Button ClosePrintPreviewButton => this.Find("ClosePrintPreviewButton").AsButton();

    public Button PrintButton => this.Find("PrintButton").AsButton();

    public Button ZoomInButton => this.Find("ZoomInButton").AsButton();

    public Button ZoomOutButton => this.Find("ZoomOutButton").AsButton();
}

public class DocumentTabItem(FrameworkAutomationElementBase element) : TabItem(element)
{
    public string TabName => this.Find("TabName").Name;

    public Button CloseButton => this.Find("CloseButton").AsButton();

    public RichTextView RichTextView => this.Find("RichTextView").As<RichTextView>();
}