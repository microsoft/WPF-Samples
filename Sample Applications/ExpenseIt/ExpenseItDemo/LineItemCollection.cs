// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ExpenseItDemo
{
    public class LineItemCollection : ObservableCollection<LineItem>
    {
        public event EventHandler LineItemCostChanged;

        public LineItemCollection()
        {
            CollectionChanged += OnListItemsChanged;
        }

        private void LineItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost")
            {
                OnLineItemCostChanged(this, new EventArgs());
            }
        }

        private void OnListItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                for(int i = 0; i < e.NewItems.Count; i++)
                {
                    LineItem item = e.NewItems[i] as LineItem;
                    item.PropertyChanged += LineItemPropertyChanged;
                }
            }
        }

        private void OnLineItemCostChanged(object sender, EventArgs args)
        {
            LineItemCostChanged?.Invoke(sender, args);
        }
    }
}