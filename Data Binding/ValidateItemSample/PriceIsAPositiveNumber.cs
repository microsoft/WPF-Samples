// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Controls;

namespace ValidateItemSample
{
    public class PriceIsAPositiveNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var price = Convert.ToDouble(value);

                if (price < 0)
                {
                    return new ValidationResult(false, "Price must be positive.");
                }
                return ValidationResult.ValidResult;
            }
            catch (Exception)
            {
                // Exception thrown by Conversion - value is not a number.
                return new ValidationResult(false, "Price must be a number.");
            }
        }
    }
}