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
        /// Identifies the <see cref="LazyContent"/> dependency property.
        /// </summary>
        public static DependencyProperty LazyContentProperty { get; }
            = DependencyProperty.Register(nameof(LazyContent), typeof(Lazy<object>), typeof(LazyContentPresenter), new PropertyMetadata(null, LazyContentPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the content via the lazy construct.
        /// </summary>
        public Lazy<object> LazyContent
        {
            get { return (Lazy<object>)GetValue(LazyContentProperty); }
            set { SetValue(LazyContentProperty, value); }
        }

        private bool IsVisible
        {
            get { return isVisible; }
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
            IsVisible = actualSize.Width > 0 && actualSize.Height > 0;
            return actualSize;
        }

        private void UpdateContent()
        {
            if (LazyContent != null && IsVisible)
            {
                Content = LazyContent.Value;
            }
            else
            {
                Content = null;
            }
        }

        private static void LazyContentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LazyContentPresenter)d).UpdateContent();
        }
    }
}
