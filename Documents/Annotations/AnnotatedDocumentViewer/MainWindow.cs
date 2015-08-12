// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace AnnotatedDocumentViewer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAnchorInfo _info;
        private AnnotationService _service;
        private AnnotationStore _store;
        private Stream _stream;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load annotations store
            _stream = new FileStream("storage.xml", FileMode.OpenOrCreate);
            _service = new AnnotationService(flowDocumentReader);
            _store = new XmlStreamStore(_stream) {AutoFlush = true};
            _service.Enable(_store);

            // Detect when annotations are added or deleted
            _service.Store.StoreContentChanged +=
                AnnotationStore_StoreContentChanged;

            // Bind to annotations in store
            BindToAnnotations(_store.GetAnnotations());
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (_service != null && _service.IsEnabled)
            {
                _service.Disable();
                _stream.Close();
            }
        }

        private void AnnotationStore_StoreContentChanged(object sender, StoreContentChangedEventArgs e)
        {
            // Bind to refreshed annotations store
            BindToAnnotations(_store.GetAnnotations());
        }

        private void annotationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comment = (sender as ListBox).SelectedItem as Annotation;
            if (comment != null)
            {
                // IAnchorInfo info;
                // service is an AnnotationService object
                // comment is an Annotation object
                _info = AnnotationHelper.GetAnchorInfo(_service, comment);
                var resolvedAnchor = _info.ResolvedAnchor as TextAnchor;
                var textPointer = (TextPointer) resolvedAnchor.BoundingStart;
                textPointer.Paragraph.BringIntoView();
            }
        }

        private void BindToAnnotations(IList<Annotation> annotations)
        {
            // Bind to annotations in store
            annotationsListBox.DataContext = annotations;

            // Sort annotations by creation time
            var sortDescription = new SortDescription
            {
                PropertyName = "CreationTime",
                Direction = ListSortDirection.Descending
            };
            var view = CollectionViewSource.GetDefaultView(annotationsListBox.DataContext);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(sortDescription);
        }
    }
}