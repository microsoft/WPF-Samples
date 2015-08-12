// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;

namespace AnnotationsStyling
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

        // ------------------------- OnStyleSelected --------------------------
        /// <summary>
        ///     Replaces the default StickyNote style when a new
        ///     style is selected from the drop down combo box.
        /// </summary>
        protected void OnStyleSelected(object sender, SelectionChangedEventArgs e)
        {
            // Extract the selected style.
            var source = (ComboBox) e.Source;
            var selectedStyle = (StyleMetaData) source.SelectedItem;
            var newStyle = new Style(typeof (StickyNoteControl)) {BasedOn = selectedStyle.Value};

            // Replace the default StickyNote style with the one that was just selected.
            var defaultKey = typeof (StickyNoteControl);
            if (Viewer.Resources.Contains(defaultKey))
                Viewer.Resources.Remove(defaultKey);
            Viewer.Resources.Add(defaultKey, newStyle);

            // Re-load annotations so that they pickup new style.
            var service = AnnotationService.GetService(Viewer);
            service.Disable();
            service.Enable(service.Store);
        }

        #region Boilerplate Annotations Code

        // ----------------------------- OnLoaded -----------------------------
        /// <summary>
        ///     Turns Annotations on.
        /// </summary>
        protected void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Make sure that an AnnotationService isn�t already enabled.
            var service = AnnotationService.GetService(Viewer);
            if (service == null)
            {
                // (a) Create a Stream for the annotations to be stored in.
                _annotationStream =
                    new FileStream("annotations.xml", FileMode.OpenOrCreate);
                // (b) Create an AnnotationService on our
                // FlowDocumentPageViewer.
                service = new AnnotationService(Viewer);
                // (c) Create an AnnotationStore and give it the stream we
                // created. (Autoflush == false)
                AnnotationStore store = new XmlStreamStore(_annotationStream);
                // (d) "Turn on annotations". Annotations will be persisted in
                // the stream created at (a).
                service.Enable(store);
            }
        } // end:OnLoaded


        // ---------------------------- OnUnLoaded ----------------------------
        /// <summary>
        ///     Turns Annotations off.
        /// </summary>
        protected void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // (a) Check that an AnnotationService
            // actually existed and was Enabled.
            var service = AnnotationService.GetService(Viewer);
            if (service != null && service.IsEnabled)
            {
                // (b) Flush changes to annotations to our stream.
                service.Store.Flush();
                // (c) Turn off annotations.
                service.Disable();
                // (d) Close our stream.
                _annotationStream.Close();
            }
        }

        // The stream that we will store annotations in.
        private Stream _annotationStream;

        #endregion Boilerplate Annotations Code
    } // end:partial class Window1


    // =============== class ResourceEntryToComboItemConverter ================
    // end:class ResourceEntryToComboItemConverter


    // ========================= class StyleMetaData ==========================
}