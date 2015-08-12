// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace SDKSample
{
    public partial class BevelInteractiveExample : Page
    {
        private void ChangeSelection(object sender, RoutedEventArgs args)
        {
            var cb = (ComboBox) sender;
            var cbi = (ComboBoxItem) cb.SelectedValue;

            var s = cbi.Content.ToString();
            switch (s)
            {
                case "BulgedUp":
                    InteractiveEffect.EdgeProfile = EdgeProfile.BulgedUp;
                    break;
                case "CurvedIn":
                    InteractiveEffect.EdgeProfile = EdgeProfile.CurvedIn;
                    break;
                case "CurvedOut":
                    InteractiveEffect.EdgeProfile = EdgeProfile.CurvedOut;
                    break;
                case "Linear":
                    InteractiveEffect.EdgeProfile = EdgeProfile.Linear;
                    break;
            }
        }
    }
}