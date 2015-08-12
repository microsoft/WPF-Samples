// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows.Controls;

namespace DialogBox
{
    public class MarginValidationRule : ValidationRule
    {
        public double MinMargin { get; set; }
        public double MaxMargin { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double margin;

            // Is a number?
            if (!double.TryParse((string) value, out margin))
            {
                return new ValidationResult(false, "Not a number.");
            }

            // Is in range?
            if ((margin < MinMargin) || (margin > MaxMargin))
            {
                var msg = $"Margin must be between {MinMargin} and {MaxMargin}.";
                return new ValidationResult(false, msg);
            }

            // Number is valid
            return new ValidationResult(true, null);
        }
    }
}

//</SnippetMarginValidationRuleCODE>