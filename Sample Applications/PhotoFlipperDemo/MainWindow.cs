// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace PhotoFlipperDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            // setup trackball for moving the model around
            _trackball = new Trackball();
            _trackball.Attach(this);
            _trackball.Servants.Add(myViewport3D);
            _trackball.Enabled = true;

            // Get the mesh objects for changing the material
            var mv3D = myViewport3D.Children[0] as ModelVisual3D;
            var m3DgBase = mv3D.Content as Model3DGroup;

            var m3Dg = m3DgBase.Children[0] as Model3DGroup;
            _topPlane = m3Dg.Children[2] as GeometryModel3D;
            _bottomPlane = m3Dg.Children[3] as GeometryModel3D;

            m3Dg = m3DgBase.Children[1] as Model3DGroup;
            _frontSpinPlane = m3Dg.Children[0] as GeometryModel3D;
            _backSpinPlane = m3Dg.Children[1] as GeometryModel3D;

            AnimateToNextPicture();
        }

        #region Events

        private void OnToggleAutoRun(object sender, RoutedEventArgs e)
        {
            if (_autorun)
            {
                _autorun = false;
            }
            else
            {
                _autorun = true;
                AnimateToNextPicture();
            }
        }

        private void OnSingleStep(object sender, RoutedEventArgs e)
        {
            AnimateToNextPicture();
        }

        #endregion

        #region Private Methods

        private void AnimateToNextPicture()
        {
            var nextPic = _currentPic + 1;

            if (nextPic > MaxPics)
                nextPic = 1;

            var df1 = FindResource("Pic01" + _currentPic) as DiffuseMaterial;
            var df2 = FindResource("Pic01" + nextPic) as DiffuseMaterial;

            if ((df1 == null) || (df2 == null))
                return;

            _bottomPlane.Material = df1;
            _frontSpinPlane.Material = df1;
            _topPlane.Material = df2;
            _backSpinPlane.Material = df2;

            _currentPic++;
            if (_currentPic > MaxPics)
                _currentPic = 1;

            var s = (Storyboard) FindResource("FlipPicTimeline");
            BeginStoryboard(s);
        }

        private void OnFlipPicTimeline(object sender, EventArgs e)
        {
            var clock = (Clock) sender;

            if (clock.CurrentState == ClockState.Active) // Begun case
            {
                return;
            }

            if (clock.CurrentState != ClockState.Active) // Ended case
            {
                if (_autorun)
                {
                    AnimateToNextPicture();
                }
            }
        }

        #endregion

        #region Globals

        // Geometry models
        private GeometryModel3D _topPlane;
        private GeometryModel3D _bottomPlane;
        private GeometryModel3D _frontSpinPlane;
        private GeometryModel3D _backSpinPlane;

        private Trackball _trackball;
        private int _currentPic = 1;
        private const int MaxPics = 6;

        private bool _autorun;

        #endregion
    }
}