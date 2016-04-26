using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IFeedItemView)), Shared]
    public sealed partial class FeedItemView : UserControl, IFeedItemView
    {
        private readonly Lazy<FeedItemViewModel> viewModel;
        private TaskCompletionSource<object> unloadedTaskSource;


        public FeedItemView()
        {
            InitializeComponent();
            viewModel = new Lazy<FeedItemViewModel>(() => (FeedItemViewModel)DataContext);
            unloadedTaskSource = new TaskCompletionSource<object>();
            unloadedTaskSource.SetResult(null);
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
            webView.NavigationStarting += WebViewNavigationStarting;
            webView.NavigationCompleted += WebViewNavigationCompleted;
        }


        public FeedItemViewModel ViewModel => viewModel.Value;

        private bool IsLoaded => !unloadedTaskSource.Task.IsCompleted;


        private async void LoadedHandler(object sender, RoutedEventArgs e)
        {
            // Workaround because the Windows Runtime does not guarantee that first the Unloaded handler is called before the second Loaded handler comes.
            await unloadedTaskSource.Task;
            unloadedTaskSource = new TaskCompletionSource<object>();
            webView.Navigate(webView.Source);
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            unloadedTaskSource.SetResult(null);
        }

        private void WebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs e)
        {
            if (!IsLoaded)
            {
                e.Cancel = true;
                return;
            }
            loadingProgressBar.Visibility = Visibility.Visible;
        }

        private void WebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            loadingProgressBar.Visibility = Visibility.Collapsed;
            if (ViewModel.SelectionService.SelectedFeedItem != null)
            {
                ViewModel.SelectionService.SelectedFeedItem.MarkAsRead = true;
            }
        }
    }
}
