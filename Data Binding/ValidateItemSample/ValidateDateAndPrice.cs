// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ValidateItemSample
{
    public class ValidateDateAndPrice : ValidationRule
    {
        // Ensure that an item over $100 is available for at least 7 days.
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var bg = value as BindingGroup;

            // Get the source object.
            var item = bg?.Items[0] as PurchaseItem;

            object doubleValue;
            object dateTimeValue;

            // Get the proposed values for Price and OfferExpires.
            var priceResult = bg.TryGetValue(item, "Price", out doubleValue);
            var dateResult = bg.TryGetValue(item, "OfferExpires", out dateTimeValue);

            if (!priceResult || !dateResult)
            {
                return new ValidationResult(false, "Properties not found");
            }

            var price = (double) doubleValue;
            var offerExpires = (DateTime) dateTimeValue;

            // Check that an item over $100 is available for at least 7 days.
            if (price > 100)
            {
                if (offerExpires < DateTime.Today + new TimeSpan(7, 0, 0, 0))
                {
                    return new ValidationResult(false, "Items over $100 must be available for at least 7 days.");
                }
            }

            return ValidationResult.ValidResult;
        }
    }


    //Ensure that the price is positive.

    // Ensure that the date is in the future.

    // PurchaseItem implements INotifyPropertyChanged and IEditableObject
    // to support edit transactions, which enable users to cancel pending changes.
}