using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormattedTextExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {            
            DrawFormattedText(VisualTreeHelper.GetDpi(this));
        }

        protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
        {
            DrawFormattedText(newDpiScaleInfo);
        }

        private void DrawFormattedText(DpiScale dpiInfo)
        {
            FormattedText formattedText = new FormattedText(
                "FABLE",
                new System.Globalization.CultureInfo("en-US"),
                FlowDirection.LeftToRight,
                new Typeface(
                    new System.Windows.Media.FontFamily("Segoe UI"),
                    FontStyles.Normal,
                    FontWeights.Bold,
                    FontStretches.Normal),
                120,
                System.Windows.Media.Brushes.Red,
                dpiInfo.PixelsPerDip);

            // Build a geometry out of the text.
            Geometry geometry = new PathGeometry();
            geometry = formattedText.BuildGeometry(new System.Windows.Point(0, 0));

            // Adjust the geometry to fit the aspect ration of the video by scaling it.
            ScaleTransform myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleX = .85;
            myScaleTransform.ScaleY = 2.0;
            geometry.Transform = myScaleTransform;

            // Flatten the geometry and create a PathGeometry out of it.
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry = geometry.GetFlattenedPathGeometry();

            // Use the PathGeometry for the empty placeholder Path element in XAML.
            path.Data = pathGeometry;

            // Use the PathGeometry for the animated ball that follows the path of the text outline.
            matrixAnimation.PathGeometry = pathGeometry;
        }
    }
}
