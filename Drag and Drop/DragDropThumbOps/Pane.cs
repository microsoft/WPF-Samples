// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DragDropThumbOps
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Pane : Canvas
    {
        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            //Move the Thumb to the mouse position during the drag operation
            var yadjust = myCanvasStretch.Height + e.VerticalChange;
            var xadjust = myCanvasStretch.Width + e.HorizontalChange;
            if ((xadjust >= 0) && (yadjust >= 0))
            {
                myCanvasStretch.Width = xadjust;
                myCanvasStretch.Height = yadjust;
                SetLeft(myThumb, GetLeft(myThumb) +
                                 e.HorizontalChange);
                SetTop(myThumb, GetTop(myThumb) +
                                e.VerticalChange);
                changes.Text = "Size: " +
                               myCanvasStretch.Width.ToString(CultureInfo.InvariantCulture) +
                               ", " +
                               myCanvasStretch.Height.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            myThumb.Background = Brushes.Orange;
        }

        private void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            myThumb.Background = Brushes.Blue;
        }
    }
}