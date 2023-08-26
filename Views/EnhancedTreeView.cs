using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TripleKill.Views
{
    public class EnhancedTreeView : TreeView
    {
        public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register("CurrentItem", typeof(object), typeof(EnhancedTreeView), new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true
        });

        public object CurrentItem
        {
            get => GetValue(CurrentItemProperty);
            set => SetValue(CurrentItemProperty, value);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            UpdateLayout();
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            CurrentItem = SelectedItem;
        }
    }
}
