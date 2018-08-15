using Windows.UI.Xaml.Data;

namespace System.Waf.Presentation.Converters
{
    /// <summary>
    /// Special value converter that just returns the value.
    /// Use this converter with x:Bind Mode=TwoWay when binding to a dependency property of type object (e.g. SelectedItem).
    /// </summary>
    public sealed class NoOperationConverter : IValueConverter
    {
        /// <summary>
        /// Returns just the passed value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">This parameter will be ignored.</param>
        /// <param name="parameter">This parameter will be ignored.</param>
        /// <param name="language">This parameter will be ignored.</param>
        /// <returns>Returns just the passed value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        /// <summary>
        /// Returns just the passed value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">This parameter will be ignored.</param>
        /// <param name="parameter">This parameter will be ignored.</param>
        /// <param name="language">This parameter will be ignored.</param>
        /// <returns>Returns just the passed value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
