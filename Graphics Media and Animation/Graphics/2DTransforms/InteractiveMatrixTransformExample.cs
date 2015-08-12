// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Samples.Graphics.Transforms
{
    public partial class InteractiveMatrixTransformExample : Page
    {
        private void ApplyButtonClicked(object sender, EventArgs args)
        {
            UpdateMatrixTransform();
        }

        private void UpdateMatrixTransform()
        {
            var myMatrix = new Matrix
            {
                M11 = double.Parse(M11TextBox.Text),
                M12 = double.Parse(M12TextBox.Text),
                M21 = double.Parse(M21TextBox.Text),
                M22 = double.Parse(M22TextBox.Text),
                OffsetX = double.Parse(OffsetXTextBox.Text),
                OffsetY = double.Parse(OffsetYTextBox.Text)
            };


            myMatrixTransform.Matrix = myMatrix;
        }
    }
}