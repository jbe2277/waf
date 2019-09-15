using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class FeedItemViewModel : ViewModel<IFeedItemView>
    {
        private FeedItem feedItem;

        public FeedItemViewModel(IFeedItemView view) : base(view)
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
