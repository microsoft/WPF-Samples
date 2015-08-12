// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace FlowDocumentNewsClient
{
    public partial class DockPanel2 : DockPanel
    {
        public void ColWidest(object sender, RoutedEventArgs args)
        {
            fd2.ColumnWidth = 150;
        }

        public void ColMore(object sender, RoutedEventArgs args)
        {
            fd2.ColumnWidth = 250;
        }

        public void ColAverage(object sender, RoutedEventArgs args)
        {
            fd2.ColumnWidth = 350;
        }

        public void ColFewer(object sender, RoutedEventArgs args)
        {
            fd2.ColumnWidth = 450;
        }

        public void ColLeast(object sender, RoutedEventArgs args)
        {
            fd2.ColumnWidth = 550;
        }

        public void TextLargest(object sender, RoutedEventArgs args)
        {
            fd2.FontSize = 18;
        }

        public void TextLarge(object sender, RoutedEventArgs args)
        {
            fd2.FontSize = 15;
        }

        public void TextAverage(object sender, RoutedEventArgs args)
        {
            fd2.FontSize = 12;
        }

        public void TextSmall(object sender, RoutedEventArgs args)
        {
            fd2.FontSize = 10;
        }

        public void TextSmallest(object sender, RoutedEventArgs args)
        {
            fd2.FontSize = 8;
        }
    }
}