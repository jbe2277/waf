using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IFeedItemListView)), Shared]
    public sealed partial class FeedItemListView : UserControl, IFeedItemListView
    {
        private readonly Lazy<FeedItemListViewModel> viewModel;


        public FeedItemListView()
        {
            InitializeComponent();
            viewModel = new Lazy<FeedItemListViewModel>(() => (FeedItemListViewModel)DataContext);
            SetDefaultSearchVisibility();
        }


        public FeedItemListViewModel ViewModel => viewModel.Value;


        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var isControlKeyDown = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            if (isControlKeyDown && e.Key == VirtualKey.U)
            {
                ViewModel.ReadUnreadCommand.Execute("unread");
            }
            else if (isControlKeyDown && e.Key == VirtualKey.Q)
            {
                ViewModel.ReadUnreadCommand.Execute("read");
            }
            else if (isControlKeyDown && e.Key == VirtualKey.F)
            {
                ShowSearch();
            }
        }

        private void FeedDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewModel.ShowFeedItemViewCommand.Execute(((FrameworkElement)sender).DataContext);
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            ShowSearch();
        }

        private async void ShowSearch()
        {
            searchButton.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Visible;
            await Task.Delay(10);
            searchBox.Focus(FocusState.Programmatic);
        }

        private void SetDefaultSearchVisibility()
        {
            if (ActualWidth >= 600 || !string.IsNullOrEmpty(searchBox.Text))
            {
                searchButton.Visibility = Visibility.Collapsed;
                searchBox.Visibility = Visibility.Visible;
            }
            else
            {
                searchButton.Visibility = Visibility.Visible;
                searchBox.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchBoxLostFocus(object sender, RoutedEventArgs e)
        {
            SetDefaultSearchVisibility();
        }
        
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            SetDefaultSearchVisibility();
        }
    }
}
