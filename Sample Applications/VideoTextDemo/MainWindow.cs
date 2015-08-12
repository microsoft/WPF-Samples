// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace VideoTextDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Create the text as FormattedText object.
            var formattedText = new FormattedText(
                "FABLE",
                new CultureInfo("en-US"),
                FlowDirection.LeftToRight,
                new Typeface(
                    new FontFamily("Segoe UI"),
                    FontStyles.Normal,
                    FontWeights.Bold,
                    FontStretches.Normal),
                120,
                Brushes.Red);

            // Build a geometry out of the text.
            var geometry = formattedText.BuildGeometry(new Point(0, 0));

            // Adjust the geometry to fit the aspect ration of the video by scaling it.
            var myScaleTransform = new ScaleTransform
            {
                ScaleX = .85,
                ScaleY = 2.0
            };
            geometry.Transform = myScaleTransform;

            // Flatten the geometry and create a PathGeometry out of it.
            var pathGeometry = geometry.GetFlattenedPathGeometry();

            // Use the PathGeometry for the empty placeholder Path element in XAML.
            path.Data = pathGeometry;

            // Use the PathGeometry to clip the video.
            myMediaElement.Clip = pathGeometry;

            // Use the PathGeometry for the animated ball that follows the path of the text outline.
            matrixAnimation.PathGeometry = pathGeometry;
        }

        // The handler for the MediaFailed event is invoked when MediaElement detects an error.
        public void OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var exceptionString = e.ErrorException.Message + " : " + e.ErrorException.InnerException.Message;
        }
    }
}