using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Jbe.NewsReader.Presentation.Controls
{
    public enum DynamicToolBarMode
    {
        TopBar,
        BottomBar
    }

    public static class ToolBarHelper
    {
        private static readonly Dictionary<FrameworkElement, DependencyObject> objectWithValues = new Dictionary<FrameworkElement, DependencyObject>();
        private static readonly Dictionary<DependencyObject, long> objectWithValueHandles = new Dictionary<DependencyObject, long>();


        public static readonly DependencyProperty HideBottomToolBarProperty =
            DependencyProperty.RegisterAttached("HideBottomToolBar", typeof(bool), typeof(ToolBarHelper), new PropertyMetadata(false));

        public static DependencyProperty DynamicToolBarProperty { get; } =
            DependencyProperty.RegisterAttached("DynamicToolBar", typeof(DynamicToolBarMode), typeof(ToolBarHelper), new PropertyMetadata(false, DynamicToolBarPropertyChanged));

        public static DependencyProperty AssociatedToolBarProperty { get; } =
            DependencyProperty.RegisterAttached("AssociatedToolBar", typeof(FrameworkElement), typeof(ToolBarHelper), new PropertyMetadata(null));
        

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
            UpdateDynamicToolBar(dynamicToolBar);
            dynamicToolBar.RegisterSafeLoadedCallback(DynamicToolBarLoaded);
            dynamicToolBar.RegisterSafeUnloadedCallback(DynamicToolBarUnloaded);
        }
        
        private static void DynamicToolBarLoaded(object sender, RoutedEventArgs e)
        {
            UpdateDynamicToolBar((FrameworkElement)sender);
        }

        private static void DynamicToolBarUnloaded(object sender, RoutedEventArgs e)
        {
            var dynamicToolBar = (FrameworkElement)sender;
            var objectWithValue = objectWithValues[dynamicToolBar];
            long handle;
            if (objectWithValueHandles.TryGetValue(objectWithValue, out handle)
                && objectWithValues.Count(x => x.Value == objectWithValue) == 1)
            {
                objectWithValue.UnregisterPropertyChangedCallback(HideBottomToolBarProperty, handle);
                objectWithValueHandles.Remove(objectWithValue);
            }
            objectWithValues.Remove(dynamicToolBar);
        }
        
        private static void HideBottomToolBarPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            foreach (var item in objectWithValues.Where(x => x.Value == sender))
            {
                UpdateDynamicToolBar(item.Key);
            }
        }

        private static void UpdateDynamicToolBar(FrameworkElement dynamicToolBar)
        {
            DependencyObject objectWithValue = null;
            objectWithValues.TryGetValue(dynamicToolBar, out objectWithValue);
            if (objectWithValue == null)
            {
                objectWithValue = FindObjectWithDependencyProperty(dynamicToolBar, HideBottomToolBarProperty);
                objectWithValues[dynamicToolBar] = objectWithValue;
            }
            if (objectWithValue != null)
            {
                if (!objectWithValueHandles.ContainsKey(objectWithValue))
                {
                    objectWithValueHandles.Add(objectWithValue, objectWithValue.RegisterPropertyChangedCallback(HideBottomToolBarProperty, HideBottomToolBarPropertyChanged));
                }

                var dynamicToolBarMode = GetDynamicToolBar(dynamicToolBar);
                var hideBottomToolBar = GetHideBottomToolBar(objectWithValue);
                var associatedToolBar = GetAssociatedToolBar(dynamicToolBar);
                if (associatedToolBar == null)
                {
                    throw new InvalidOperationException("The DynamicToolBar must have the AssociatedToolBar property set.");
                }

                if (hideBottomToolBar && dynamicToolBarMode == DynamicToolBarMode.TopBar
                    || !hideBottomToolBar && dynamicToolBarMode == DynamicToolBarMode.BottomBar)
                {
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

        private static DependencyObject FindObjectWithDependencyProperty(DependencyObject startObj, DependencyProperty dp)
        {
            var currentObj = startObj;
            while (currentObj != null)
            {
                var propertyValue = currentObj.ReadLocalValue(dp);
                if (propertyValue != DependencyProperty.UnsetValue)
                {
                    return currentObj;
                }

                currentObj = VisualTreeHelper.GetParent(currentObj);
            }
            return null;
        }
    }
}
