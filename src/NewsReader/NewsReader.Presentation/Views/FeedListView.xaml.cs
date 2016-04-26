using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IFeedListView)), Shared]
    public sealed partial class FeedListView : UserControl, IFeedListView
    {
        private readonly Lazy<FeedListViewModel> viewModel;


        public FeedListView()
        {
            InitializeComponent();
            viewModel = new Lazy<FeedListViewModel>(() => (FeedListViewModel)DataContext);
        }


        public FeedListViewModel ViewModel => viewModel.Value;


        public void FeedAdded()
        {
            addFeedButton.Flyout.Hide();
        }

        private void AddNewFeedUriBoxKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.AddNewFeedCommand.Execute(null);
            }
        }

        private void AddFeedFlyoutCloseAction(object sender, RoutedEventArgs e)
        {
            addFeedButton.Flyout.Hide();
        }
    }
}
