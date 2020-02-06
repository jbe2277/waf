using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class FeedItemViewModel : ViewModelCore<IFeedItemView>
    {
        private FeedItem feedItem;

        public FeedItemViewModel(IFeedItemView view) : base(view, false)
        {
        }

        public ICommand LaunchBrowserCommand { get; set; }

        public FeedItem FeedItem
        {
            get => feedItem;
            set => SetProperty(ref feedItem, value);
        }
    }
}
