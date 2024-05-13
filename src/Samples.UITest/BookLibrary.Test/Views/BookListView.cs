using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.BookLibrary.Views;

public class BookListView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox SearchBox => this.Find("SearchBox").AsTextBox();

    public Grid BookDataGrid => this.Find("BookDataGrid").AsGrid();
}

public class BookGridRow(FrameworkAutomationElementBase element) : GridRow(element)
{
    public GridCell TitleCell => Cells[0];

    public GridCell AuthorCell => Cells[1];

    public GridCell PublishDateCell => Cells[2];

    public LendToGridCell LendToCell => Cells[3].As<LendToGridCell>();
}

public class LendToGridCell(FrameworkAutomationElementBase element) : GridCell(element)
{
    public Label LendToLabel => this.Find("LendToLabel").AsLabel();

    public Button LendToButton => this.Find("LendToButton").AsButton();
}