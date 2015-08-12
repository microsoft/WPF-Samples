// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace HexSphereDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Globals

        private Trackball _trackball;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            // setup trackball for moving the model around
            _trackball = new Trackball();
            _trackball.Attach(this);
            _trackball.Slaves.Add(myViewport3D);
            _trackball.Enabled = true;
        }

        #region Events

        private void OnImage1Animate(object sender, RoutedEventArgs e)
        {
            var s = (Storyboard) FindResource("RotateStoryboard");
            BeginStoryboard(s);
        }

        #endregion
    }
}