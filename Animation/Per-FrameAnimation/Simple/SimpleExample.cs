// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace PerFrameAnimation
{
    /// <summary>
    ///     Interaction logic for Page1.xaml
    /// </summary>
    public partial class SimpleExample : Page
    {
        private readonly Random _rand = new Random();

        public SimpleExample()
        {
            CompositionTarget.Rendering += UpdateColor;
        }

        private void UpdateColor(object sender, EventArgs e)
        {
            // Set a random color
            animatedBrush.Color = Color.FromRgb((byte) _rand.Next(255),
                (byte) _rand.Next(255),
                (byte) _rand.Next(255));
        }
    }
}