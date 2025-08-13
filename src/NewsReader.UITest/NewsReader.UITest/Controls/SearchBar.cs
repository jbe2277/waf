using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record SearchBar(AppiumElement Element)
{
    public AppiumElement Entry => Element.OnPlatform(
        android: () => Element,
        windows: () => Element.Find("TextBox"),
        iOS: () => Element.Find(MobileBy.ClassName("XCUIElementTypeSearchField")));

    public string Text => Entry.Text;

    public void Click() => Entry.Click();

    public void Clear() => Entry.Clear();

    public void EnterText(string text)
    {
        Entry.SafeClick();
        Entry.Clear();
        Entry.SafeSendKeys(text);
        
        if (Element.IsAndroid()) Element.GetDriver().HideKeyboard();
    }
}
