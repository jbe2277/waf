using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using UITest.SystemViews;
using UITest.Writer.Views;
using Xunit;
using Xunit.Abstractions;

namespace UITest.Writer;

public class WriterTest : UITest
{
    public WriterTest(ITestOutputHelper log) : base(log)
    {
        // SkipTextBoxReadText = true;  // Workaround: Uncomment this line if your machine provides empty strings for RichTextBox.Text.
    }

    /// <summary>Workaround: On some machines reading the RichTextBox.Text returns always an empty string. Use this property to skip the step.</summary>
    public bool SkipTextBoxReadText { get; set; }

    protected void AssertTextEqual(string expected, TextBox actual) { if (!SkipTextBoxReadText) Assert.Equal(expected, actual.Text); }

    [Fact]
    public void AboutTest() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();

        window.AboutButton.Click();

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Waf Writer", messageBox.Title);
        Log.WriteLine(messageBox.Message);
        Assert.StartsWith("Waf Writer ", messageBox.Message);
        Capture.Screen().ToFile(GetScreenshotFile("About"));
        messageBox.Buttons[0].Click();

        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();
    });

    [Fact]
    public void NewZoomWritePrintPreviewExitWithoutSave() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();
        window.SetState(WindowVisualState.Maximized);

        var viewTab = window.ViewTab;
        viewTab.Select();
        var zoomInButton = viewTab.ZoomInButton;
        Assert.False(zoomInButton.IsEnabled);

        var startView = window.StartView;
        Assert.False(startView.IsOffscreen);
        startView.NewButton.Click();

        Retry.WhileTrue(() => startView.IsOffscreen, throwOnTimeout: true);
        Assert.True(zoomInButton.IsEnabled);
        zoomInButton.Click();
        var zoomComboBox = window.ZoomComboBox;
        Assert.Equal("110%", zoomComboBox.EditableText);

        var homeTab = window.HomeTab;
        homeTab.Select();
        var richTextView = window.DocumentTabItems.Single().RichTextView;
        var range = richTextView.RichTextBox.Patterns.Text.Pattern.DocumentRange;
        var nl = Environment.NewLine;
        richTextView.RichTextBox.Enter($"Title{nl}");
        richTextView.RichTextBox.Enter($"Heading 1{nl}");
        richTextView.RichTextBox.Enter($"Paragraph{nl}");
        richTextView.RichTextBox.Enter($"Paragraph (Increase indentation){nl}");
        richTextView.RichTextBox.Enter($"Paragraph{nl}");

        SelectLine(0);        
        homeTab.ToggleBoldButton.Click();
        homeTab.ToggleItalicButton.Click();
        homeTab.ToggleUnderlineButton.Click();
        SelectLine(1);
        homeTab.ToggleItalicButton.Click();
        homeTab.ToggleUnderlineButton.Click();
        SelectLine(2);
        homeTab.IncreaseIndentationButton.Click();

        SelectLine(int.MaxValue);
        homeTab.ToggleNumberingButton.Click();
        richTextView.RichTextBox.Enter($"Numbering list first{nl}Numbering list second{nl}{nl}");
        homeTab.ToggleBulletsButton.Click();
        richTextView.RichTextBox.Enter($"Bullet list first{nl}Bullet list second");

        // Select the lines again the check the correct toggle button state
        SelectLine(int.MinValue);
        AssertFormatState(true, true, true, false, false);
        SelectLine(1);
        AssertFormatState(false, true, true, false, false);
        SelectLine(1);
        AssertFormatState(false, false, false, false, false);
        SelectLine(3);
        AssertFormatState(false, false, false, true, false);
        SelectLine(2);
        AssertFormatState(false, false, false, false, true);

        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.PrintPreviewMenuItem.Invoke();

        var printPreviewTab = window.PrintPreviewTab;
        Assert.True(printPreviewTab.IsSelected);
        printPreviewTab.ZoomOutButton.Click();
        Assert.Equal("90%", zoomComboBox.EditableText);
        PrintAsPdf(GetScreenshotFile("HelloWorld.pdf"));
        printPreviewTab.ClosePrintPreviewButton.Click();

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.NewMenuItem.Invoke();

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();

        var saveChangesWindow = window.FirstModalWindow().As<SaveChangesWindow>();
        var firstItem = saveChangesWindow.FilesToSaveList.Items.Single();
        Assert.Equal("Document 1.rtf", firstItem.Text);
        saveChangesWindow.NoButton.Click();

        void SelectLine(int line)
        {
            range.Move(TextUnit.Line, line);
            range.ExpandToEnclosingUnit(TextUnit.Line);
            range.Select();
        }

        void AssertFormatState(bool isBold, bool isItalic, bool isUnderline, bool isNumbering, bool isBullets)
        {
            Assert.Equal((isBold, isItalic, isUnderline), (homeTab.ToggleBoldButton.IsToggled, homeTab.ToggleItalicButton.IsToggled, homeTab.ToggleUnderlineButton.IsToggled));
            Assert.Equal((isNumbering, isBullets), (homeTab.ToggleNumberingButton.IsToggled, homeTab.ToggleBulletsButton.IsToggled));
        }

        void PrintAsPdf(string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);

            printPreviewTab.PrintButton.Click();
            var version = new Version(10, 0, 22621, 0);  // Windows 11 22H2
            if (Environment.OSVersion.Version >= version)
            {
                var printDialog = PrintDialog.GetDialog(Automation);
                printDialog.PrinterSelector.Select(printDialog.PrintToPdf.Name);
                Retry.WhileFalse(() => printDialog.PrintButton.IsEnabled, throwOnTimeout: true);
                printDialog.PrintButton.Invoke();
            }
            else
            {
                var printDialog = window.FirstModalWindow().As<LegacyPrintDialog>();
                printDialog.PrinterList.Select(printDialog.PrintToPdf.Text);
                printDialog.PrintButton.Click();
            }

            var saveFileDialog = window.FirstModalWindow().As<SaveFileDialog>();
            saveFileDialog.SetFileName(fileName);
            saveFileDialog.SaveButton.Click();
        }
    });

    [Fact]
    public void MultipleNewSaveRestartOpenRecentChangeAskToSave() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();

        var startView = window.StartView;
        Assert.False(startView.IsOffscreen);

        // Create new document, add text, check tab name with dirty flag indicator '*'
        startView.NewButton.Click();
        var tab1 = window.DocumentTabItems.Single();
        tab1.RichTextView.RichTextBox.Text = "Hello World";
        Assert.Equal("Document 1.rtf*", tab1.TabName);

        // Create new document #2, add text, check tab name with dirty flag indicator '*'
        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.NewMenuItem.Invoke();
        var tab2 = window.DocumentTabItems[1];
        tab2.RichTextView.RichTextBox.Text = "Hello World 2";
        Assert.Equal("Document 2.rtf*", tab2.TabName);

        // Create new document #3, check tab name, close tab - no save dialog comes as text has not been changed
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.NewMenuItem.Invoke();
        var tab3 = window.DocumentTab.SelectedTabItem.As<DocumentTabItem>();
        Assert.Equal("Document 3.rtf", tab3.TabName);
        tab3.CloseButton.Invoke();

        // Select tab #1, close tab, save the document, define custom filename and save
        Retry.WhileFalse(() => tab2.IsSelected, throwOnTimeout: true);
        tab1.Select();
        tab1.CloseButton.Invoke();
        var saveChangesWindow = window.FirstModalWindow().As<SaveChangesWindow>();
        var firstItem = saveChangesWindow.FilesToSaveList.Items.Single();
        Assert.Equal("Document 1.rtf", firstItem.Text);
        saveChangesWindow.YesButton.Click();
        var saveFileDialog = window.FirstModalWindow().As<SaveFileDialog>();
        var fileName = GetTempFileName("rtf");
        saveFileDialog.SetFileName(fileName);
        Capture.Screen().ToFile(GetScreenshotFile("FullSaveScreen"));
        saveFileDialog.SaveButton.Click();
        Capture.Screen().ToFile(GetScreenshotFile("AfterSaveFile"));

        // Exit app, save the document, define custom filename and save
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();
        saveChangesWindow = window.FirstModalWindow().As<SaveChangesWindow>();
        firstItem = saveChangesWindow.FilesToSaveList.Items.Single();
        Assert.Equal("Document 2.rtf", firstItem.Text);
        saveChangesWindow.YesButton.Click();
        saveFileDialog = window.FirstModalWindow().As<SaveFileDialog>();
        var fileName2 = GetTempFileName("rtf");
        saveFileDialog.SetFileName(fileName2);
        saveFileDialog.SaveButton.Click();


        // Restart the app and check recent file list, pin second item -> moved to top
        Launch(resetSettings: false);
        window = GetShellWindow();
        startView = window.StartView;
        Capture.Screen().ToFile(GetScreenshotFile("RestartScreen"));
        Assert.Equal([fileName2, fileName], startView.RecentFileListItems.Select(x => x.ToolTip));
        startView.RecentFileListItems[1].PinButton.Click();
        Assert.Equal([fileName, fileName2], startView.RecentFileListItems.Select(x => x.ToolTip));
        Assert.True(startView.RecentFileListItems[0].PinButton.IsToggled);
        Assert.False(startView.RecentFileListItems[1].PinButton.IsToggled);

        // Check that the recent file list within the ribbon menu has the same content
        fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        Assert.Equal([fileName, fileName2], fileRibbonMenu.RecentFileListItems.Select(x => x.ToolTip));
        Assert.True(fileRibbonMenu.RecentFileListItems[0].PinButton.IsToggled);
        Assert.False(fileRibbonMenu.RecentFileListItems[1].PinButton.IsToggled);
        fileRibbonMenu.MenuButton.Toggle();

        // Open recent file #1, modify text, check tab name with dirty flag indicator '*'
        startView.RecentFileListItems[0].OpenFileButton.Invoke();
        tab1 = window.DocumentTabItems.Single();
        Assert.Equal(Path.GetFileName(fileName), tab1.TabName);
        AssertTextEqual("Hello World", tab1.RichTextView.RichTextBox);
        tab1.RichTextView.RichTextBox.Text = "- ";
        Assert.Equal(Path.GetFileName(fileName) + "*", tab1.TabName);

        // Open recent file #2
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.RecentFileListItems[^1].OpenFileButton.Click();
        tab2 = window.DocumentTab.SelectedTabItem.As<DocumentTabItem>();
        Assert.Equal(Path.GetFileName(fileName2), tab2.TabName);
        AssertTextEqual("Hello World 2", tab2.RichTextView.RichTextBox);

        // Close app, save dialog shows just file #1, don't save
        fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();
        saveChangesWindow = window.FirstModalWindow().As<SaveChangesWindow>();
        firstItem = saveChangesWindow.FilesToSaveList.Items.Single();
        Assert.Equal(fileName, firstItem.Text);
        saveChangesWindow.NoButton.Click();


        // Restart the app and check recent file list, use context menu to unpin, remove and open
        Launch(resetSettings: false);
        window = GetShellWindow();
        startView = window.StartView;
        Assert.Equal([fileName, fileName2], startView.RecentFileListItems.Select(x => x.ToolTip));
        Assert.True(startView.RecentFileListItems[0].PinButton.IsToggled);
        var contextMenu = startView.RecentFileListItems[0].ShowContextMenu();
        UIAssert.NotExists(() => _ = contextMenu.PinFileMenuItem);
        contextMenu.UnpinFileMenuItem.Invoke();
        Assert.False(startView.RecentFileListItems[0].PinButton.IsToggled);

        contextMenu = startView.RecentFileListItems[0].ShowContextMenu();
        UIAssert.NotExists(() => _ = contextMenu.UnpinFileMenuItem);
        contextMenu.RemoveFileMenuItem.Invoke();
        Assert.Equal([fileName2], startView.RecentFileListItems.Select(x => x.ToolTip));

        contextMenu = startView.RecentFileListItems[0].ShowContextMenu();
        contextMenu.OpenFileMenuItem.Invoke();
        tab1 = window.DocumentTab.SelectedTabItem.As<DocumentTabItem>();
        Assert.Equal(Path.GetFileName(fileName2), tab1.TabName);
        AssertTextEqual("Hello World 2", tab1.RichTextView.RichTextBox);
    });

    [Fact]
    public void SaveCloseAndOpen() => Run(() =>
    {
        Launch();
        var window = GetShellWindow();

        var startView = window.StartView;
        Assert.False(startView.IsOffscreen);

        startView.NewButton.Invoke();
        var tab1 = window.DocumentTabItems.Single();
        tab1.RichTextView.RichTextBox.Text = "Hello World";
        Assert.Equal("Document 1.rtf*", tab1.TabName);

        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.SaveMenuItem.Invoke();

        var saveDialog = window.FirstModalWindow().As<SaveFileDialog>();
        var fileName = GetTempFileName("rtf");
        saveDialog.SetFileName(fileName);
        saveDialog.SaveButton.Click();

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.CloseMenuItem.Invoke();

        Assert.Empty(window.DocumentTabItems);

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.OpenMenuItem.Invoke();

        var openDialog = window.FirstModalWindow().As<OpenFileDialog>();
        openDialog.SetFileName(fileName);
        openDialog.OpenButton.Click();

        tab1 = window.DocumentTabItems.Single();
        AssertTextEqual("Hello World", tab1.RichTextView.RichTextBox);

        window.Close();
    });
}
