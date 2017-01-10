using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Domain;
using Jbe.NewsReader.Presentation.Controls;
using System;
using System.ComponentModel;
using System.Composition;
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
        private bool isLoaded;
        private FeedItem feedItem;
        

        public FeedItemView()
        {
            InitializeComponent();
            webView = new WebView(WebViewExecutionMode.SeparateThread) { Visibility = Visibility.Collapsed };
            webViewPresenter.Content = webView;
            viewModel = new Lazy<FeedItemViewModel>(() => (FeedItemViewModel)DataContext);
            FrameworkElementHelper.RegisterSafeLoadedCallback(this, LoadedHandler);
            FrameworkElementHelper.RegisterSafeUnloadedCallback(this, UnloadedHandler);
            webView.NavigationStarting += WebViewNavigationStarting;
            webView.NavigationCompleted += WebViewNavigationCompleted;
        }


        public FeedItemViewModel ViewModel => viewModel.Value;

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
        

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            ViewModel.SelectionService.PropertyChanged += SelectionServicePropertyChanged;
            FeedItem = ViewModel.SelectionService.SelectedFeedItem;
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            isLoaded = false;
            ViewModel.SelectionService.PropertyChanged -= SelectionServicePropertyChanged;
            FeedItem = null;
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
            if (!isLoaded)
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
