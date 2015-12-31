using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Waf.BookLibrary.Reporting.Presentation.Controls
{
    public class BindableTable : Table
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(BindableTable), new UIPropertyMetadata(null, ItemsSourceChangedHandler));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(BindableTable), new UIPropertyMetadata(null));

        public static readonly DependencyProperty HeaderRowProperty =
            DependencyProperty.Register(nameof(HeaderRowGroup), typeof(TableRowGroup), typeof(BindableTable), new UIPropertyMetadata(null));

        public static readonly DependencyProperty FooterRowProperty =
            DependencyProperty.Register(nameof(FooterRowGroup), typeof(TableRowGroup), typeof(BindableTable), new UIPropertyMetadata(null));


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public TableRowGroup HeaderRowGroup
        {
            get { return (TableRowGroup)GetValue(HeaderRowProperty); }
            set { SetValue(HeaderRowProperty, value); }
        }

        public TableRowGroup FooterRowGroup
        {
            get { return (TableRowGroup)GetValue(FooterRowProperty); }
            set { SetValue(FooterRowProperty, value); }
        }


        private void UpdateContent()
        {
            RowGroups.Clear();

            if (HeaderRowGroup != null) { RowGroups.Add(HeaderRowGroup); }
            
            if (ItemsSource != null)
            {
                if (ItemTemplate == null) 
                { 
                    throw new InvalidOperationException("When ItemsSource is used then the ItemTemplate must not be null."); 
                }

                TableRowGroup contentRowGroup = new TableRowGroup();
                foreach (object item in ItemsSource)
                {
                    TableRow tableRow = null;
                    ContentElement contentElement = ItemTemplate.LoadContent() as ContentElement;
                    if (contentElement != null)
                    {
                        tableRow = contentElement.Content as TableRow;
                    }
                    
                    if (tableRow == null)
                    {
                        throw new InvalidOperationException("The ItemTemplate must define: DataTemplate > ContentElement > TableRow.");
                    }
                    tableRow.DataContext = item;
                    contentRowGroup.Rows.Add(tableRow);
                }

                if (contentRowGroup.Rows.Any()) { RowGroups.Add(contentRowGroup); }
            }
            
            if (FooterRowGroup != null) { RowGroups.Add(FooterRowGroup); }
        }

        private static void ItemsSourceChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindableTable table = (BindableTable)d;
            table.UpdateContent();
        }
    }
}
