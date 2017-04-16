using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using Jbe.NewsReader.Presentation.Controls;
using Jbe.NewsReader.Presentation.Services;
using System;
using System.ComponentModel;
using System.Composition;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
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
        private readonly AsyncDelegateCommand pasteCommand;
        private readonly ISelectionStateManager selectionStateManager;


        public FeedListView()
        {
            InitializeComponent();
            addFeedGrid.MaxWidth = Window.Current.CoreWindow.Bounds.Width - 24;  // Minus the margin defined by the Flyout
            viewModel = new Lazy<FeedListViewModel>(() => (FeedListViewModel)DataContext);
            pasteCommand = new AsyncDelegateCommand(PasteUriAsync, CanPasteUri);
            Clipboard.ContentChanged += ClipboardContentChanged;
            selectionStateManager = SelectionStateHelper.CreateManager(feedListView, selectItemsButton, cancelSelectionButton);
            selectionStateManager.PropertyChanged += SelectionStateManagerPropertyChanged;
        }


        public FeedListViewModel ViewModel => viewModel.Value;

        public ICommand PasteCommand => pasteCommand;


        public void FeedAdded() => addFeedButton.Flyout.Hide();

        private void AddFeedFlyoutOpened(object sender, object e)
        {
            ViewModel.LoadErrorMessage = null;
            feedBox.Select(feedBox.Text.Length, 0);
            feedBox.Focus(FocusState.Programmatic);
            pasteCommand.RaiseCanExecuteChanged();  // Manual update necessary because of the CanPasteUri workaround.
        }

        private void AddNewFeedUriBoxKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.AddNewFeedCommand.Execute(null);
            }
        }

        private void AddFeedFlyoutCloseAction(object sender, RoutedEventArgs e) => addFeedButton.Flyout.Hide();
        
        private bool CanPasteUri()
        {
            try
            {
                return Clipboard.GetContent().Contains(StandardDataFormats.Text);
            }
            catch (Exception)
            {
                // Workaround: The first call comes too early. An UnauthorizedAccessException is thrown but only when the debugger is not running.
                return false;
            }
        }

        private async Task PasteUriAsync() => ViewModel.AddNewFeedUri = await Clipboard.GetContent().GetTextAsync();
        
        private void ClipboardContentChanged(object sender, object e) => pasteCommand.RaiseCanExecuteChanged();
        
        private void FeedListViewItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.ShowFeedItemListViewCommand.Execute(e.ClickedItem);
        }

        private void FeedDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewModel.ShowFeedItemListViewCommand.Execute(((FrameworkElement)sender).DataContext);
        }

        private void SelectionStateManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISelectionStateManager.SelectionState))
            {
                addFeedButton.Visibility = selectionStateManager.SelectionState == SelectionState.ExtendedSelection
                    || selectionStateManager.SelectionState == SelectionState.MultipleSelection ? Visibility.Collapsed : Visibility.Visible;
                removeFeedButton.Visibility = selectionStateManager.SelectionState == SelectionState.Master ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private static string GetItemsCountText(int itemsCount) => ResourceService.GetString("ItemsCount", itemsCount);
        
        private static string GetUnreadItemsCountText(int itemsCount) => ResourceService.GetString("UnreadItemsCount", itemsCount);
    }
}
