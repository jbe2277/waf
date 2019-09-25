using System.ComponentModel;
using System.Windows.Input;
using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class AddEditFeedViewModel : ViewModel<IAddEditFeedView>
    {
        private bool isEditMode;
        private string feedUrl;
        private Feed feed;
        private string loadErrorMessage;

        public AddEditFeedViewModel(IAddEditFeedView view) : base(view)
        {
        }

        public ICommand LoadFeedCommand { get; set; }

        public ICommand AddUpdateCommand { get; set; }

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

        public Feed OldFeed { get; set; }

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

        public string LoadErrorMessage
        {
            get => loadErrorMessage;
            set => SetProperty(ref loadErrorMessage, value);
        }

        private void FeedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FeedChanged(sender, e);
        }
    }
}
