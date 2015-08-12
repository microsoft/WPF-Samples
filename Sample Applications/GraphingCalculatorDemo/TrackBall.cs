// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace GraphingCalculatorDemo
{
    public class Trackball
    {
        private Point3D _center;
        private bool _centered; // Have we already determined the rotation center?
        // The state of the trackball
        private bool _enabled;
        private Point _point; // Initial point of drag
        private Quaternion _rotation;
        private Quaternion _rotationDelta; // Change to rotation because of this drag
        private double _scale;
        private double _scaleDelta; // Change to scale because of this drag
        // The state of the current drag
        private bool _scaling; // Are we scaling?  NOTE otherwise we're rotating
        private List<Viewport3D> _servants;

        public Trackball()
        {
            Reset();
        }

        public List<Viewport3D> Servants
        {
            get { return _servants ?? (_servants = new List<Viewport3D>()); }
            set { _servants = value; }
        }

        public bool Enabled
        {
            get { return _enabled && (_servants != null) && (_servants.Count > 0); }
            set { _enabled = value; }
        }

        public void Attach(FrameworkElement element)
        {
            element.MouseMove += MouseMoveHandler;
            element.MouseLeftButtonDown += MouseDownHandler;
            element.MouseLeftButtonUp += MouseUpHandler;
        }

        public void Detach(FrameworkElement element)
        {
            element.MouseMove -= MouseMoveHandler;
            element.MouseLeftButtonDown -= MouseDownHandler;
            element.MouseLeftButtonUp -= MouseUpHandler;
        }

        // Updates the matrices of the slaves using the rotation quaternion.
        private void UpdateServants(Quaternion q, double s)
        {
            var rotation = new RotateTransform3D(); //IB: changed this


            var quatRotation = new QuaternionRotation3D(q);
            rotation.Rotation = quatRotation;
            //rotation.Rotation = new Rotation3D(q);
            rotation.CenterX = _center.X;
            rotation.CenterY = _center.Y;

            var scale = new ScaleTransform3D(new Vector3D(s, s, s));
            var rotateAndScale = new Transform3DCollection {scale, rotation};

            //IB: moved out of the constructor above


            if (_servants != null)
            {
                foreach (var i in _servants)
                {
                    // Note that we don't copy constantly here, we copy the first time someone tries to
                    // trackball a frozen Models, but we replace it with a ChangeableReference
                    // and so subsequent interactions go through without a copy.
                    /* mijacobs: commenting out
					if (i.Models.Transform.IsFrozen)
					{
						Model3DGroup mutableCopy = i.Models.Copy();
						//mutableCopy.StatusOfNextUse = UseStatus.ChangeableReference; IB: no longer necessary I need to architect this out if time permits
						i.Models = mutableCopy;
					}*/

                    var myTransformGroup = new Transform3DGroup {Children = rotateAndScale}; //IB: added transformGroup

                    // mijacobs old: i.Models.Transform = myTransformGroup;
                    ((Model3DGroup) ((ModelVisual3D) i.Children[0]).Content).Transform = myTransformGroup;
                }
            }
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            e.Handled = true;

            var el = (UIElement) sender;
            if (el.IsMouseCaptured)
            {
                var delta = _point - e.MouseDevice.GetPosition(el);
                delta /= 2;
                // We can redefine this 2D mouse delta as a 3D mouse delta
                // where "into the screen" is Z
                var mouse = new Vector3D(delta.X, -delta.Y, 0);
                var axis = Vector3D.CrossProduct(mouse, new Vector3D(0, 0, 1));
                var len = axis.Length;
                if (len < 0.00001 || _scaling)
                    _rotationDelta = new Quaternion(new Vector3D(0, 0, 1), 0);
                else
                    _rotationDelta = new Quaternion(axis, len);
                var q = _rotationDelta*_rotation;
                _scaleDelta = _scaling ? Math.Exp(delta.Y/10) : 1;
                UpdateServants(q, _scale*_scaleDelta);
            }
        }

        private void MouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled) return;
            e.Handled = true;

            if (e.ClickCount == 2)
            {
                Reset();
                return;
            }

            var el = (UIElement) sender;
            _point = e.MouseDevice.GetPosition(el);
            // Initialize the center of rotation to the lookatpoint
            if (!_centered)
            {
                var camera = (ProjectionCamera) _servants[0].Camera;
                _center = (Point3D) camera.LookDirection;
                _centered = true;
            }
            _scaling = (e.RightButton == MouseButtonState.Pressed);

            el.CaptureMouse();
        }

        private void MouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            if (!_enabled) return;
            e.Handled = true;

            // Stuff the current initial + delta into initial so when we next move we
            // start at the right place.
            _rotation = _rotationDelta*_rotation;
            _scale = _scaleDelta*_scale;
            var el = (UIElement) sender;
            el.ReleaseMouseCapture();
        }

        private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            _rotation = Quaternion.Identity;
            _scale = 1;
            // Clear delta too, because if reset is called because of a double click then the mouse
            // up handler will also be called and this way it won't do anything.
            _rotationDelta = Quaternion.Identity;
            _scaleDelta = 1;
            UpdateServants(_rotation, _scale);
        }
    }
}