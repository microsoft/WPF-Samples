// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace Brushes
{
    /// <summary>
    ///     Implements the show/hide gradient stops functionlity for
    ///     GradientBrushesExample.xaml.
    /// </summary>
    public partial class GradientBrushesExample : Page
    {
        public GradientBrushesExample()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs args)
        {
            showHideGradientStopsCheckBox.Checked += ShowGradientStops;
            showHideGradientStopsCheckBox.Unchecked += HideGradientStops;
        }

        private void ShowGradientStops(object sender, RoutedEventArgs args)
        {
            gradientLine1.Opacity = 1.0;
            gradientLine2.Opacity = 1.0;
            gradientLine3.Opacity = 1.0;
            gradientLine4.Opacity = 1.0;
            gradientLine5.Opacity = 1.0;
            gradientPath1.Opacity = 1.0;
            gradientPath2.Opacity = 1.0;
            gradientPath3.Opacity = 1.0;
            gradientPath4.Opacity = 1.0;
            gradientPath5.Opacity = 1.0;
        }

        private void HideGradientStops(object sender, RoutedEventArgs args)
        {
            gradientLine1.Opacity = 0.0;
            gradientLine2.Opacity = 0.0;
            gradientLine3.Opacity = 0.0;
            gradientLine4.Opacity = 0.0;
            gradientLine5.Opacity = 0.0;
            gradientPath1.Opacity = 0.0;
            gradientPath2.Opacity = 0.0;
            gradientPath3.Opacity = 0.0;
            gradientPath4.Opacity = 0.0;
            gradientPath5.Opacity = 0.0;
        }
    }
}