// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Xml;

namespace Brushes
{
    /// <summary>
    ///     Interaction logic for SampleViewer.xaml
    /// </summary>
    public partial class SampleViewer : Page
    {
        public static RoutedUICommand ExitCommand =
            new RoutedUICommand("Exit", "Exit", typeof (SampleViewer));

        public SampleViewer()
        {
            InitializeComponent();
        }

        private void TransitionAnimationStateChanged(object sender, EventArgs args)
        {
            var transitionAnimationClock = (AnimationClock) sender;
            if (transitionAnimationClock.CurrentState == ClockState.Filling)
            {
                FadeEnded();
            }
        }

        private void MyFrameNavigated(object sender, NavigationEventArgs args)
        {
            var myFadeInAnimation = (DoubleAnimation) Resources["MyFadeInAnimationResource"];
            myFrame.BeginAnimation(OpacityProperty, myFadeInAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        private void FadeEnded()
        {
            var el = (XmlElement) myPageList.SelectedItem;
            var att = el.Attributes["Uri"];
            if (att != null)
            {
                myFrame.Navigate(new Uri(att.Value, UriKind.Relative));
            }
            else
            {
                myFrame.Content = null;
            }
        }

        private void ExecuteExitCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}