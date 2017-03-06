using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Jbe.NewsReader.Presentation.Controls
{
    // Provides a property value inheritance feature. Note: The element that contains the attached property value is resolved just once.
    // All further calls to TryGetInheritedValue returns the value of this first resolved object.
    internal class AttachedPropertyService<T>
    {
        private readonly DependencyProperty attachedProperty;
        private readonly Action<FrameworkElement> updateInheritedValueCallback;
        private readonly Dictionary<FrameworkElement, DependencyObject> objectWithValues = new Dictionary<FrameworkElement, DependencyObject>();
        private readonly Dictionary<DependencyObject, long> objectWithValueHandles = new Dictionary<DependencyObject, long>();


        public AttachedPropertyService(DependencyProperty attachedProperty, Action<FrameworkElement> updateInheritedValueCallback)
        {
            if (attachedProperty == null) throw new ArgumentNullException(nameof(attachedProperty));
            if (updateInheritedValueCallback == null) throw new ArgumentNullException(nameof(updateInheritedValueCallback));
            this.attachedProperty = attachedProperty;
            this.updateInheritedValueCallback = updateInheritedValueCallback;
        }


        public void RegisterElement(FrameworkElement element)
        {
            element.RegisterSafeLoadedCallback(ElementLoaded);
            element.RegisterSafeUnloadedCallback(ElementUnloaded);
            T value;
            TryGetInheritedValue(element, out value);  // This method registers the element
        }

        public bool TryGetInheritedValue(FrameworkElement element, out T value)
        {
            DependencyObject objectWithValue = null;
            objectWithValues.TryGetValue(element, out objectWithValue);
            if (objectWithValue == null)
            {
                objectWithValue = FindAncestorWithDependencyProperty(element, attachedProperty);
                objectWithValues[element] = objectWithValue;
            }
            if (objectWithValue != null)
            {
                if (!objectWithValueHandles.ContainsKey(objectWithValue))
                {
                    objectWithValueHandles.Add(objectWithValue, objectWithValue.RegisterPropertyChangedCallback(attachedProperty, InheritedPropertyChanged));
                }
                value = (T)objectWithValue.GetValue(attachedProperty);
                return true;
            }
            value = default(T);
            return false;
        }

        private void ElementLoaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            T value;
            TryGetInheritedValue(element, out value);  // This method registers the element
            updateInheritedValueCallback(element);     // Maybe the value has change as long the element was unloaded
        }

        private void ElementUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var objectWithValue = objectWithValues[element];
            long handle;
            if (objectWithValueHandles.TryGetValue(objectWithValue, out handle)
                && objectWithValues.Count(x => x.Value == objectWithValue) == 1)
            {
                objectWithValue.UnregisterPropertyChangedCallback(attachedProperty, handle);
                objectWithValueHandles.Remove(objectWithValue);
            }
            objectWithValues.Remove(element);
        }

        private void InheritedPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            foreach (var item in objectWithValues.Where(x => x.Value == sender))
            {
                updateInheritedValueCallback(item.Key);
            }
        }

        private static DependencyObject FindAncestorWithDependencyProperty(DependencyObject startObj, DependencyProperty dp)
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
