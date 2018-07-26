using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Waf.BookLibrary.Library.Presentation.Controls
{
    public static class DataGridHelper
    {
        public static Func<IEnumerable<T>, IOrderedEnumerable<T>> HandleDataGridSorting<T>(DataGridSortingEventArgs e, IDictionary<string, Func<T, IComparable>> sortSelectors)
        {
            e.Handled = true;
            if (e.Column.SortDirection == null)
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
            }
            else if (e.Column.SortDirection == ListSortDirection.Ascending)
            {
                e.Column.SortDirection = ListSortDirection.Descending;
            }
            else
            {
                e.Column.SortDirection = null;
            }
            return GetSorting(e.Column, sortSelectors);
        }

        public static Func<IEnumerable<T>, IOrderedEnumerable<T>> GetSorting<T>(DataGridColumn column, IDictionary<string, Func<T, IComparable>> sortSelectors)
        {
            var keySelector = sortSelectors[column.SortMemberPath];
            if (column.SortDirection == ListSortDirection.Ascending)
            {
                return x => x.OrderBy(keySelector);
            }
            else if (column.SortDirection == ListSortDirection.Descending)
            {
                return x => x.OrderByDescending(keySelector);
            }
            else
            {
                return null;
            }
        }
    }
}
