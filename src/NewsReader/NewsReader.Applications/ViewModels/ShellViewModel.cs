using System.Collections.Generic;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.DataModels;
using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class ShellViewModel : ViewModelCore<IShellView>, INavigationService
    {
        private NavigationItem? selectedFooterMenu;
        private IReadOnlyList<Feed> feeds = null!;
        private Feed? selectedFeed;

        public ShellViewModel(IShellView view, IAppInfoService appInfoService) : base(view, false)
        {
            AppName = appInfoService.AppName;
        }

        public string AppName { get; }

        public ICommand EditFeedCommand { get; internal set; } = null!;

        public ICommand MoveFeedUpCommand { get; internal set; } = null!;

        public ICommand MoveFeedDownCommand { get; internal set; } = null!;

        public ICommand RemoveFeedCommand { get; internal set; } = null!;

        public ICommand ShowFeedViewCommand { get; internal set; } = null!;

        public IReadOnlyList<NavigationItem> FooterMenu { get; internal set; } = null!;

        public NavigationItem? SelectedFooterMenu
        {
            get => selectedFooterMenu;
            set
            {
                if (SetProperty(ref selectedFooterMenu, value) && selectedFooterMenu != null)
                {
                    SelectedFeed = null;
                }
            }
        }

        public IReadOnlyList<Feed> Feeds
        {
            get => feeds;
            set => SetProperty(ref feeds, value);
        }

        public Feed? SelectedFeed
        {
            get => selectedFeed;
            set
            {
                if (SetProperty(ref selectedFeed, value) && SelectedFeed != null)
                {
                    SelectedFooterMenu = null;
                }
            }
        }

        public Task Navigate(IViewModelCore viewModel)
        {
            viewModel.Initialize();
            return ViewCore.PushAsync(viewModel.View);
        }

        public Task NavigateBack()
        {
            return ViewCore.PopAsync();
        }
    }
}
