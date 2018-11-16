using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace System.Waf.Presentation.Controls
{
    /// <summary>
    /// Use this button for navigation commands in the navigation view. The toggle state can be used to show the current
    /// navigation state.
    /// </summary>
    public class NavigationToggleButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationToggleButton"/> class.
        /// </summary>
        public NavigationToggleButton()
        {
            DefaultStyleKey = typeof(NavigationToggleButton);
        }


        /// <summary>
        /// Identifies the <see cref="Icon"/> dependency property.
        /// </summary>
        public static DependencyProperty IconProperty { get; } =
            DependencyProperty.Register(nameof(Icon), typeof(IconElement), typeof(NavigationToggleButton), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the graphic content of the navigation toggle button.
        /// </summary>
        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }


        /// <summary>
        /// Called when the ToggleButton receives toggle stimulus.
        /// </summary>
        protected override void OnToggle()
        {
            // Disable the automatic toggle behavior: do not call the base implementation.
        }
    }
}
