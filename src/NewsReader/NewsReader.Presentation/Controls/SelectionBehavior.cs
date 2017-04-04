using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Jbe.NewsReader.Presentation.Controls
{
    // TODO: Crash when deleting multiple feeds
    public static class SelectionBehavior
    {
        private static List<Tuple<IMultiSelector, INotifyCollectionChanged>> multiSelectorWithObservableList = new List<Tuple<IMultiSelector, INotifyCollectionChanged>>();
        private static HashSet<object> syncListsThatAreUpdating = new HashSet<object>();
        private static HashSet<Selector> selectorsThatAreUpdating = new HashSet<Selector>();


        public static DependencyProperty SyncSelectedItemsProperty { get; } =
            DependencyProperty.RegisterAttached("SyncSelectedItems", typeof(IList), typeof(SelectionBehavior), new PropertyMetadata(null, SyncSelectedItemsPropertyChanged));


        public static IList GetSyncSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SyncSelectedItemsProperty);
        }

        public static void SetSyncSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SyncSelectedItemsProperty, value);
        }

        private static void SyncSelectedItemsPropertyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            Selector selector = element as Selector;
            if (selector == null)
            {
                throw new ArgumentException("The attached property SelectedItems can only be used with a Selector.", nameof(element));
            }
            TryCleanUpOldItem(selector);
            try
            {
                IMultiSelector multiSelector = TryGetMultiSelector(selector);
                if (multiSelector == null) { return; }

                var list = GetSyncSelectedItems(selector);
                if (list == null) { return; }

                if (multiSelector.SelectedItems.Count > 0) { multiSelector.SelectedItems.Clear(); }
                foreach (var item in list)
                {
                    multiSelector.SelectedItems.Add(item);
                }

                var observableList = list as INotifyCollectionChanged;
                if (observableList == null) { return; }

                multiSelectorWithObservableList.Add(new Tuple<IMultiSelector, INotifyCollectionChanged>(multiSelector, observableList));
                observableList.CollectionChanged += ListCollectionChanged;
            }
            finally
            {
                selector.SelectionChanged += SelectorSelectionChanged;
            }
        }

        private static void TryCleanUpOldItem(Selector selector)
        {
            selector.SelectionChanged -= SelectorSelectionChanged;  // Remove a previously added event handler.

            var item = multiSelectorWithObservableList.FirstOrDefault(x => x.Item1.Selector == selector);
            if (item == null) { return; }

            multiSelectorWithObservableList.Remove(item);
            item.Item2.CollectionChanged -= ListCollectionChanged;
        }


        private static void ListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (syncListsThatAreUpdating.Contains(sender)) return;

            var multiSelector = multiSelectorWithObservableList.First(x => x.Item2 == sender).Item1;
            if (multiSelector.SelectionMode == ListViewSelectionMode.None) return;

            selectorsThatAreUpdating.Add(multiSelector.Selector);
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var items in e.NewItems)
                    {
                        multiSelector.SelectedItems.Add(items);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var items in e.OldItems)
                    {
                        multiSelector.SelectedItems.Remove(items);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    multiSelector.SelectedItems.Clear();
                    foreach (var item in (IEnumerable)sender)
                    {
                        multiSelector.SelectedItems.Add(item);
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            finally
            {
                selectorsThatAreUpdating.Remove(multiSelector.Selector);
            }
        }

        private static void SelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = (Selector)sender;
            if (selectorsThatAreUpdating.Contains(selector)) { return; }

            var list = GetSyncSelectedItems(selector);
            if (list == null) { return; }

            syncListsThatAreUpdating.Add(list);
            try
            {
                foreach (var item in e.RemovedItems)
                {
                    list.Remove(item);
                }
                foreach (var item in e.AddedItems)
                {
                    list.Add(item);
                }
            }
            finally
            {
                syncListsThatAreUpdating.Remove(list);
            }
        }

        private static IMultiSelector TryGetMultiSelector(Selector selector)
        {
            if (selector is ListViewBase) { return new ListViewAdapter((ListViewBase)selector); }
            if (selector is ListBox) { return new ListBoxAdapter((ListBox)selector); }
            return null;
        }

        private interface IMultiSelector
        {
            Selector Selector { get; }

            IList<object> SelectedItems { get; }

            ListViewSelectionMode SelectionMode { get; }
        }

        private class ListViewAdapter : IMultiSelector
        {
            private readonly ListViewBase listView;

            public ListViewAdapter(ListViewBase listView)
            {
                this.listView = listView;
            }

            public Selector Selector => listView;

            public IList<object> SelectedItems => listView.SelectedItems;

            public ListViewSelectionMode SelectionMode => listView.SelectionMode;
        }

        private class ListBoxAdapter : IMultiSelector
        {
            private readonly ListBox listBox;

            public ListBoxAdapter(ListBox listBox)
            {
                this.listBox = listBox;
            }

            public Selector Selector => listBox;

            public IList<object> SelectedItems => listBox.SelectedItems;

            public ListViewSelectionMode SelectionMode => (ListViewSelectionMode)(listBox.SelectionMode + 1);
        }
    }
}
