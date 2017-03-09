using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Presentation.Controls;
using System;
using System.Composition;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IFeedItemListView)), Shared]
    public sealed partial class FeedItemListView : UserControl, IFeedItemListView
    {
        private readonly Lazy<FeedItemListViewModel> viewModel;
        private readonly ISelectionStateManager selectionStateManager;


        public FeedItemListView()
        {
            InitializeComponent();
            viewModel = new Lazy<FeedItemListViewModel>(() => (FeedItemListViewModel)DataContext);
            SetDefaultSearchVisibility();
            selectionStateManager = SelectionStateHelper.CreateManager(feedItemListView, selectItemsButton, cancelSelectionButton);
        }


        public FeedItemListViewModel ViewModel => viewModel.Value;


        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var isControlKeyDown = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            if (!isControlKeyDown && e.Key == VirtualKey.F5)
            {
                ViewModel.RefreshCommand.Execute(null);
            }
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

        private void FeedItemListViewItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.ShowFeedItemViewCommand.Execute(e.ClickedItem);
        }

        private void FeedDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewModel.ShowFeedItemViewCommand.Execute(((FrameworkElement)sender).DataContext);
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e) => ShowSearch();

        private async void ShowSearch()
        {
            searchButton.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Visible;
            await Dispatcher.RunIdleAsync(ha => { });
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

        private void SearchBoxLostFocus(object sender, RoutedEventArgs e) => SetDefaultSearchVisibility();
        
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e) => SetDefaultSearchVisibility();

        private async void FeedItemListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Dispatcher.RunIdleAsync(ha => { });  // Ensure that items are layouted first so that ScrollIntoView works correct.
            
            if (feedItemListView.SelectedItem != null)
            {
                feedItemListView.ScrollIntoView(feedItemListView.SelectedItem);

                // When the element is not yet available (virtualized) then scroll into view again
                if (feedItemListView.ContainerFromItem(feedItemListView.SelectedItem) == null)
                {
                    await Dispatcher.RunIdleAsync(ha => { });
                    feedItemListView.ScrollIntoView(feedItemListView.SelectedItem);
                }
            }
        }

        private static FontWeight GetFontWeight(bool markAsRead) => markAsRead ? FontWeights.Normal : FontWeights.SemiBold;
    }
}
