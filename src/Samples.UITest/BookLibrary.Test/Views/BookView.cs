using FlaUI.Core.AutomationElements;
using FlaUI.Core;

namespace UITest.BookLibrary.Views;

public class BookView(FrameworkAutomationElementBase element) : AutomationElement(element)
{
    public TextBox TitleTextBox => this.Find("TitleTextBox").AsTextBox();

    public TextBox AuthorTextBox => this.Find("AuthorTextBox").AsTextBox();

    public TextBox PublisherTextBox => this.Find("PublisherTextBox").AsTextBox();

    public DateTimePicker PublishDatePicker => this.Find("PublishDatePicker").AsDateTimePicker();

    public TextBox IsbnTextBox => this.Find("IsbnTextBox").AsTextBox();

    public ComboBox LanguageComboBox => this.Find("LanguageComboBox").AsComboBox();

    public TextBox PagesTextBox => this.Find("PagesTextBox").AsTextBox();

    public TextBox LendToTextBox => this.Find("LendToTextBox").AsTextBox();

    public Button LendToButton => this.Find("LendToButton").AsButton();
}
