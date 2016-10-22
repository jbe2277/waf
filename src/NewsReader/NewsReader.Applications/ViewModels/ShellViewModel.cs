using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using System.Waf.Applications;
using System.Windows.Input;

namespace Jbe.NewsReader.Applications.ViewModels
{
    [Export, Shared]
    public class ShellViewModel : ViewModelCore<IShellView>
    {
        private object contentView;
        private Lazy<object> lazyPreviewView;
        private NavigationItem selectedNavigationItem;


        [ImportingConstructor]
        public ShellViewModel(IShellView view, IAccountInfoService accountInfoService) : base(view)
        {
            AccountInfoService = accountInfoService;
        }


        public IAccountInfoService AccountInfoService { get; }

        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }

        public Lazy<object> LazyPreviewView
        {
            get { return lazyPreviewView; }
            set { SetProperty(ref lazyPreviewView, value); }
        }

        public ICommand NavigateBackCommand { get; set; }

        public ICommand ShowNewsViewCommand { get; set; }

        public ICommand ShowReviewViewCommand { get; set; }

        public ICommand ShowSettingsViewCommand { get; set; }

        public ICommand SignInCommand { get; set; }

        public ICommand SignOutCommand { get; set; }

        public NavigationItem SelectedNavigationItem
        {
            get { return selectedNavigationItem; }
            internal set { SetProperty(ref selectedNavigationItem, value); }
        }


        public void Show()
        {
            ViewCore.Show();
        }
    }
}
