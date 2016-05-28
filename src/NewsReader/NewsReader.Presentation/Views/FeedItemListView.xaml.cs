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
            var controlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            if (controlState.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.U)
            {
                ViewModel.ReadUnreadCommand.Execute("unread");
            }
            else if (controlState.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.Q)
            {
                ViewModel.ReadUnreadCommand.Execute("read");
            }
        }

        private void FeedDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewModel.ShowFeedItemViewCommand.Execute(((FrameworkElement)sender).DataContext);
        }

        private async void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            searchButton.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Visible;
            await Task.Delay(10);
            searchBox.Focus(FocusState.Programmatic);
        }

        private void SetDefaultSearchVisibility()
        {
            if (ActualWidth >= 500)
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
