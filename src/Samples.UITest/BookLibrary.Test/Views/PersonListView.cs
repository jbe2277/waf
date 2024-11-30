using FlaUI.Core.AutomationElements;
using FlaUI.Core;
using UITest.BookLibrary.Controls;

namespace UITest.BookLibrary.Views;

public class PersonListView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Button AddButton => this.Find("AddButton").AsButton();

    public Button RemoveButton => this.Find("RemoveButton").AsButton();

    public TextBox SearchBox => this.Find("SearchBox").AsTextBox();

    public Grid PersonDataGrid => this.Find("PersonDataGrid").AsGrid();
}

public class PersonGridRow(FrameworkAutomationElementBase element) : GridRow(element)
{
    public TextGridCell FirstnameCell => Cells[0].As<TextGridCell>();

    public TextGridCell LastnameCell => Cells[1].As<TextGridCell>();

    public HyperlinkGridCell EmailCell => Cells[2].As<HyperlinkGridCell>();

    public (string firstname, string lastname, string email) ToTuple() => (FirstnameCell.Text, LastnameCell.Text, EmailCell.Name);
}
