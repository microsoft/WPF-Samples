using System;
using System.Windows;
using System.Windows.Forms.Integration;

namespace WinFormsHost
{
    /// <summary>
    /// This sample works with the Per Monitor DPI 11/2015 Preview of WPF.
    /// It shows how a developer can use WPF's Per Monitor DPI Aware APIs to
    /// perform scaling of WinForms content in a WPF app.
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
            winFormsHost.DpiChanged += WinFormsHost_DpiChanged;
        }

        private void WinFormsHost_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            System.Windows.Forms.Control control = ((WindowsFormsHost)sender).Child;

            float scaleFactor = (float) (e.NewDpi.PixelsPerDip / e.OldDpi.PixelsPerDip);
            
            // This method recursively scales all child Controls.
            control.Scale(new System.Drawing.SizeF(scaleFactor, scaleFactor));;
            
            // Scale the root control's font
            ScaleFont(control, scaleFactor);

            // Recursively scale the font of controls with different fonts
            ScaleFontRecursively(control, scaleFactor);
        }

        private void ScaleFont(System.Windows.Forms.Control control, float scaleFactor)
        {
            control.Font = new System.Drawing.Font(control.Font.FontFamily,
                                                   control.Font.Size * scaleFactor,
                                                   control.Font.Style
                                                   );
        }

        private void ScaleFontRecursively(System.Windows.Forms.Control parentControl, float scaleFactor)
        {
            System.Drawing.Font parentFont = parentControl.Font;
            foreach (System.Windows.Forms.Control control in parentControl.Controls)
            {
                // Scale the font only if it is different from the parent control's font.
                if (!Object.ReferenceEquals(control.Font, parentFont))
                {
                    ScaleFont(control, scaleFactor);
                }

                ScaleFontRecursively(control, scaleFactor);
            }
        }
    }
}
