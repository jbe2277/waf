using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class ShellViewModel : ViewModelCore<IShellView>
    {
        private readonly ObservableCollection<KeyValuePair<string, Exception>> messages;
        private object contentView;
        private Lazy<object> lazyPreviewView;
        private NavigationItem selectedNavigationItem;
        

        [ImportingConstructor]
        public ShellViewModel(IShellView view, IAccountInfoService accountInfoService, INetworkInfoService networkInfoService) : base(view)
        {
            AccountInfoService = accountInfoService;
            NetworkInfoService = networkInfoService;
            messages = new ObservableCollection<KeyValuePair<string, Exception>>();
            Messages = new ReadOnlyObservableList<KeyValuePair<string, Exception>>(messages);
            CloseMessageCommand = new DelegateCommand(CloseMessage);
            messages.CollectionChanged += MessagesCollectionChanged;
        }

        public IAccountInfoService AccountInfoService { get; }

        public INetworkInfoService NetworkInfoService { get; }

        public IReadOnlyObservableList<KeyValuePair<string, Exception>> Messages { get; }

        public KeyValuePair<string, Exception> LastMessage => messages.LastOrDefault();

        public object ContentView
        {
            get => contentView;
            set => SetProperty(ref contentView, value);
        }

        public Lazy<object> LazyPreviewView
        {
            get => lazyPreviewView;
            set => SetProperty(ref lazyPreviewView, value);
        }

        public ICommand CloseMessageCommand { get; }

        public ICommand NavigateBackCommand { get; set; }

        public ICommand ShowNewsViewCommand { get; set; }

        public ICommand ShowReviewViewCommand { get; set; }

        public ICommand ShowSettingsViewCommand { get; set; }

        public ICommand SignInCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        public NavigationItem SelectedNavigationItem
        {
            get => selectedNavigationItem;
            internal set => SetProperty(ref selectedNavigationItem, value);
        }


        public void Show()
        {
            ViewCore.Show();
        }

        public void ShowMessage(string message, Exception exception)
        {
            messages.Add(new KeyValuePair<string, Exception>(message, exception));
        }

        private void CloseMessage()
        {
            if (messages.Any()) { messages.RemoveAt(messages.Count - 1); }
        }

        private void MessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(LastMessage));
        }
    }
}
