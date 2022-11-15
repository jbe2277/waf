using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;

namespace Waf.NewsReader.Presentation.Views
{
    public partial class FeedItemView : IFeedItemView
    {
        private FeedItemViewModel viewModel = null!;
        private TaskCompletionSource<object?>? webViewNavigated;
        private CancellationTokenSource cancellationTokenSource;

        public FeedItemView()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public object? DataContext
        {
            get => BindingContext;
            set => BindingContext = value;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (FeedItemViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cancellationTokenSource = new CancellationTokenSource();
            webView.Source = viewModel.FeedItem?.Uri;
        }

        protected override void OnDisappearing()
        {
            cancellationTokenSource.Cancel();
            webView.Source = null;
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
                return true;
            }
            else return base.OnBackButtonPressed();
        }

        private async void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            webViewNavigated = new TaskCompletionSource<object?>();
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            var delayTask = Task.Delay(TimeSpan.FromSeconds(5), cancellationTokenSource.Token);
            try
            {
                await Task.WhenAny(delayTask, webViewNavigated.Task);
            }
            catch (OperationCanceledException) { }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }

            try
            {
                await delayTask;
                if (viewModel.FeedItem != null)
                {
                    viewModel.FeedItem.MarkAsRead = true;
                }
            }
            catch (OperationCanceledException) { }
        }

        private void WebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            webViewNavigated?.TrySetResult(null);
        }
    }
}