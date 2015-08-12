// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Media;

namespace PerFrameAnimation
{
    /// <summary>
    ///     This class is used internally to delegate it's rendering to the parent OverlayRenderDecorator.
    /// </summary>
    internal class OverlayRenderDecoratorOverlayVisual : DrawingVisual
    {
        internal bool IsHitTestVisible { get; set; } = false;
        //dont hit test, these are just overlay graphics
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
        {
            if (IsHitTestVisible)
                return base.HitTestCore(hitTestParameters);
            return null;
        }

        //dont hit test, these are just overlay graphics
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            if (IsHitTestVisible)
                return base.HitTestCore(hitTestParameters);
            return null;
        }
    }
}