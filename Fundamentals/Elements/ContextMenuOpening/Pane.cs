// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ContextMenuOpening
{
    public partial class Pane
    {
        private bool _flagForCustomContextMenu;

        private ContextMenu BuildMenu()
        {
            var theMenu = new ContextMenu();
            var mia = new MenuItem {Header = "Item1"};
            var mib = new MenuItem {Header = "Item2"};
            var mic = new MenuItem {Header = "Item3"};
            theMenu.Items.Add(mia);
            theMenu.Items.Add(mib);
            theMenu.Items.Add(mic);
            return theMenu;
        }

        private void AddItemToCm(object sender, ContextMenuEventArgs e)
        {
            //check if Item4 is already there, this will probably run more than once
            var fe = e.Source as FrameworkElement;
            var cm = fe.ContextMenu;
            if (cm.Items.Cast<MenuItem>().Any(mi => (string) mi.Header == "Item4"))
            {
                return;
            }
            var mi4 = new MenuItem {Header = "Item4"};
            fe.ContextMenu.Items.Add(mi4);
        }

        private void HandlerForCMO(object sender, ContextMenuEventArgs e)
        {
            var fe = e.Source as FrameworkElement;
            fe.ContextMenu = BuildMenu();
        }

        private void HandlerForCMO2(object sender, ContextMenuEventArgs e)
        {
            if (!_flagForCustomContextMenu)
            {
                e.Handled = true; //need to suppress empty menu
                var fe = e.Source as FrameworkElement;
                fe.ContextMenu = BuildMenu();
                _flagForCustomContextMenu = true;
                fe.ContextMenu.IsOpen = true;
            }
        }
    }
}