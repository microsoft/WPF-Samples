// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SystemBrushesAndColors
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListSystemBrushes();
            ListGradientExamples();
        }

        // Demonstrates using system colors to fill rectangles and buttons.
        private void ListSystemBrushes()
        {
            // The window style (defined in Window1.xaml) is used to
            // specify the height and width of each of the System.Windows.Shapes.Rectangles.
            var t = new TextBlock {Text = "ActiveBorder"};
            var r = new Rectangle
            {
                Fill = SystemColors.ActiveBorderBrush
            };
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ActiveCaption"};
            r = new Rectangle {Fill = SystemColors.ActiveCaptionBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ActiveCaptionText"};
            r = new Rectangle {Fill = SystemColors.ActiveCaptionTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "AppWorkspace"};
            r = new Rectangle {Fill = SystemColors.AppWorkspaceBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "Control"};
            r = new Rectangle {Fill = SystemColors.ControlBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ControlDark"};
            r = new Rectangle {Fill = SystemColors.ControlDarkBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ControlDarkDark"};
            r = new Rectangle {Fill = SystemColors.ControlDarkDarkBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ControlLight"};
            r = new Rectangle {Fill = SystemColors.ControlLightBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ControlLightLight"};
            r = new Rectangle {Fill = SystemColors.ControlLightLightBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ControlText"};
            r = new Rectangle {Fill = SystemColors.ControlTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "Desktop"};
            r = new Rectangle();
            //r.Fill = SystemColors.DesktopBrush;
            r.SetResourceReference(Shape.FillProperty, SystemColors.DesktopBrushKey);

            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "GradientActiveCaption"};
            r = new Rectangle {Fill = SystemColors.GradientActiveCaptionBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "GradientInactiveCaption"};
            r = new Rectangle {Fill = SystemColors.GradientInactiveCaptionBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "GrayText"};
            r = new Rectangle {Fill = SystemColors.GrayTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "Highlight"};
            r = new Rectangle {Fill = SystemColors.HighlightBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "HighlightText"};
            r = new Rectangle {Fill = SystemColors.HighlightTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "HotTrack"};
            r = new Rectangle {Fill = SystemColors.HotTrackBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "InactiveBorder"};
            r = new Rectangle {Fill = SystemColors.InactiveBorderBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "InactiveCaption"};
            r = new Rectangle {Fill = SystemColors.InactiveCaptionBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "InactiveCaptionText"};
            r = new Rectangle {Fill = SystemColors.InactiveCaptionTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "Info"};
            r = new Rectangle {Fill = SystemColors.InfoBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "InfoText"};
            r = new Rectangle {Fill = SystemColors.InfoTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "Menu"};
            r = new Rectangle {Fill = SystemColors.MenuBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "MenuBar"};
            r = new Rectangle {Fill = SystemColors.MenuBarBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "MenuHighlight"};
            r = new Rectangle {Fill = SystemColors.MenuHighlightBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "MenuText"};
            r = new Rectangle {Fill = SystemColors.MenuTextBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "ScrollBar"};
            r = new Rectangle {Fill = SystemColors.ScrollBarBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            t = new TextBlock {Text = "Window"};
            r = new Rectangle {Fill = SystemColors.WindowBrush};
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(r);

            // Try it out on a button.
            t = new TextBlock {Text = "WindowFrame"};
            var b = new Button
            {
                Width = 120,
                Height = 20,
                Background = SystemColors.WindowFrameBrush
            };
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(b);

            t = new TextBlock {Text = "WindowText"};
            b = new Button
            {
                Width = 120,
                Height = 20,
                Background = SystemColors.WindowTextBrush
            };
            systemBrushesPanel.Children.Add(t);
            systemBrushesPanel.Children.Add(b);
        }

        // Demonstrates using system colors to create gradients.
        public void ListGradientExamples()
        {
            // The window style (defined in Window1.xaml) is used to
            // specify the height and width of each of the System.Windows.Shapes.Rectangles.

            var t = new TextBlock
            {
                Text = "System Color Gradient Examples",
                HorizontalAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold
            };
            gradientExamplePanel.Children.Add(t);

            t = new TextBlock {Text = "ControlDark to ControlLight"};
            var r = new Rectangle
            {
                Fill = new RadialGradientBrush(
                    SystemColors.ControlDarkColor, SystemColors.ControlLightColor)
            };
            gradientExamplePanel.Children.Add(t);
            gradientExamplePanel.Children.Add(r);

            t = new TextBlock {Text = "ControlDarkDark to ControlLightLight"};
            r = new Rectangle
            {
                Fill =
                    new LinearGradientBrush(SystemColors.ControlDarkDarkColor,
                        SystemColors.ControlLightLightColor, 45)
            };
            gradientExamplePanel.Children.Add(t);
            gradientExamplePanel.Children.Add(r);

            // Try it out on a button.
            t = new TextBlock {Text = "Desktop to AppWorkspace"};
            var b = new Button
            {
                Width = 120,
                Height = 20,
                Background =
                    new RadialGradientBrush(SystemColors.DesktopColor,
                        SystemColors.AppWorkspaceColor)
            };
            gradientExamplePanel.Children.Add(t);
            gradientExamplePanel.Children.Add(b);

            t = new TextBlock {Text = "Desktop to Control"};
            b = new Button
            {
                Width = 120,
                Height = 20,
                Background =
                    new RadialGradientBrush(SystemColors.DesktopColor,
                        SystemColors.ControlColor)
            };
            gradientExamplePanel.Children.Add(t);
            gradientExamplePanel.Children.Add(b);
        }
    }
}