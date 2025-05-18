using Xunit;

namespace UITest.NewsReader;
public class NewsReaderTest : UITest
{
    [Fact]
    public void Start()
    {
        GetShellWindow();
        Thread.Sleep(1000);
        CreateScreenshot("About");
    }
}
