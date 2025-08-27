using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record Entry(AppiumElement Element, string? Placeholder = null)
{
    public string Text => Element.Text;

    public bool IsTextEmpty => string.IsNullOrEmpty(Text) || string.Equals(Text, Placeholder, StringComparison.Ordinal);

    public void EnterText(string value)
    {
        Element.Click();
        Element.Clear();
        Element.SendKeys(value);
        if (Element.IsAndroid()) Element.GetDriver().HideKeyboard();
    }
}