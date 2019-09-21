using Waf.NewsReader.Applications.Views;
using Waf.NewsReader.Domain;

namespace Waf.NewsReader.Applications.ViewModels
{
    public class AddEditFeedViewModel : ViewModel<IAddEditFeedView>
    {
        private bool isEditMode;
        private string feedUrl;
        private Feed feed;

        public AddEditFeedViewModel(IAddEditFeedView view) : base(view)
        {
        }

        public bool IsEditMode
        {
            get => isEditMode;
            set => SetProperty(ref isEditMode, value);
        }

        public string FeedUrl
        {
            get => feedUrl;
            set => SetProperty(ref feedUrl, value);
        }

        public Feed Feed
        {
            get => feed;
            set => SetProperty(ref feed, value);
        }
    }
}
