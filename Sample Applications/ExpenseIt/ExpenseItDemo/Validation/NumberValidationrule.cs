// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows.Controls;

// ValidationRule

// CultureInfo

namespace ExpenseItDemo.Validation
{
    // Number Validation Rule
    public class NumberValidationRule : ValidationRule
    {
        /// <summary>
        ///     Must the number be a specific length
        /// </summary>
        public bool IsFixedLength { get; set; }

        /// <summary>
        ///     The required length of the number, if IsFixedLength is true
        /// </summary>
        public int Length { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Is the value a number?
            int number;
            if (!int.TryParse((string) value, out number))
            {
                var msg = $"{value} is not a number.";
                return new ValidationResult(false, msg);
            }

            // Does value contain the number of digits specified by Length?
            if (IsFixedLength && (((string) value).Length != Length))
            {
                var msg = $"Number must be {Length} digits long.";
                return new ValidationResult(false, msg);
            }

            // Number is valid
            return new ValidationResult(true, null);
        }
    }
}