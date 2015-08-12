// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace SDKSample
{
    public partial class BlurInteractiveExample : Page
    {
        // Add Bevel effect.
        private void ChangeSelection(object sender, RoutedEventArgs args)
        {
            var cb = (ComboBox) sender;
            var cbi = (ComboBoxItem) cb.SelectedValue;

            var s = cbi.Content.ToString();
            if (s == "Box")
            {
                InteractiveEffect.KernelType = KernelType.Box;
            }
            else if (s == "Gaussian")
            {
                InteractiveEffect.KernelType = KernelType.Gaussian;
            }
        }
    }
}