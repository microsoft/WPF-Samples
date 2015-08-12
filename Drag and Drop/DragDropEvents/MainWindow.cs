// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace DragDropEvents
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // To store the height of the other elements in the grid column containing
        // the event log.
        private readonly double _heightAdjustment;
        private uint _eventCount;

        public MainWindow()
        {
            InitializeComponent();
            _heightAdjustment = lblEventLogWindowTitle.Height + lblEventSelectionTitle.Height + eventChecksGrid.Height;
        }

        private void ClickClearEventHistory(object sender, RoutedEventArgs args)
        {
            tbEventEvents.Clear();
            _eventCount = 0;
        }

        // This event handler fires whenever the main window changes size.  
        private void EhEventWindowSizeChanged(object sender, EventArgs args)
        {
            // Adjust the size of the TextBox so scrolling works properly.
            tbEventEvents.MaxHeight = tbEventEvents.Height = mainGrid.RowDefinitions[1].ActualHeight - _heightAdjustment;
        }

        // Handlers for the drag/drop events.  
        private void EhDragEnter(object sender, DragEventArgs args)
        {
            if (cbDragEnter.IsChecked.Value) LogEvent(EventFireStrings.DragEnter, args);
        }

        private void EhDragLeave(object sender, DragEventArgs args)
        {
            if (cbDragLeave.IsChecked.Value) LogEvent(EventFireStrings.DragLeave, args);
        }

        private void EhDragOver(object sender, DragEventArgs args)
        {
            if (cbDragOver.IsChecked.Value) LogEvent(EventFireStrings.DragOver, args);
        }

        private void EhDrop(object sender, DragEventArgs args)
        {
            if (cbDrop.IsChecked.Value) LogEvent(EventFireStrings.Drop, args);
        }

        private void EhPreviewDragEnter(object sender, DragEventArgs args)
        {
            if (cbPreviewDragEnter.IsChecked.Value) LogEvent(EventFireStrings.PreviewDragEnter, args);
        }

        private void EhPreviewDragLeave(object sender, DragEventArgs args)
        {
            if (cbPreviewDragLeave.IsChecked.Value) LogEvent(EventFireStrings.PreviewDragLeave, args);
        }

        private void EhPreviewDragOver(object sender, DragEventArgs args)
        {
            if (cbPreviewDragOver.IsChecked.Value) LogEvent(EventFireStrings.PreviewDragOver, args);
        }

        private void EhPreviewDrop(object sender, DragEventArgs args)
        {
            if (cbPreviewDrop.IsChecked.Value) LogEvent(EventFireStrings.PreviewDrop, args);
        }

        private void LogEvent(string eventMessage, DragEventArgs args)
        {
            tbEventEvents.AppendText("[" + (++_eventCount) + eventMessage);

            if (cbVerbose.IsChecked.Value)
            {
                tbEventEvents.AppendText("     Source Object: " + args.Source + "\n");
                tbEventEvents.AppendText("     Drag Effects: " + args.Effects + "\n");
                tbEventEvents.AppendText("     Key States: " + args.KeyStates + "\n");
                tbEventEvents.AppendText("     Available Data Formats:\n");
                foreach (var format in args.Data.GetFormats())
                {
                    tbEventEvents.AppendText("          " + format + "\n");
                }
            }
            tbEventEvents.ScrollToEnd();
        }

        private struct EventFireStrings
        {
            public static readonly string DragEnter = "]: The DragEnter event just fired.\n";
            public static readonly string DragLeave = "]: The DragLeave event just fired.\n";
            public static readonly string DragOver = "]: The DragOver event just fired.\n";
            public static readonly string Drop = "]: The Drop event just fired.\n";
            public static readonly string PreviewDragEnter = "]: The PreviewDragEnter event just fired.\n";
            public static readonly string PreviewDragLeave = "]: The PreviewDragLeave event just fired.\n";
            public static readonly string PreviewDragOver = "]: The PreviewDragOver event just fired.\n";
            public static readonly string PreviewDrop = "]: The PreviewDrop event just fired.\n";
        }
    }
}