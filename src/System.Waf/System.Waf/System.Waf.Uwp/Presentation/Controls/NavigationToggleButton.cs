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
        /// Identifies the <see cref="Symbol"/> dependency property.
        /// </summary>
        public static DependencyProperty SymbolProperty { get; } =
            DependencyProperty.Register(nameof(Symbol), typeof(Symbol), typeof(NavigationToggleButton), new PropertyMetadata(default(Symbol)));

        /// <summary>
        /// Gets or sets the symbol that is shown within the button.
        /// </summary>
        public Symbol Symbol
        {
            get { return (Symbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
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
