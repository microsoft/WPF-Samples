// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

// ValidationRule

// CultureInfo

namespace ExpenseItDemo.Validation
{
    // Email Validation Rule
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Is a valid email address?
            var pattern =
                @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*";
            if (!Regex.IsMatch((string) value, pattern))
            {
                var msg = $"{value} is not a valid email address.";
                return new ValidationResult(false, msg);
            }

            // Email address is valid
            return new ValidationResult(true, null);
        }
    }
}