using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Jbe.NewsReader.Presentation.Controls
{
    public static class FrameworkElementHelper
    {
        private static DependencyProperty UnloadedTaskSourceProperty { get; } =
            DependencyProperty.RegisterAttached("UnloadedTaskSource", typeof(TaskCompletionSource<object>), typeof(FrameworkElementHelper), new PropertyMetadata(null));


        public static void RegisterSafeLoadedCallback(this FrameworkElement element, RoutedEventHandler callback)
        {
            element.Loaded += async (sender, e) => 
            {
                var unloadedTaskSource = GetUnloadedTaskSource(element);
                if (unloadedTaskSource?.Task.IsCompleted == false)
                {
                    Debug.WriteLine("SafeLoaded: Waiting was necessary because the element was not yet unloaded.");
                    await unloadedTaskSource.Task;
                }
                SetUnloadedTaskSource(element, new TaskCompletionSource<object>());
                callback(sender, e);
            };
        }

        public static void RegisterSafeUnloadedCallback(this FrameworkElement element, RoutedEventHandler callback)
        {
            element.Unloaded += (sender, e) => 
            {
                callback(sender, e);
                var unloadedTaskSource = GetUnloadedTaskSource(element);
                unloadedTaskSource?.SetResult(null);
            };
        }

        private static TaskCompletionSource<object> GetUnloadedTaskSource(DependencyObject obj) => (TaskCompletionSource<object>)obj.GetValue(UnloadedTaskSourceProperty);
        
        private static void SetUnloadedTaskSource(DependencyObject obj, TaskCompletionSource<object> value) => obj.SetValue(UnloadedTaskSourceProperty, value);
    }
}
