using System.ComponentModel;
using System.Waf.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Jbe.NewsReader.Presentation.Controls
{
    public enum SelectionState
    {
        Master,
        MasterDetail,
        ExtendedSelection,
        MultipleSelection
    }

    public interface ISelectionStateManager : INotifyPropertyChanged
    {
        SelectionState SelectionState { get; }

        void CancelMultipleSelectionMode();
    }

    public static class SelectionStateHelper
    {
        public static readonly DependencyProperty IsSinglePageViewSizeProperty =
            DependencyProperty.RegisterAttached("IsSinglePageViewSize", typeof(bool), typeof(SelectionStateHelper), new PropertyMetadata(false));

        private static readonly DependencyProperty AttachedSelectionManagerProperty =
            DependencyProperty.RegisterAttached("AttachedSelectionManager", typeof(SelectionStateManager), typeof(SelectionStateHelper), new PropertyMetadata(null));

        private static readonly AttachedPropertyService<bool> attachedPropertiesService = new AttachedPropertyService<bool>(IsSinglePageViewSizeProperty, UpdateIsSinglePageViewSizeProperty);


        public static bool GetIsSinglePageViewSize(DependencyObject obj) => (bool)obj.GetValue(IsSinglePageViewSizeProperty);

        public static void SetIsSinglePageViewSize(DependencyObject obj, bool value) => obj.SetValue(IsSinglePageViewSizeProperty, value);

        public static ISelectionStateManager CreateManager(ListViewBase listView, ButtonBase selectItemsButton, ButtonBase cancelSelectionButton)
        {
            var selectionManager = new SelectionStateManager(listView, selectItemsButton, cancelSelectionButton);
            listView.SetValue(AttachedSelectionManagerProperty, selectionManager);
            attachedPropertiesService.RegisterElement(listView);
            return selectionManager;
        }
        
        private static void UpdateIsSinglePageViewSizeProperty(FrameworkElement element)
        {
            var selectionManager = (SelectionStateManager)element.GetValue(AttachedSelectionManagerProperty);
            bool isSinglePageViewSize;
            if (attachedPropertiesService.TryGetInheritedValue(element, out isSinglePageViewSize))
            {
                selectionManager.IsSinglePageViewSize = isSinglePageViewSize;
            }
        }

        private class SelectionStateManager : Model, ISelectionStateManager
        {
            private readonly ListViewBase listView;
            private readonly ButtonBase selectItemsButton;
            private readonly ButtonBase cancelSelectionButton;
            private SelectionState selectionState;
            private bool isSinglePageViewSize;
            

            public SelectionStateManager(ListViewBase listView, ButtonBase selectItemsButton, ButtonBase cancelSelectionButton)
            {
                this.listView = listView;
                this.selectItemsButton = selectItemsButton;
                this.cancelSelectionButton = cancelSelectionButton;
                UpdateSelectionStateGroup();
                this.listView.Items.VectorChanged += ItemsCollectionChanged;
                this.listView.SelectionChanged += ListViewSelectionChanged;
                this.selectItemsButton.Click += SelectItemsButtonClick;
                this.cancelSelectionButton.Click += CancelSelectionButtonClick;
            }


            public SelectionState SelectionState
            {
                get { return selectionState; }
                private set
                {
                    if (SetProperty(ref selectionState, value))
                    {
                        // Set SelectionMode last because this might change the current selection and results in reentry of this method.
                        var oldSelectedItem = listView.SelectedItem;
                        if (selectionState == SelectionState.Master)
                        {
                            selectItemsButton.Visibility = Visibility.Visible;
                            cancelSelectionButton.Visibility = Visibility.Collapsed;
                            listView.IsItemClickEnabled = true;
                            listView.SelectionMode = ListViewSelectionMode.None;
                        }
                        else if (selectionState == SelectionState.MasterDetail)
                        {
                            selectItemsButton.Visibility = Visibility.Visible;
                            cancelSelectionButton.Visibility = Visibility.Collapsed;
                            listView.IsItemClickEnabled = false;
                            listView.SelectionMode = ListViewSelectionMode.Extended;
                        }
                        else if (selectionState == SelectionState.ExtendedSelection)
                        {
                            selectItemsButton.Visibility = Visibility.Collapsed;
                            cancelSelectionButton.Visibility = Visibility.Collapsed;
                            listView.IsItemClickEnabled = false;
                            listView.SelectionMode = ListViewSelectionMode.Extended;
                        }
                        else // SelectionState.MultipleSelection
                        {
                            selectItemsButton.Visibility = Visibility.Collapsed;
                            cancelSelectionButton.Visibility = Visibility.Visible;
                            listView.IsItemClickEnabled = false;
                            listView.SelectionMode = ListViewSelectionMode.Multiple;
                        }
                        if (listView.SelectedItem == null) listView.SelectedItem = oldSelectedItem;
                    }
                }
            }

            internal bool IsSinglePageViewSize
            {
                get { return isSinglePageViewSize; }
                set
                {
                    if (isSinglePageViewSize == value) return;
                    isSinglePageViewSize = value;
                    UpdateSelectionStateGroup();
                }
            }


            public void CancelMultipleSelectionMode() => UpdateSelectionStateGroup(true);

            private void ItemsCollectionChanged(IObservableVector<object> sender, IVectorChangedEventArgs e)
            {
                selectItemsButton.IsEnabled = listView.Items.Count > 0;
            }

            private void ListViewSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateSelectionStateGroup();

            private void SelectItemsButtonClick(object sender, RoutedEventArgs e)
            {
                if (listView.Items.Count > 0)
                {
                    SelectionState = SelectionState.MultipleSelection;
                }
            }

            private void CancelSelectionButtonClick(object sender, RoutedEventArgs e) => CancelMultipleSelectionMode();

            private void UpdateSelectionStateGroup(bool cancelSelectionState = false)
            {
                if (!cancelSelectionState && SelectionState == SelectionState.MultipleSelection) { return; }

                if (IsSinglePageViewSize)
                {
                    SelectionState = SelectionState.Master;
                }
                else
                {
                    if (listView.SelectedItems.Count <= 1)
                    {
                        SelectionState = SelectionState.MasterDetail;
                    }
                    else
                    {
                        SelectionState = SelectionState.ExtendedSelection;
                    }
                }
            }
        }
    }
}
