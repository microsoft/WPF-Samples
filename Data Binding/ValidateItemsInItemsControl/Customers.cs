// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

namespace ValidateItemsInItemsControl
{
    public class Customers : ObservableCollection<Customer>
    {
        public Customers()
        {
            Add(new Customer());
        }
    }

    // Check whether the customer and service representative are in the
    // same area.
}