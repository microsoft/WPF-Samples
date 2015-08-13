// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace ContextMenuOpening
{
    public class MyButton : Button
    {
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);
            var buttonMenu = new ContextMenu();
            var mia = new MenuItem {Header = "Item1"};
            var mib = new MenuItem {Header = "Item2"};
            var mic = new MenuItem {Header = "Item3"};
            buttonMenu.Items.Add(mia);
            buttonMenu.Items.Add(mib);
            buttonMenu.Items.Add(mic);
            var fe = e.Source as FrameworkElement;
            fe.ContextMenu = buttonMenu;
        }
    }
}