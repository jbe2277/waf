using Windows.UI.Xaml.Controls.Primitives;

namespace System.Waf.Presentation.Controls
{
    /// <summary>
    /// ToggleButton for showing and hiding the navigation view. Known as burger icon button.
    /// </summary>
    public class NavigationSplitViewToggleButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationSplitViewToggleButton"/> class.
        /// </summary>
        public NavigationSplitViewToggleButton()
        {
            DefaultStyleKey = typeof(NavigationSplitViewToggleButton);
        }
    }
}
