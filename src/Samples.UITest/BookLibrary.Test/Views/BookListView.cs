using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using UITest.BookLibrary.Controls;

namespace UITest.BookLibrary.Views;

public class BookListView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public Button AddButton => this.Find("AddButton").AsButton();

    public Button RemoveButton => this.Find("RemoveButton").AsButton();

    public TextBox SearchBox => this.Find("SearchBox").AsTextBox();

    public Grid BookDataGrid => this.Find("BookDataGrid").AsGrid();
}

public class BookGridRow(FrameworkAutomationElementBase element) : GridRow(element)
{
    public TextGridCell TitleCell => Cells[0].As<TextGridCell>();

    public TextGridCell AuthorCell => Cells[1].As<TextGridCell>();

    public TextGridCell PublishDateCell => Cells[2].As<TextGridCell>();

    public LendToGridCell LendToCell => Cells[3].As<LendToGridCell>();
}

public class LendToGridCell(FrameworkAutomationElementBase element) : GridCell(element)
{
    public Label LendToLabel => this.Find("LendToLabel").AsLabel();

    public Button LendToButton => this.Find("LendToButton").AsButton();
}