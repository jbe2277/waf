using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace Waf.BookLibrary.Reporting.Presentation.Controls
{
    public class ItemsElement : Section
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ItemsElement), new UIPropertyMetadata(null, ItemsSourceChangedHandler));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ItemsElement), new UIPropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        private void UpdateContent()
        {
            if (ItemsSource != null)
            {
                if (ItemTemplate == null)
                {
                    throw new InvalidOperationException("When ItemsSource is used then the ItemTemplate must not be null.");
                }
                var blocks = new List<Block>();
                foreach (object? item in ItemsSource)
                {
                    Block? block = null;
                    if (ItemTemplate.LoadContent() is ContentElement contentElement)
                    {
                        block = contentElement.Content as Block;
                    }
                    if (block == null)
                    {
                        throw new InvalidOperationException("The ItemTemplate must define: DataTemplate > ContentElement > Block element.");
                    }
                    block.DataContext = item;
                    blocks.Add(block);
                }
                Blocks.AddRange(blocks);
            }
        }

        private static void ItemsSourceChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsElement)d).UpdateContent();
        }
    }
}
