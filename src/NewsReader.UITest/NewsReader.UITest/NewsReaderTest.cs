using FlaUI.Core.Capturing;
using Xunit;

namespace UITest.NewsReader;
public class NewsReaderTest : UITest
{
    [Fact]
    public void Start()
    {
        Launch();
        GetShellWindow();
        Thread.Sleep(1000);
        Capture.Screen().ToFile(GetScreenshotFile("About"));
    }
}
