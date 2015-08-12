// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;

namespace KeyFrameAnimation
{
    public partial class KeyFramesIntroduction : Page
    {
        public void PageLoaded(object sender, RoutedEventArgs args)
        {
            myVisualBrush.Visual = myImage;
        }

        private void ExampleCanvasLayoutUpdated(object sender, EventArgs args)
        {
            myVisualBrush.Viewbox =
                new Rect(
                    Canvas.GetLeft(myRectangle),
                    Canvas.GetTop(myRectangle),
                    myRectangle.ActualWidth,
                    myRectangle.ActualHeight
                    );
        }
    }
}