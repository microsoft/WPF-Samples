// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;

namespace LayoutTransitionsDemo
{
    public class LayoutToLayoutTarget : Border
    {
        /*
         * Create a dependency property for binding the host to the target
         * */

        public static readonly DependencyProperty HostProperty =
            DependencyProperty.Register("Host", typeof (LayoutToLayoutHost), typeof (LayoutToLayoutTarget), null);

        /*
         * If the layout changes for the target (the object that actually gets passed between layout schemes)
         * then let the host know so that it can update itself if it wants to (i.e. if it's not in the middle of
         * an animation).
         * */

        public LayoutToLayoutTarget()
        {
            LayoutUpdated += UpdateHost;
        }

        public LayoutToLayoutHost Host
        {
            get { return (LayoutToLayoutHost) GetValue(HostProperty); }
            set { SetValue(HostProperty, value); }
        }

        private void UpdateHost(object sender, EventArgs e)
        {
            Host?.UpdateFromTarget();
        }
    }
}