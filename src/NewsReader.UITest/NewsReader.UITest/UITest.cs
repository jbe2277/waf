using FlaUI.Core;
using FlaUI.Core.AutomationElements;

namespace UITest.NewsReader;

public class UITest() : UITestBase("Waf.NewsReader_a8txtqew917ny!App",
        Environment.GetEnvironmentVariable("UITestOutputPath") ?? "out/NewsReader.UITest/")
{
    public Application Launch()
    {
        return App = Application.LaunchStoreApp(AppId);        
    }

    public Window GetShellWindow() => App!.GetMainWindow(Automation) ?? throw new InvalidOperationException("MainWindow not found");
}
