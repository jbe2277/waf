using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.ViewModels;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class AppController : IAppController
    {
        public AppController(ShellViewModel shellViewModel)
        {
            shellViewModel.FooterMenu = new[]
            {
                new NavigationItem("Settings", "\uf493")
            };
            shellViewModel.Initialize();
            MainView = shellViewModel.View;
        }

        public object MainView { get; }

        public void Start()
        {
        }

        public void Sleep()
        {
        }

        public void Resume()
        {
        }
    }
}
