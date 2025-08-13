using OpenQA.Selenium.Appium;

namespace UITest.NewsReader.Controls;

public record SearchBar(AppiumElement Element)
{
    public AppiumElement Entry => Element.OnPlatform(
        android: () => Element,
        windows: () => Element.Find("TextBox"),
        iOS: () => throw new NotSupportedException());

    public string Text => Entry.Text;

    public void Click() => Entry.Click();

    public void Clear() => Entry.Clear();

    public void EnterText(string value)
    {
        Entry.SafeClick();
        Entry.Clear();
        if (Element.IsWindows())
        {
            foreach (var x in value)
            {
                Entry.SendKeys(x.ToString());
                Thread.Sleep(50);
            }
        }
        else Entry.SendKeys(value);
        
        if (Element.IsAndroid()) Element.GetDriver().HideKeyboard();
    }
}
