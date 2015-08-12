// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace AnnotationsStyling
{
    /// <summary>
    ///     Provides a simple wrapper that associates a
    ///     Style with its ResourceDictionary key.
    /// </summary>
    public class StyleMetaData : DependencyObject
    {
        public static DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof (string), typeof (StyleMetaData));

        public Style Value;

        public StyleMetaData(string key, Style value)
        {
            Key = key;
            Value = value;
        }

        public string Key
        {
            get { return (string) GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
    }
}