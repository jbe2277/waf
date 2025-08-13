using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;

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
            var a = new Actions(Element.GetDriver());
            foreach (var x in value)
            {
                a.SendKeys(x.ToString()).Pause(TimeSpan.FromMilliseconds(100));
            }
            a.Perform();
        }
        else Entry.SendKeys(value);
        
        if (Element.IsAndroid()) Element.GetDriver().HideKeyboard();
    }
}
