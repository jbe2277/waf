using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace System.Waf.Presentation.Controls
{
    /// <summary>
    /// Represents a header content control that automatically reserves the space for the <see cref="NavigationSplitViewToggleButton"/>.
    /// This control should be in the child hierarchy of the <see cref="NavigationSplitView"/>.
    /// </summary>
    public class HeaderContentControl : ContentControl
    {
        private ContentPresenter presenter;
        private NavigationSplitView splitView;
        private long displayModePropertyChangedToken;
        private long isPaneOpenPropertyChangedToken;
        private TaskCompletionSource<object> unloadedTaskSource;


        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderContentControl"/> class.
        /// </summary>
        public HeaderContentControl()
        {
            DefaultStyleKey = typeof(HeaderContentControl);
            unloadedTaskSource = new TaskCompletionSource<object>();
            unloadedTaskSource.SetResult(null);
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }


        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is
        /// called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            presenter = (ContentPresenter)GetTemplateChild("ContentPresenter");
        }

        private async void LoadedHandler(object sender, RoutedEventArgs e)
        {
            // Workaround because the Windows Runtime does not guarantee that first the Unloaded handler is called before the second Loaded handler comes.
            await unloadedTaskSource.Task;
            unloadedTaskSource = new TaskCompletionSource<object>();

            splitView = GetAncestor<NavigationSplitView>(this);
            if (splitView != null)
            {
                displayModePropertyChangedToken = splitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, SplitViewDisplayModeChanged);
                isPaneOpenPropertyChangedToken = splitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, SplitViewIsPaneOpenChanged);
            }

            UpdateContentPresenterPadding();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            if (splitView != null)
            {
                splitView.UnregisterPropertyChangedCallback(SplitView.DisplayModeProperty, displayModePropertyChangedToken);
                splitView.UnregisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, isPaneOpenPropertyChangedToken);
            }
            splitView = null;
            unloadedTaskSource.SetResult(null);
        }

        private void UpdateContentPresenterPadding()
        {
            if (splitView != null)
            {
                presenter.Padding = splitView.DisplayMode == SplitViewDisplayMode.Overlay
                    || (splitView.DisplayMode == SplitViewDisplayMode.Inline && splitView.IsPaneOpen == false) ? new Thickness(48, 0, 0, 0) : new Thickness();
            }
        }

        private void SplitViewDisplayModeChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateContentPresenterPadding();
        }

        private void SplitViewIsPaneOpenChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateContentPresenterPadding();
        }

        private static TAncestorType GetAncestor<TAncestorType>(DependencyObject reference) where TAncestorType : DependencyObject
        {
            var parent = reference;
            while (parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is TAncestorType)
                {
                    return (TAncestorType)parent;
                }
            }
            return null;
        }
    }
}
