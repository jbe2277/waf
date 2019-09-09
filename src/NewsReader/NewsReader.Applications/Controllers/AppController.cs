using System;
using Waf.NewsReader.Applications.ViewModels;

namespace Waf.NewsReader.Applications.Controllers
{
    internal class AppController : IAppController
    {
        private readonly Lazy<ShellViewModel> shellViewModel;

        public AppController(Lazy<ShellViewModel> shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }

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
