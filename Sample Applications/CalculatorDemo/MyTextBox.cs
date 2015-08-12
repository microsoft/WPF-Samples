// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Controls;
using System.Windows.Input;

namespace CalculatorDemo
{
    internal sealed class MyTextBox : TextBox
    {
        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
            base.OnPreviewGotKeyboardFocus(e);
        }
    }
}