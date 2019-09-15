using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class ShellViewModel : ViewModel<IShellView>, INavigationService
    {
        private IReadOnlyList<Feed> feeds;

        public ShellViewModel(IShellView view, IAppInfoService appInfoService) : base(view)
        {
            AppName = appInfoService.AppName;
        }

        public string AppName { get; }

        public ICommand RemoveFeedCommand { get; set; }

        public ICommand ShowFeedViewCommand { get; set; }

        public IReadOnlyList<NavigationItem> FooterMenu { get; set; }

        public IReadOnlyList<Feed> Feeds
        {
            get => feeds;
            set => SetProperty(ref feeds, value);
        }

        public Task PushAsync(IViewModel viewModel)
        {
            viewModel.Initialize();
            return ViewCore.PushAsync(viewModel.View);
        }
    }
}
