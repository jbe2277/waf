using System;
using Waf.NewsReader.Applications.ViewModels;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class AppController : IAppController
    {
        public AppController(Lazy<ShellViewModel> shellViewModel)
        {
            MainView = shellViewModel.Value.View;
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
