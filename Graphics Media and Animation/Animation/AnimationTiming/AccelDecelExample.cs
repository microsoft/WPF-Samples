// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AnimationTiming
{
    public partial class AccelDecelExample : Page
    {
        private void StateInvalidated(object sender, EventArgs args)
        {
            if (sender != null)
            {
                elapsedTime.Clock = (Clock) sender;
            }
        }
    }
}