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
        private NavigationItem selectedFooterMenu;
        private IReadOnlyList<Feed> feeds;
        private Feed selectedFeed;

        public ShellViewModel(IShellView view, IAppInfoService appInfoService) : base(view)
        {
            AppName = appInfoService.AppName;
        }

        public string AppName { get; }

        public ICommand EditFeedCommand { get; set; }

        public ICommand RemoveFeedCommand { get; set; }

        public ICommand ShowFeedViewCommand { get; set; }

        public IReadOnlyList<NavigationItem> FooterMenu { get; set; }

        public NavigationItem SelectedFooterMenu
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

        public Feed SelectedFeed
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

        public Task Navigate(IViewModel viewModel)
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
