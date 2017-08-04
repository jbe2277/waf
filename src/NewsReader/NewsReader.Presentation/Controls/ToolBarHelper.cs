using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Controls
{
    public enum DynamicToolBarMode
    {
        TopBar,
        BottomBar
    }

    public static class ToolBarHelper
    {
        public static readonly DependencyProperty HideBottomToolBarProperty =
            DependencyProperty.RegisterAttached("HideBottomToolBar", typeof(bool), typeof(ToolBarHelper), new PropertyMetadata(false));

        public static DependencyProperty DynamicToolBarProperty { get; } =
            DependencyProperty.RegisterAttached("DynamicToolBar", typeof(DynamicToolBarMode), typeof(ToolBarHelper), new PropertyMetadata(false, DynamicToolBarPropertyChanged));

        public static DependencyProperty AssociatedToolBarProperty { get; } =
            DependencyProperty.RegisterAttached("AssociatedToolBar", typeof(FrameworkElement), typeof(ToolBarHelper), new PropertyMetadata(null));

        private static readonly AttachedPropertyService<bool> attachedPropertiesService = new AttachedPropertyService<bool>(HideBottomToolBarProperty, HideBottomToolBarPropertyChanged);


        public static bool GetHideBottomToolBar(DependencyObject obj) => (bool)obj.GetValue(HideBottomToolBarProperty);
        
        public static void SetHideBottomToolBar(DependencyObject obj, bool value) => obj.SetValue(HideBottomToolBarProperty, value);
        
        public static DynamicToolBarMode GetDynamicToolBar(DependencyObject obj) => (DynamicToolBarMode)obj.GetValue(DynamicToolBarProperty);
        
        public static void SetDynamicToolBar(DependencyObject obj, DynamicToolBarMode value) => obj.SetValue(DynamicToolBarProperty, value);

        public static FrameworkElement GetAssociatedToolBar(DependencyObject obj) => (FrameworkElement)obj.GetValue(AssociatedToolBarProperty);

        public static void SetAssociatedToolBar(DependencyObject obj, FrameworkElement value) => obj.SetValue(AssociatedToolBarProperty, value);
        
        private static void DynamicToolBarPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is DynamicToolBarMode)
            {
                throw new NotSupportedException("DynamicToolBar property cannot be changed anymore after the property was set.");
            }
            
            var dynamicToolBar = (FrameworkElement)obj;
            attachedPropertiesService.RegisterElement(dynamicToolBar);
            UpdateDynamicToolBar(dynamicToolBar);
        }
        
        private static void HideBottomToolBarPropertyChanged(FrameworkElement element)
        {
            UpdateDynamicToolBar(element);
        }

        private static async void UpdateDynamicToolBar(FrameworkElement dynamicToolBar)
        {
            bool hideBottomToolBar;
            if (attachedPropertiesService.TryGetInheritedValue(dynamicToolBar, out hideBottomToolBar))
            {
                var dynamicToolBarMode = GetDynamicToolBar(dynamicToolBar);
                var associatedToolBar = GetAssociatedToolBar(dynamicToolBar);
                if (associatedToolBar == null)
                {
                    throw new InvalidOperationException("The DynamicToolBar must have the AssociatedToolBar property set.");
                }

                if (hideBottomToolBar && dynamicToolBarMode == DynamicToolBarMode.TopBar
                    || !hideBottomToolBar && dynamicToolBarMode == DynamicToolBarMode.BottomBar)
                {
                    // Workaround: Await idle to solve a rendering issue of the moved toolbar commands (e.g. AppBarButton).
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunIdleAsync(ha => { });
                    associatedToolBar.Visibility = Visibility.Collapsed;
                    MoveCommands((CommandBar)associatedToolBar, (CommandBar)dynamicToolBar);
                    dynamicToolBar.Visibility = Visibility.Visible;
                }
            }
        }

        private static void MoveCommands(CommandBar source, CommandBar target)
        {
            var primaryCommands = source.PrimaryCommands.ToArray();
            var secondaryCommands = source.SecondaryCommands.ToArray();

            source.PrimaryCommands.Clear();
            source.SecondaryCommands.Clear();

            foreach (var command in primaryCommands)
            {
                target.PrimaryCommands.Add(command);
            }
            foreach (var command in secondaryCommands)
            {
                target.SecondaryCommands.Add(command);
            }
        }
    }
}
