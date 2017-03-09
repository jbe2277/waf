using System.Waf.Presentation.Controls;
using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Jbe.NewsReader.Presentation.Controls;
using System.Linq;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IShellView)), Shared]
    public sealed partial class ShellView : Page, IShellView
    {
        private readonly Lazy<ShellViewModel> viewModel;


        public ShellView()
        {
            InitializeComponent();
            viewModel = new Lazy<ShellViewModel>(() => (ShellViewModel)DataContext);
            Loaded += FirstTimeLoadedHandler;

            contentView.RegisterPropertyChangedCallback(ContentPresenter.ContentProperty, ContentChanged);
            previewView.RegisterPropertyChangedCallback(LazyContentPresenter.LazyContentProperty, PreviewChanged);
            UpdatePreviewColumnWidth();

            var window = CoreWindow.GetForCurrentThread();
            UpdateHideBottomToolBar(window.Bounds.Width);
            UpdateSelectionState(window.Bounds.Width);
            window.SizeChanged += WindowSizeChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManagerBackRequested;
        }


        public ShellViewModel ViewModel => viewModel.Value;


        public void Show()
        {
            Window.Current.Content = this;
            Window.Current.Activate();
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= FirstTimeLoadedHandler;

            ViewModel.NavigateBackCommand.CanExecuteChanged += NavigateBackCommandCanExecuteChanged;
            UpdateBackButtonVisibility();
        }

        private void WindowSizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            UpdateHideBottomToolBar(args.Size.Width);
            UpdateSelectionState(args.Size.Width);
        }
        
        private void UpdateHideBottomToolBar(double windowWidth) => ToolBarHelper.SetHideBottomToolBar(navigationSplitView, windowWidth > 600);

        private void UpdateSelectionState(double windowWidth) => SelectionStateHelper.SetIsSinglePageViewSize(this, windowWidth < 720);

        private void ContentChanged(DependencyObject obj, DependencyProperty property) => navigationSplitView.IsPaneOpen = false;
        
        private void PreviewChanged(DependencyObject obj, DependencyProperty property) => UpdatePreviewColumnWidth();

        private void UpdatePreviewColumnWidth()
        {
            previewColumn.Width = previewView.LazyContent == null ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
        }

        private void NavigateBackCommandCanExecuteChanged(object sender, EventArgs e) => UpdateBackButtonVisibility();

        private void UpdateBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = ViewModel.NavigateBackCommand.CanExecute(null)
                ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void SystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled && ViewModel.NavigateBackCommand.CanExecute(null))
            {
                e.Handled = true;
                ViewModel.NavigateBackCommand.Execute(null);
            }
        }

        private static bool IsNavigationSelected(NavigationItem selectedNavigationItem, string navigationItems)
        {
            var items = navigationItems.Split(';').Select(x => Enum.Parse(typeof(NavigationItem), x.Trim()));
            return items.Any(x => x.Equals(selectedNavigationItem));
        }
    }
}
