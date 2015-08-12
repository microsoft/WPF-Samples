// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace NumbersMultipleLangauges
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var currentLanguage =
                CultureInfo.CurrentCulture.IetfLanguageTag;

            text1.Language = XmlLanguage.GetLanguage(currentLanguage);

            text1.FlowDirection = currentLanguage.ToLower().StartsWith("ar")
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;
        }
    }
}