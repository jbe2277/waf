namespace Waf.NewsReader.Applications.Controllers;

public interface IAppController
{
    object MainView { get; }

    void Start();

    void Save();

    void Update();
}
