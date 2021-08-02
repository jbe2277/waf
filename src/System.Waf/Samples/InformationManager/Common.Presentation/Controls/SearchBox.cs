using System.Windows;
using System.Windows.Controls;

namespace Waf.InformationManager.Common.Presentation.Controls
{
    /// <summary>Represents a control that can be used to enter a search or filter text.</summary>
    public class SearchBox : Control
    {
        /// <summary>Identifies the SearchText dependency property.</summary>
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchBox), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>Identifies the HintText dependency property.</summary>
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register(nameof(HintText), typeof(string), typeof(SearchBox), new FrameworkPropertyMetadata(""));

        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }

        /// <summary>Gets or sets the search text.</summary>
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        /// <summary>Gets or sets the hint text. This text is shown in the background if no search text is entered.</summary>
        public string HintText
        {
            get => (string)GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }
    }
}
