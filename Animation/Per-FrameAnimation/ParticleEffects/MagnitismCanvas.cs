// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PerFrameAnimation
{
    public class MagnitismCanvas : Canvas
    {
        public MagnitismCanvas()
        {
            // suppress movement in the visual studio designer.
            if (Process.GetCurrentProcess().ProcessName != "devenv")
                CompositionTarget.Rendering += UpdateChildren;
            _timeTracker = new ParticleEffectsTimeTracker();
        }

        private void UpdateChildren(object sender, EventArgs e)
        {
            //update time delta
            _timeTracker.Update();

            foreach (UIElement child in LogicalTreeHelper.GetChildren(this))
            {
                var velocity = _childrenVelocities.ContainsKey(child) ? _childrenVelocities[child] : new Vector(0, 0);

                //compute velocity dampening
                velocity = velocity*Drag;

                var truePosition = GetTruePosition(child);
                var childRect = new Rect(truePosition, child.RenderSize);


                //accumulate forces
                var forces = new Vector(0, 0);

                //add wall repulsion
                forces.X += BorderForce/Math.Max(1, childRect.Left);
                forces.X -= BorderForce/Math.Max(1, RenderSize.Width - childRect.Right);
                forces.Y += BorderForce/Math.Max(1, childRect.Top);
                forces.Y -= BorderForce/Math.Max(1, RenderSize.Height - childRect.Bottom);

                //each other child pushes away based on the square distance
                foreach (UIElement otherchild in LogicalTreeHelper.GetChildren(this))
                {
                    //dont push against itself
                    if (otherchild == child)
                        continue;

                    var otherchildtruePosition = GetTruePosition(otherchild);
                    var otherchildRect = new Rect(otherchildtruePosition, otherchild.RenderSize);

                    //make sure rects aren't the same
                    if (otherchildRect == childRect)
                        continue;

                    //ignore children with a size of 0,0
                    if (otherchildRect.Width == 0 && otherchildRect.Height == 0 ||
                        childRect.Width == 0 && childRect.Height == 0)
                        continue;

                    //vector from current other child to current child
                    //are they overlapping?  if so, distance is 0
                    var toChild = AreRectsOverlapping(childRect, otherchildRect)
                        ? new Vector(0, 0)
                        : VectorBetweenRects(childRect, otherchildRect);

                    var length = toChild.Length;
                    if (length < 1)
                    {
                        length = 1;
                        var childCenter = GetCenter(childRect);
                        var otherchildCenter = GetCenter(otherchildRect);
                        //compute toChild from the center of both rects
                        toChild = childCenter - otherchildCenter;
                    }

                    var childpush = ChildForce/length;

                    toChild.Normalize();
                    forces += toChild*childpush;
                }

                //add forces to velocity and store it for next iteration
                velocity += forces;
                _childrenVelocities[child] = velocity;

                //move the object based on it's velocity
                SetTruePosition(child, truePosition + _timeTracker.DeltaSeconds*velocity);
            }
        }

        private bool AreRectsOverlapping(Rect r1, Rect r2)
        {
            if (r1.Bottom < r2.Top) return false;
            if (r1.Top > r2.Bottom) return false;

            if (r1.Right < r2.Left) return false;
            if (r1.Left > r2.Right) return false;

            return true;
        }

        private Point IntersectInsideRect(Rect r, Point raystart, Vector raydir)
        {
            var xtop = raystart.X + raydir.X*(r.Top - raystart.Y)/raydir.Y;
            var xbottom = raystart.X + raydir.X*(r.Bottom - raystart.Y)/raydir.Y;
            var yleft = raystart.Y + raydir.Y*(r.Left - raystart.X)/raydir.X;
            var yright = raystart.Y + raydir.Y*(r.Right - raystart.X)/raydir.X;
            var top = new Point(xtop, r.Top);
            var bottom = new Point(xbottom, r.Bottom);
            var left = new Point(r.Left, yleft);
            var right = new Point(r.Right, yright);
            var tv = raystart - top;
            var bv = raystart - bottom;
            var lv = raystart - left;
            var rv = raystart - right;
            //classify ray direction
            if (raydir.Y < 0)
            {
                if (raydir.X < 0) //top left
                {
                    if (tv.LengthSquared < lv.LengthSquared)
                        return top;
                    return left;
                }
                if (tv.LengthSquared < rv.LengthSquared)
                    return top;
                return right;
            }
            if (raydir.X < 0) //bottom left
            {
                if (bv.LengthSquared < lv.LengthSquared)
                    return bottom;
                return left;
            }
            if (bv.LengthSquared < rv.LengthSquared)
                return bottom;
            return right;
        }

        private Vector VectorBetweenRects(Rect r1, Rect r2)
        {
            //find the edge points and use these to measure the distance
            var r1Center = GetCenter(r1);
            var r2Center = GetCenter(r2);
            var between = (r1Center - r2Center);
            between.Normalize();
            var edge1 = IntersectInsideRect(r1, r1Center, -between);
            var edge2 = IntersectInsideRect(r2, r2Center, between);
            return edge1 - edge2;
        }

        private Point GetRenderTransformOffset(UIElement e)
        {
            //make sure they object's render transform is a translation
            var renderTranslation = e.RenderTransform as TranslateTransform;
            if (renderTranslation == null)
            {
                renderTranslation = new TranslateTransform(0, 0);
                e.RenderTransform = renderTranslation;
            }

            return new Point(renderTranslation.X, renderTranslation.Y);
        }

        private void SetRenderTransformOffset(UIElement e, Point offset)
        {
            //make sure they object's render transform is a translation
            var renderTranslation = e.RenderTransform as TranslateTransform;
            if (renderTranslation == null)
            {
                renderTranslation = new TranslateTransform(0, 0);
                e.RenderTransform = renderTranslation;
            }

            //set new offset
            renderTranslation.X = offset.X;
            renderTranslation.Y = offset.Y;
        }

        private Point GetTruePosition(UIElement e)
        {
            var renderTranslation = GetRenderTransformOffset(e);
            return new Point(GetLeft(e) + renderTranslation.X, GetTop(e) + renderTranslation.Y);
        }

        private void SetTruePosition(UIElement e, Point p)
        {
            var canvasOffset = new Vector(GetLeft(e), GetTop(e));
            var renderTranslation = p - canvasOffset;

            SetRenderTransformOffset(e, renderTranslation);
        }

        private Point GetCenter(Rect r) => new Point((r.Left + r.Right) / 2.0, (r.Top + r.Bottom) / 2.0);

        #region Private Members

        private readonly ParticleEffectsTimeTracker _timeTracker;
        private readonly Dictionary<UIElement, Vector> _childrenVelocities = new Dictionary<UIElement, Vector>();

        #endregion

        #region Properties

        public double BorderForce { get; set; } = 1000.0;

        public double ChildForce { get; set; } = 200.0;

        public double Drag { get; set; } = 0.9;

        #endregion
    }
}