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
    public static class SelectionBehavior
    {
        private static readonly List<Tuple<IMultiSelector, INotifyCollectionChanged>> multiSelectorWithObservableList = new List<Tuple<IMultiSelector, INotifyCollectionChanged>>();
        private static readonly HashSet<object> syncListsThatAreUpdating = new HashSet<object>();
        private static readonly  HashSet<Selector> selectorsThatAreUpdating = new HashSet<Selector>();


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
                multiSelector.SelectionModeChanged += MultiSelectorSelectionModeChanged;
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
            item.Item1.SelectionModeChanged -= MultiSelectorSelectionModeChanged;
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
            UpdateSelectedItemsModel((Selector)sender);
        }

        private static void MultiSelectorSelectionModeChanged(object sender, EventArgs e)
        {
            UpdateSelectedItemsModel(((IMultiSelector)sender).Selector);
        }

        private static void UpdateSelectedItemsModel(Selector selector)
        {
            if (selectorsThatAreUpdating.Contains(selector)) { return; }

            var list = GetSyncSelectedItems(selector);
            if (list == null) { return; }

            var multiSelector = multiSelectorWithObservableList.First(x => x.Item1.Selector == selector).Item1;

            syncListsThatAreUpdating.Add(list);
            try
            {
                var genericList = list.OfType<object>().ToArray();
                foreach (var item in genericList.Except(multiSelector.SelectedItems))
                {
                    list.Remove(item);
                }
                foreach (var item in multiSelector.SelectedItems.Except(genericList))
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
            if (selector is ListViewBase listViewBase) return new ListViewAdapter(listViewBase);
            if (selector is ListBox listBox) return new ListBoxAdapter(listBox);
            return null;
        }

        private interface IMultiSelector
        {
            Selector Selector { get; }

            IList<object> SelectedItems { get; }

            ListViewSelectionMode SelectionMode { get; }

            event EventHandler SelectionModeChanged;
        }

        private class ListViewAdapter : IMultiSelector
        {
            private readonly ListViewBase listView;

            public ListViewAdapter(ListViewBase listView)
            {
                this.listView = listView;
                listView.RegisterPropertyChangedCallback(ListViewBase.SelectionModeProperty, (sender, dp) => SelectionModeChanged?.Invoke(this, EventArgs.Empty));
            }
            
            public Selector Selector => listView;

            public IList<object> SelectedItems => listView.SelectedItems;

            public ListViewSelectionMode SelectionMode => listView.SelectionMode;

            public event EventHandler SelectionModeChanged;
        }

        private class ListBoxAdapter : IMultiSelector
        {
            private readonly ListBox listBox;

            public ListBoxAdapter(ListBox listBox)
            {
                this.listBox = listBox;
                listBox.RegisterPropertyChangedCallback(ListBox.SelectionModeProperty, (sender, dp) => SelectionModeChanged?.Invoke(this, EventArgs.Empty));
            }

            public Selector Selector => listBox;

            public IList<object> SelectedItems => listBox.SelectedItems;

            public ListViewSelectionMode SelectionMode => (ListViewSelectionMode)(listBox.SelectionMode + 1);

            public event EventHandler SelectionModeChanged;
        }
    }
}
