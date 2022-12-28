namespace Waf.NewsReader.Applications.Controllers;

public interface IAppController
{
    object MainView { get; }

    void Start();

    void Sleep();

    void Resume();
}
