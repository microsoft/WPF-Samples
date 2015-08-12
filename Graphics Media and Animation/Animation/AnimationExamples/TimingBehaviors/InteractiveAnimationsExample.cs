// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace AnimationExamples
{
    public partial class InteractiveAnimationsExample : Page
    {
        private AnimationTransitionType _selectedTransition;

        // Computes the target point when the user clicks the Canvas
        // and starts the appropriate animation.
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            var targetPoint = args.GetPosition(ContainerCanvas);
            targetPoint.X = targetPoint.X - (MyAnimatedObject.ActualWidth/2);
            targetPoint.Y = targetPoint.Y - (MyAnimatedObject.ActualHeight/2);


            switch (_selectedTransition)
            {
                case AnimationTransitionType.Linear:
                    AnimateToDestinationUsingLinearAnimation(targetPoint);
                    break;

                case AnimationTransitionType.Bounce:
                    AnimateToDestinationUsingBounceAnimation(targetPoint);
                    break;

                case AnimationTransitionType.Elastic:
                    AnimateToDestinationUsingElasticAnimation(targetPoint);
                    break;
            }
        }

        // Animates to the target point using a standard
        // DoubleAnimation.
        private void AnimateToDestinationUsingLinearAnimation(Point targetPoint)
        {
            var xAnimation = new DoubleAnimation
            {
                To = targetPoint.X,
                Duration = new Duration(TimeSpan.FromSeconds(5))
            };
            var yAnimation = new DoubleAnimation
            {
                To = targetPoint.Y,
                Duration = new Duration(TimeSpan.FromSeconds(5))
            };
            MyAnimatedObject.BeginAnimation(Canvas.LeftProperty, xAnimation);
            MyAnimatedObject.BeginAnimation(Canvas.TopProperty, yAnimation);
        }

        // Animates to the target point using a custom
        // BouncAnimation.
        private void AnimateToDestinationUsingBounceAnimation(Point targetPoint)
        {
            var bounceXAnimation =
                new BounceDoubleAnimation
                {
                    From = Canvas.GetLeft(MyAnimatedObject),
                    To = targetPoint.X,
                    Duration = TimeSpan.FromSeconds(5),
                    EdgeBehavior = BounceDoubleAnimation.EdgeBehaviorEnum.EaseIn
                };
            MyAnimatedObject.BeginAnimation(Canvas.LeftProperty, bounceXAnimation);

            var bounceYAnimation =
                new BounceDoubleAnimation
                {
                    From = Canvas.GetTop(MyAnimatedObject),
                    To = targetPoint.Y,
                    Duration = TimeSpan.FromSeconds(5),
                    EdgeBehavior = BounceDoubleAnimation.EdgeBehaviorEnum.EaseIn
                };
            MyAnimatedObject.BeginAnimation(Canvas.TopProperty, bounceYAnimation);
        }

        // Animates to the target point using a custom
        // ElasticAnimation.        
        private void AnimateToDestinationUsingElasticAnimation(Point targetPoint)
        {
            var elasticXAnimation =
                new ElasticDoubleAnimation
                {
                    From = Canvas.GetLeft(MyAnimatedObject),
                    To = targetPoint.X,
                    Duration = TimeSpan.FromSeconds(5),
                    EdgeBehavior = ElasticDoubleAnimation.EdgeBehaviorEnum.EaseIn
                };
            MyAnimatedObject.BeginAnimation(Canvas.LeftProperty, elasticXAnimation);

            var elasticYAnimation =
                new ElasticDoubleAnimation
                {
                    From = Canvas.GetTop(MyAnimatedObject),
                    To = targetPoint.Y,
                    Duration = TimeSpan.FromSeconds(5),
                    EdgeBehavior = ElasticDoubleAnimation.EdgeBehaviorEnum.EaseIn
                };
            MyAnimatedObject.BeginAnimation(Canvas.TopProperty, elasticYAnimation);
        }

        // Sets the default animation transition mode.
        private void PageLoaded(object sender, RoutedEventArgs args)
        {
            LinearTransitionRadioButton.IsChecked = true;
        }

        // Updates the cached animation transition.
        private void SelectedTransitionChanged(object sender, RoutedEventArgs args)
        {
            var value = (string) ((RadioButton) args.Source).Content;
            _selectedTransition = (AnimationTransitionType) Enum.Parse(typeof (AnimationTransitionType), value);
        }
    }
}