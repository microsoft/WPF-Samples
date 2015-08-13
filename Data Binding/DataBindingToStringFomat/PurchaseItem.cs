// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DataBindingToStringFomat
{
    public class PurchaseItem
    {
        public PurchaseItem()
        {
        }

        public PurchaseItem(string desc, double price, DateTime endDate)
        {
            Description = desc;
            Price = price;
            OfferExpires = endDate;
        }

        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime OfferExpires { get; set; }

        public override string ToString() => $"{Description}, {Price:c}, {OfferExpires:D}";
    }
}