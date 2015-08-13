// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace LocalAnimations
{
    public class SampleViewer : Window
    {
        public SampleViewer()
        {
            var tControl = new TabControl();
            var tItem = new TabItem {Header = "Local Animation Example"};
            var contentFrame = new Frame {Content = new LocalAnimationExample()};
            tItem.Content = contentFrame;
            tControl.Items.Add(tItem);
            tItem = new TabItem {Header = "Interactive Animation Example"};
            contentFrame = new Frame {Content = new InteractiveAnimationExample()};
            tItem.Content = contentFrame;
            tControl.Items.Add(tItem);

            Content = tControl;
            Title = "Local Animations Example";
        }
    }
}