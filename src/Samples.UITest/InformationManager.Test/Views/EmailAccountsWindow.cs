using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using UITest.Controls;

namespace UITest.InformationManager.Views;

public class EmailAccountsWindow(FrameworkAutomationElementBase element) : Window(element)
{
    public Button NewButton => this.Find("NewButton").AsButton();

    public Button RemoveButton => this.Find("RemoveButton").AsButton();

    public Button EditButton => this.Find("EditButton").AsButton();

    public Button CloseButton => this.Find("CloseButton").AsButton();

    public Grid EmailAccountsDataGrid => this.Find("EmailAccountsDataGrid").AsGrid();
}

public class EmailAccountGridRow(FrameworkAutomationElementBase element) : GridRow(element)
{
    public TextGridCell NameCell => Cells[0].As<TextGridCell>();

    public TextGridCell EmailCell => Cells[1].As<TextGridCell>();

    public (string name, string email) ToTuple() => (NameCell.Text, EmailCell.Name);
}
