using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
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


        public FeedListView()
        {
            InitializeComponent();
            viewModel = new Lazy<FeedListViewModel>(() => (FeedListViewModel)DataContext);
            pasteCommand = new AsyncDelegateCommand(PasteUriAsync, CanPasteUri);
            Clipboard.ContentChanged += ClipboardContentChanged;
        }

        
        public FeedListViewModel ViewModel => viewModel.Value;

        public ICommand PasteCommand => pasteCommand;


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

        private bool CanPasteUri()
        {
            return Clipboard.GetContent().Contains(StandardDataFormats.Text);
        }

        private async Task PasteUriAsync()
        {
            ViewModel.AddNewFeedUri = await Clipboard.GetContent().GetTextAsync();
        }

        private void ClipboardContentChanged(object sender, object e)
        {
            pasteCommand.RaiseCanExecuteChanged();
        }
    }
}
