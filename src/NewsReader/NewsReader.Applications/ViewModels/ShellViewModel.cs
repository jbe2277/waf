using System.Collections.Generic;
using System.Threading.Tasks;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class ShellViewModel : ViewModel<IShellView>, INavigationService
    {
        public ShellViewModel(IShellView view, IAppInfoService appInfoService) : base(view)
        {
            AppName = appInfoService.AppName;
        }

        public string AppName { get; }

        public IReadOnlyList<NavigationItem> FooterMenu { get; set; }

        public Task PushAsync(IViewModel viewModel)
        {
            viewModel.Initialize();
            return ViewCore.PushAsync(viewModel.View);
        }
    }
}
