// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AnimationExamples
{
    public partial class SampleViewer : Page
    {
        private static readonly Uri PackUri = new Uri("pack://application:,,,/");
        private readonly DoubleAnimation _borderTranslateDoubleAnimation;
        private readonly DoubleAnimation _sampleGridOpacityAnimation;
        private readonly DoubleAnimation _sampleGridTranslateTransformAnimation;

        public SampleViewer()
        {
            InitializeComponent();


            var widthBinding = new Binding("ActualWidth") {Source = this};

            _sampleGridOpacityAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            _sampleGridTranslateTransformAnimation = new DoubleAnimation {BeginTime = TimeSpan.FromSeconds(1)};

            BindingOperations.SetBinding(_sampleGridTranslateTransformAnimation, DoubleAnimation.ToProperty,
                widthBinding);
            _sampleGridTranslateTransformAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            _borderTranslateDoubleAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                BeginTime = TimeSpan.FromSeconds(2),
                FillBehavior = FillBehavior.HoldEnd
            };
            BindingOperations.SetBinding(_borderTranslateDoubleAnimation, DoubleAnimation.FromProperty, widthBinding);
        }

        private void SelectedSampleChanged(object sender, RoutedEventArgs args)
        {
            if (args.Source is RadioButton)
            {
                var theButton = (RadioButton) args.Source;
                var theFrame = (Frame) theButton.Content;

                if (theFrame.HasContent)
                {
                    var source = theFrame.CurrentSource;
                    if ((source != null) && !source.IsAbsoluteUri)
                    {
                        source = new Uri(PackUri, source);
                    }
                    SampleDisplayFrame.Source = source;

                    SampleDisplayBorder.Visibility = Visibility.Visible;
                }
            }
        }

        private void SampleDisplayFrameLoaded(object sender, EventArgs args)
        {
            SampleGrid.BeginAnimation(OpacityProperty, _sampleGridOpacityAnimation);
            SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty,
                _sampleGridTranslateTransformAnimation);
            SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty,
                _borderTranslateDoubleAnimation);
            SampleDisplayBorder.Visibility = Visibility.Visible;
        }

        private void GalleryLoaded(object sender, RoutedEventArgs args)
        {
            SampleDisplayBorderTranslateTransform.X = ActualWidth;
            SampleDisplayBorder.Visibility = Visibility.Hidden;
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs args)
        {
            SampleDisplayBorderTranslateTransform.X = ActualWidth;
        }
    }
}