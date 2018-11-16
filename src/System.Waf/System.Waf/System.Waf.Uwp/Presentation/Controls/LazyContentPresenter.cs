using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace System.Waf.Presentation.Controls
{
    /// <summary>
    /// Displays the lazy content when the <see cref="LazyContentPresenter"/> is visible.
    /// </summary>
    [ContentProperty(Name = null)]
    public sealed class LazyContentPresenter : ContentPresenter
    {
        private bool isVisible;


        /// <summary>
        /// Initializes a new instance of the <see cref="LazyContentPresenter"/> class.
        /// </summary>
        public LazyContentPresenter()
        {
            // Unloading can set ActualWidth/Height to 0 without calling Arrange
            Loaded += (sender, e) => UpdateIsVisible();
            Unloaded += (sender, e) => UpdateIsVisible();
        }


        /// <summary>
        /// Identifies the <see cref="LazyContent"/> dependency property.
        /// </summary>
        public static DependencyProperty LazyContentProperty { get; }
            = DependencyProperty.Register(nameof(LazyContent), typeof(Lazy<object>), typeof(LazyContentPresenter), new PropertyMetadata(null, LazyContentPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the content via the lazy construct.
        /// </summary>
        public Lazy<object> LazyContent
        {
            get => (Lazy<object>)GetValue(LazyContentProperty);
            set => SetValue(LazyContentProperty, value);
        }

        private bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    UpdateContent();
                }
            }
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>The actual size that is used after the element is arranged in layout.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var actualSize = base.ArrangeOverride(finalSize);
            IsVisible = Visibility == Visibility.Visible && actualSize.Width > 0 && actualSize.Height > 0;
            return actualSize;
        }

        private void UpdateIsVisible()
        {
            IsVisible = Visibility == Visibility.Visible && ActualWidth > 0 && ActualHeight > 0;
        }

        private void UpdateContent()
        {
            Content = LazyContent != null && IsVisible ? LazyContent.Value : null;
        }

        private static void LazyContentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LazyContentPresenter)d).UpdateContent();
        }
    }
}
