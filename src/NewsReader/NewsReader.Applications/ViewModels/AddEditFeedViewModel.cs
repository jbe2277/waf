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

        public Feed Feed
        {
            get => feed;
            set => SetProperty(ref feed, value);
        }

        public string LoadErrorMessage
        {
            get => loadErrorMessage;
            set => SetProperty(ref loadErrorMessage, value);
        }
    }
}
