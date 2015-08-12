// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace DrawingVisual
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void WindowLoaded(object sender, EventArgs e)
        {
            var visualHost = new MyVisualHost();
            MyCanvas.Children.Add(visualHost);
        }
    }

    // Create a host visual derived from the FrameworkElement class.
    // This class provides layout, event handling, and container support for
    // the child visual objects.
}