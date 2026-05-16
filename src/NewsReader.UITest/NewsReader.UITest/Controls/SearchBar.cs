using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record SearchBar(AppiumElement Element)
{
    public AppiumElement Entry => Element.OnPlatform(
        android: () => Element,
        windows: () => Element.Find("TextBox"),
        iOS: () => Element.Find(MobileBy.ClassName("XCUIElementTypeSearchField")));

    public string Text => Entry.Text;

    public void Click() => Entry.SafeClick();

    public void Clear() => Entry.Clear();

    public void EnterText(string text)
    {
        Click();
        Clear();
        Entry.SendKeys(text);
    }
}
