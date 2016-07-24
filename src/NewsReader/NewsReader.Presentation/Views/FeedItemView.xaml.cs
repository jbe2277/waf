using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Domain;
using System;
using System.ComponentModel;
using System.Composition;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IFeedItemView)), Shared]
    public sealed partial class FeedItemView : UserControl, IFeedItemView
    {
        private static readonly Uri blankUri = new Uri("about:blank");
        private readonly Lazy<FeedItemViewModel> viewModel;
        private readonly WebView webView;
        private TaskCompletionSource<object> unloadedTaskSource;
        private FeedItem feedItem;
        

        public FeedItemView()
        {
            InitializeComponent();
            webView = new WebView(WebViewExecutionMode.SeparateThread) { Visibility = Visibility.Collapsed };
            webViewPresenter.Content = webView;
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

        private FeedItem FeedItem
        {
            get { return feedItem; }
            set
            {
                if (feedItem != value)
                {
                    if (feedItem != null)
                    {
                        feedItem.PropertyChanged -= FeedItemPropertyChanged;
                    }
                    feedItem = value;
                    UpdateWebView();
                    if (feedItem != null)
                    {
                        feedItem.PropertyChanged += FeedItemPropertyChanged;
                    }
                }
            }
        }
        

        private async void LoadedHandler(object sender, RoutedEventArgs e)
        {
            // Workaround because the Windows Runtime does not guarantee that first the Unloaded handler is called before the second Loaded handler comes.
            await unloadedTaskSource.Task;
            unloadedTaskSource = new TaskCompletionSource<object>();
            ViewModel.SelectionService.PropertyChanged += SelectionServicePropertyChanged;
            FeedItem = ViewModel.SelectionService.SelectedFeedItem;
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectionService.PropertyChanged -= SelectionServicePropertyChanged;
            FeedItem = null;
            unloadedTaskSource.SetResult(null);
        }

        private void SelectionServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectionService.SelectedFeedItem))
            {
                FeedItem = ViewModel.SelectionService.SelectedFeedItem;
            }
        }

        private void FeedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FeedItem.Uri))
            {
                UpdateWebView();
            }
        }

        private void UpdateWebView()
        {
            webView.Visibility = FeedItem != null ? Visibility.Visible : Visibility.Collapsed;
            var uri = FeedItem?.Uri;
            if (uri == null)
            {
                webView.Navigate(blankUri);
            }
            else if (webView.Source != uri)
            {
                webView.Navigate(uri);
            }
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
