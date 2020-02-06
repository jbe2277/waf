using System.ComponentModel;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class AddEditFeedViewModel : ViewModelCore<IAddEditFeedView>
    {
        private readonly DelegateCommand useTitleAsNameCommand;
        private bool isEditMode;
        private string feedUrl;
        private Feed oldFeed;
        private Feed feed;
        private string loadErrorMessage;

        public AddEditFeedViewModel(IAddEditFeedView view) : base(view, false)
        {
            useTitleAsNameCommand = new DelegateCommand(() => Feed.Name = Feed.Title, () => !string.IsNullOrEmpty(Feed?.Title) && Feed.Name != Feed.Title);
        }

        public ICommand LoadFeedCommand { get; set; }

        public ICommand AddUpdateCommand { get; set; }

        public ICommand UseTitleAsNameCommand => useTitleAsNameCommand;

        public event PropertyChangedEventHandler FeedChanged;

        public bool IsEditMode
        {
            get => isEditMode;
            set => SetProperty(ref isEditMode, value);
        }

        public string FeedUrl
        {
            get => feedUrl;
            set
            {
                if (SetProperty(ref feedUrl, value))
                {
                    Feed = null;
                    LoadErrorMessage = null;
                }
            }
        }

        public Feed OldFeed
        {
            get => oldFeed;
            set => SetProperty(ref oldFeed, value);
        }

        public Feed Feed
        {
            get => feed;
            set
            {
                var oldFeed = feed;
                if (SetProperty(ref feed, value))
                {
                    if (oldFeed != null) oldFeed.PropertyChanged -= FeedPropertyChanged;
                    if (feed != null) feed.PropertyChanged += FeedPropertyChanged;
                }
            }
        }

        public bool IsSameFeed => OldFeed == Feed;

        public string LoadErrorMessage
        {
            get => loadErrorMessage;
            set => SetProperty(ref loadErrorMessage, value);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Feed)) useTitleAsNameCommand.RaiseCanExecuteChanged();
            if (new[] { nameof(OldFeed), nameof(Feed) }.Contains(e.PropertyName)) RaisePropertyChanged(nameof(IsSameFeed));
        }

        private void FeedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FeedChanged(sender, e);
            if (new[] { nameof(Feed.Title), nameof(Feed.Name) }.Contains(e.PropertyName)) useTitleAsNameCommand.RaiseCanExecuteChanged();
        }
    }
}
