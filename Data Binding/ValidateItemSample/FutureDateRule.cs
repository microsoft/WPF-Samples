// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Controls;

namespace ValidateItemSample
{
    internal class FutureDateRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime date;
            try
            {
                date = DateTime.Parse(value?.ToString());
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Value is not a valid date.");
            }
            return DateTime.Now.Date > date
                ? new ValidationResult(false, "Please enter a date in the future.")
                : ValidationResult.ValidResult;
        }
    }
}