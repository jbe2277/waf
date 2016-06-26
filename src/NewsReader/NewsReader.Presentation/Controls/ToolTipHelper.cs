using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Jbe.NewsReader.Presentation.Controls
{
    public static class ToolTipHelper
    {
        public static DependencyProperty ToolTipBindingPathProperty { get; } =
            DependencyProperty.RegisterAttached("ToolTipBindingPath", typeof(string), typeof(ToolTipHelper), new PropertyMetadata(null, UseLabelAsToolTipPropertyChanged));


        public static string GetToolTipBindingPath(DependencyObject obj)
        {
            return (string)obj.GetValue(ToolTipBindingPathProperty);
        }

        public static void SetToolTipBindingPath(DependencyObject obj, string value)
        {
            obj.SetValue(ToolTipBindingPathProperty, value);
        }


        private static void UseLabelAsToolTipPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var propertyPath = (string)e.NewValue;
            if (propertyPath != null)
            {
                BindingOperations.SetBinding(obj, ToolTipService.ToolTipProperty, new Binding
                {
                    Path = new PropertyPath(propertyPath),
                    RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self }
                });
            }
        }
    }
}
