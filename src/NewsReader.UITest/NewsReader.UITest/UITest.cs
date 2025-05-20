using UITest.NewsReader.Views;

namespace UITest.NewsReader;

public class UITest() : UITestBase("Waf.NewsReader_a8txtqew917ny!App",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/NewsReader.UITest/")
{
    public ShellWindow GetShellWindow() => new(Driver);
}
