// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace AnimationExamples
{
    public partial class AnimatedParagraphExample : Page
    {
        // Updates the center of the RotateTransform used to rotate
        // the TextBlock's characters.
        private void TextBlockSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (args != null && !args.NewSize.IsEmpty)
            {
                TextEffectRotateTransform.CenterX = args.NewSize.Width/2;
                TextEffectRotateTransform.CenterY = args.NewSize.Height/2;
            }
        }
    }
}