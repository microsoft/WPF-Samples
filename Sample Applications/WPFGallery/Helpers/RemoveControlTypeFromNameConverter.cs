// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Text.RegularExpressions;

namespace WPFGallery.Helpers;

/// <summary>
/// Builds an accessible Name that does not include the element's own control type.
/// The whole-word control type (supplied via the converter parameter) is removed from
/// the source string, matching the whole-word, case-insensitive check performed by the
/// Accessibility Insights / axe-windows "NameExcludesLocalizedControlType" rule.
/// <para>
/// If removing the control type would leave an empty string (e.g. the header was only the
/// control type), the original value is returned unchanged so the Name is never blank.
/// </para>
/// </summary>
internal sealed class RemoveControlTypeFromNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        string text = value as string ?? string.Empty;

        if (string.IsNullOrWhiteSpace(text) || parameter is not string controlType || string.IsNullOrWhiteSpace(controlType))
        {
            return text;
        }

        string stripped = Regex.Replace(text, $@"\b{Regex.Escape(controlType)}\b", string.Empty, RegexOptions.IgnoreCase);
        stripped = Regex.Replace(stripped, @"\s+", " ").Trim();

        return string.IsNullOrEmpty(stripped) ? text : stripped;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
