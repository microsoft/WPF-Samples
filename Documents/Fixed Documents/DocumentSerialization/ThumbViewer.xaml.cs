// DocumentSerialize SDK Sample - ThumbViewer.cs
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.IO;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Collections;
using System.Xml;
using System.Drawing;

namespace DocumentSerialization
{
    public partial class ThumbViewer : Window
    {
        #region Command Handlers
        // ------------------------ AddCommandHandlers ------------------------
        private void AddCommandHandlers(FrameworkElement uiScope)
        {
            CommandManager.RegisterClassCommandBinding( typeof(ThumbViewer),
                new CommandBinding( ApplicationCommands.Open,
                            new ExecutedRoutedEventHandler(OnOpen),
                            new CanExecuteRoutedEventHandler(OnNewQuery) ) );

            // Add Command Handlers
            CommandBindingCollection commandBindings = uiScope.CommandBindings;

            commandBindings.Add(
                new CommandBinding( ThumbViewer.Exit,
                            new ExecutedRoutedEventHandler(OnExit),
                            new CanExecuteRoutedEventHandler(OnNewQuery) ) );

            commandBindings.Add(
                new CommandBinding( ThumbViewer.SaveAs,
                            new ExecutedRoutedEventHandler(OnSaveAs),
                            new CanExecuteRoutedEventHandler(OnNewQuery) ) );

            commandBindings.Add(
                new CommandBinding(ThumbViewer.AddBookmark,
                            new ExecutedRoutedEventHandler(OnAddBookmark),
                            new CanExecuteRoutedEventHandler(OnNewQuery) ) );

            commandBindings.Add(
                new CommandBinding(ThumbViewer.AddComment,
                            new ExecutedRoutedEventHandler(OnAddComment),
                            new CanExecuteRoutedEventHandler(OnNewQuery) ) );

            // Enable Annotations
            _annotationBuffer = new MemoryStream();
            _annStore = new XmlStreamStore(_annotationBuffer);
            _annServ  = new AnnotationService(FDPV);
            _annStore.StoreContentChanged +=
                new StoreContentChangedEventHandler(_annStore_StoreContentChanged);
            _annServ.Enable(_annStore);

        }// end:AddCommandHandlers()


        public static RoutedCommand Exit => DeclareCommand(ref _Exit, "FileExit");

        private static RoutedCommand _Exit;

        public static RoutedCommand SaveAs => DeclareCommand(ref _SaveAs, "SaveAs");

        private static RoutedCommand _SaveAs;

        public static RoutedCommand AddBookmark => DeclareCommand(ref _AddBookmark, "AddBookmark");

        private static RoutedCommand _AddBookmark;

        public static RoutedCommand AddComment => DeclareCommand(ref _AddComment, "AddComment");

        private static RoutedCommand _AddComment;

        private static RoutedCommand DeclareCommand(ref RoutedCommand command,
                                                      string commandDebugName) => DeclareCommand(ref command, commandDebugName, null);

        private static RoutedCommand DeclareCommand(ref RoutedCommand command,
                                string commandDebugName, InputGesture gesture)
        {
            if (command == null)
            {
                InputGestureCollection collection = null;

                if (gesture != null)
                {
                    collection = new InputGestureCollection();
                    collection.Add(gesture);
                }

                command = new RoutedCommand(commandDebugName,
                                            typeof(ThumbViewer), collection);
            }
            return command;
        }// end:DeclareCommand()
        #endregion Command Handlers


        #region Initialize and Terminate
        // ---------------------- InitializeThumbViewer -----------------------
        private void InitializeThumbViewer(object sender, System.EventArgs args)
        {
            AddCommandHandlers(this);
            _reflectionTextPointer_InsertTextElement =
                typeof(TextPointer).GetMethod( "InsertTextElement",
                              BindingFlags.NonPublic | BindingFlags.Instance );
            if (_reflectionTextPointer_InsertTextElement == null)
            {
                throw new Exception(
                    "TextPointer.InsertTextElement method not found" );
            }
            Debug.Assert(FDPV.Document == null);
        }// end:InitializeThumbViewer()


        // ------------------------------ OnExit ------------------------------
        private void OnExit(object target, ExecutedRoutedEventArgs args)
        {
            ShutDown();
        }


        // ------------------------ ClosedThumbViewer -------------------------
        void ClosedThumbViewer(object sender, EventArgs e)
        {
            ShutDown();
        }


        // ----------------------------- ShutDown -----------------------------
        void ShutDown()
        {
            if (FDPV.Document != null)
            {
                Debug.Assert(FDPV.Document.DocumentPaginator is
                                                    DynamicDocumentPaginator);
                (FDPV.Document.DocumentPaginator as
                    DynamicDocumentPaginator).PaginationCompleted -=
                                                    PaginationCompleted;
            }

            if (Application.Current != null)
                Application.Current.Shutdown();
        }// end:ShutDown()
        #endregion Initialize and Terminate


        #region Load and Save Documents
        // ------------------------------ OnOpen ------------------------------
        private static void OnOpen(object target, ExecutedRoutedEventArgs args)
        {
            ThumbViewer tv = (ThumbViewer)target;
            if (tv.OpenDocument(null))  // null indicates prompt for filename.
            {
                tv.CreateThumbs();
            }
        }


        // --------------------------- OpenDocument ---------------------------
        public bool OpenDocument(string fileName)
        {
            Microsoft.Win32.OpenFileDialog dialog;

            // If there is a document currently open, close it.
            if (this.Document != null)  CloseFile();

            dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.InitialDirectory = GetContentFolder();
            dialog.Filter = this.OpenFileFilter;
            bool result = (bool)dialog.ShowDialog(null);
            if (result == false)  return false;

            fileName = dialog.FileName;
            return OpenFile(fileName);
        }// end:OpenDocument()


        // ------------------------- GetContentFolder -------------------------
        /// <summary>
        ///   Locates and returns the path to the "Content\" folder
        ///   containing the document for the sample.</summary>
        /// <returns>
        ///   The path to the sample document "Content\" folder.</returns>
        private string GetContentFolder()
        {
            // Get the path to the current directory and its length.
            string contentDir = Directory.GetCurrentDirectory();
            int dirLength = contentDir.Length;

            // If we're in "...\bin\debug", move up to the root.
            if (contentDir.ToLower().EndsWith(@"\bin\debug"))
                contentDir = contentDir.Remove(dirLength - 10, 10);

            // If we're in "...\bin\release", move up to the root.
            else if (contentDir.ToLower().EndsWith(@"\bin\release"))
                contentDir = contentDir.Remove(dirLength - 12, 12);

            // If there's a "Content" subfolder, that's what we want.
            if (Directory.Exists(contentDir + @"\Content"))
                contentDir = contentDir + @"\Content";

            // Return the "Content\" folder (or the "current"
            // directory if we're executing somewhere else).
            return contentDir;
        }// end:GetContentFolder()


        // ----------------------------- OpenFile -----------------------------
        private bool OpenFile(string fileName)
        {
            if (fileName == null)  throw new ArgumentNullException(nameof(fileName));

            // Check file existence
            if (!System.IO.File.Exists(fileName))
            {
                MessageBox.Show(
                    "File not found: " + fileName, this.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            object newDocument = null;

            // Apply appropriate converter to a file content
            try
            {
                if (fileName.EndsWith(".xaml"))
                {
                    using (FileStream inputStream = File.OpenRead(fileName))
                    {
                        ParserContext pc = new ParserContext();
                        pc.BaseUri =
                            new Uri(System.Environment.CurrentDirectory + "/");
                        newDocument = XamlReader.Load(inputStream, pc) as object;
                        if (newDocument == null)
                        {
                            MessageBox.Show(
                                "Invalid Xaml File. Could not be parsed" +
                                fileName, this.GetType().Name,
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Error occurred during conversion from this file format: " +
                    fileName + "\n" + e.ToString(), this.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (newDocument is IDocumentPaginatorSource)
            {
                if (this.Document != null)
                {
                    Debug.Assert(FDPV.Document.DocumentPaginator is
                                                    DynamicDocumentPaginator);
                    (FDPV.Document.DocumentPaginator as
                        DynamicDocumentPaginator).PaginationCompleted -=
                                                          PaginationCompleted;
                }
                this.Document = (IDocumentPaginatorSource)newDocument;
                Util.FlushDispatcher();
                Debug.Assert(FDPV.Document.DocumentPaginator is
                                        DynamicDocumentPaginator);
                (FDPV.Document.DocumentPaginator as
                    DynamicDocumentPaginator).PaginationCompleted +=
                                        new EventHandler(PaginationCompleted);
                SaveAsMenu.IsEnabled = true;
                SaveAsToolbarButton.IsEnabled = true;
                _fileName = fileName;
            }

            else // if !(newDocument is IDocumentPaginatorSource)
            {
                throw new InvalidDataException(
                    "Thumbnail viewer only supports IDocumentPaginatorSource");
                //return false;
            }

            return true;
        }// end:OpenFile()


        // ---------------------------- CloseFile -----------------------------
        private void CloseFile()
        {
            // If there are existing annotations, then they should be saved.
            if (_annStore.GetAnnotations().Count > 0)
            {
                string backupFile = _fileName + ".annotations" + ".xml";
                FileStream backupStream = new FileStream(backupFile, FileMode.Create);
                CopyStream(_annotationBuffer, backupStream);
                _annStore.Flush();
                _annServ.Disable();
            }
        }


        // ---------------------------- CopyStream ----------------------------
        /// <summary>
        ///   Copies the contents of one stream to another.</summary>
        /// <param name="src">
        ///   The source stream.</param>
        /// <param name="dest">
        ///   The destination stream.</param>
        private void CopyStream(Stream src, Stream dest)
        {
            long originalPosition = src.Position;

            src.Seek(0, SeekOrigin.Begin);
            dest.Seek(0, SeekOrigin.Begin);
            dest.SetLength(0); // Erase destination.
            StreamReader reader = new StreamReader(src);
            StreamWriter writer = new StreamWriter(dest);
            while (!reader.EndOfStream)
            {
                char[] buffer = new char[50];
                reader.Read(buffer, 0, buffer.Length);
                writer.Write(buffer);
            }
            writer.Flush();
            dest.Seek(0, SeekOrigin.Begin);

            src.Seek(originalPosition, SeekOrigin.Begin);
        }// end:CopyStream()


        // -------------------------- OpenFileFilter --------------------------
        private string OpenFileFilter => "XAML FlowDocuments (*.xaml)|*.xaml";


        // ---------------------------- OnNewQuery ----------------------------
        private static void OnNewQuery(
                                object target, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }


        // --------------------------- CreateThumbs ---------------------------
        public void CreateThumbs()
        {
            if ( (ThumbList.Items != null) && (ThumbList.Items.Count > 0) )
                ThumbList.Items.Clear();

            _currentThumbnail = 0;
            _maxThumbnails = CalculateMaxThumbnails();
            _numDisplayedThumbnails = (_maxThumbnails <= FDPV.PageCount) ?
                                                _maxThumbnails : FDPV.PageCount;

            for (int i = _currentThumbnail; i < _numDisplayedThumbnails; i++)
                AddPageThumb(i, true);

            for (int i = _numDisplayedThumbnails; i < FDPV.PageCount; i++)
                AddPageThumb(i, false); // Add empty thumb

            ThumbList.Items.MoveCurrentToPosition(_currentThumbnail);
            FDPV.Focus();
        }


        // --------------------------- AddPageThumb ---------------------------
        private void AddPageThumb(int pageIndex, bool isDisplayed)
        {
            Border thumbBorder = CreateThumbBorder(pageIndex + 1);
            if (isDisplayed)
            {
                // Get page visual and display it
                DocumentPage docPage =
                    FDPV.Document.DocumentPaginator.GetPage(pageIndex);
                StackPanel pageThumb =
                    CreatePageThumb(docPage.Visual, docPage.Size, pageIndex + 1);
                if (pageThumb != null)
                {
                    PrepareBorderForDisplay(thumbBorder);
                    thumbBorder.Child = pageThumb;
                }
            }
            ThumbList.Items.Add(thumbBorder);
        }


        // ---------------------- CalculateMaxThumbnails ----------------------
        private int CalculateMaxThumbnails()
        {
            int numRows = (int)(ThumbList.ActualHeight / (_thumbnailHeight));
            int numColumns = (int)(ThumbList.ActualWidth/ (_thumbnailWidth));
            return (Math.Max(numRows, 1) * Math.Max(numColumns, 1));
        }


        // -------------------------- CreatePageThumb -------------------------
        private StackPanel CreatePageThumb(
                                    Visual visual, System.Windows.Size size, int pageNumber)
        {
            if (visual != null)
            {
                Rect pageRect = new Rect(size);
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                        (int)pageRect.Width, (int)pageRect.Height,
                         96.0, 96.0, PixelFormats.Pbgra32 );
                renderTargetBitmap.Render(visual);

                StackPanel sp = new StackPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Source = renderTargetBitmap;
                img.Height = _thumbnailHeight;

                TextBlock tb = new TextBlock(new Run(pageNumber.ToString()));
                tb.FontSize = 10;

                sp.Orientation = Orientation.Vertical;
                sp.Children.Add(img);
                sp.Children.Add(tb);
                return sp;
            }
            return null;
        }


        // ----------------------- CreateThumbBorder --------------------------
        private Border CreateThumbBorder(int pageNumber)
        {
            Border OuterBorder = new Border();
            OuterBorder.BorderThickness = new Thickness(2);
            OuterBorder.Margin = new Thickness(3);
            OuterBorder.Width = _thumbnailWidth + _borderIncrement;
            OuterBorder.Height = _thumbnailHeight + _borderIncrement;
            OuterBorder.Background = System.Windows.Media.Brushes.White;
            OuterBorder.Tag = pageNumber;
            return OuterBorder;
        }


        // -------------------- PrepareBorderForDisplay -----------------------
        private void PrepareBorderForDisplay(Border border)
        {
            border.BorderBrush = System.Windows.Media.Brushes.Black;
            border.MouseLeftButtonDown +=
                new MouseButtonEventHandler(ThumbnailClick);

            // add ContextMenu.
            ContextMenu cm = new ContextMenu();
            MenuItem miReduce = new MenuItem();
            miReduce.Header = "Reduce Pa_ge Thumbnails";
            miReduce.Click += new RoutedEventHandler(miReduce_Click);
            MenuItem miEnlarge = new MenuItem();
            miEnlarge.Header = "En_large Page Thumbnails";
            miEnlarge.Click += new RoutedEventHandler(miEnlarge_Click);
            MenuItem miRotate = new MenuItem();
            miRotate.Header = "Rotate Page Thumbnails";
            miRotate.Click += new RoutedEventHandler(miRotate_Click);
            cm.Items.Add(miReduce);
            cm.Items.Add(miEnlarge);
            cm.Items.Add(new Separator());
            cm.Items.Add(miRotate);
            border.ContextMenu = cm;
        }// end:PrepareBorderForDisplay()


        // ---------------------------- OnSaveAs ------------------------------
        /// <summary>
        ///   Occurs when the user clicks File | Save As..."</summary>
        private void OnSaveAs(object target, ExecutedRoutedEventArgs args)
        {
            SaveDocumentAsFile(null);
        }


        // ----------------------- SaveDocumentAsFile -------------------------
        public bool SaveDocumentAsFile(string fileName)
        {
            // If no filename was specified, prompt the user for one.
            if (fileName == null)
            {
                // Create a File | Save As... dailog.
                Microsoft.Win32.SaveFileDialog dialog;
                dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.CheckFileExists = false;
                dialog.Filter = this.PlugInFileFilter +
                                "|XAML FlowDocument (*.xaml)|*.xaml" +
                                "|HTML Document (*.html; *.htm)|*.html; *.htm" +
                                "|WordXML Document (*.xml)|*.xml" +
                                "|RTF Document (*.rtf)|*.rtf" +
                                "|Plain Text (*.txt)|*.txt";

                // Display the dialog and wait for the user response.
                bool result = (bool)dialog.ShowDialog(null);

                // If the user clicked "Cancel", cancel the saving the file.
                if (result == false)  return false;
                fileName = dialog.FileName;
            }

            // Save the document to the specified file.
            return SaveToFile(fileName);
        }// end:SaveDocumentAsFile()


        // --------------------------- SaveToFile -----------------------------
        /// <summary>
        ///   Saves the current document to a specified file.</summary>
        /// <param name='fileName'>
        ///   The name of file to save to.</param>
        /// <returns>
        ///   true if the document was saved successfully; otherwise, false
        ///   if there was an error or the user canceled the save.</returns>
        private bool SaveToFile(string fileName)
        {
            if (fileName == null)  throw new ArgumentNullException(nameof(fileName));

            // If the file already exists, delete it (replace).
            if ( File.Exists(fileName) )  File.Delete(fileName);

            FlowDocument flowDocument = FDPV.Document as FlowDocument;
            string fileContent = null;
            try
            {
                // Create a SerializerProvider for accessing plug-in serializers.
                SerializerProvider serializerProvider = new SerializerProvider();

                // Locate the serializer that matches the fileName extension.
                SerializerDescriptor selectedPlugIn = null;
                foreach ( SerializerDescriptor serializerDescriptor in
                                serializerProvider.InstalledSerializers )
                {
                    if ( serializerDescriptor.IsLoadable &&
                         fileName.EndsWith(serializerDescriptor.DefaultFileExtension) )
                    {   // The plug-in serializer and fileName extensions match.
                        selectedPlugIn = serializerDescriptor;
                        break; // foreach
                    }
                }

                // If a match for a plug-in serializer was found,
                // use it to output and store the document.
                if (selectedPlugIn != null)
                {
                    Stream package = File.Create(fileName);
                    SerializerWriter serializerWriter =
                        serializerProvider.CreateSerializerWriter(selectedPlugIn,
                                                                  package);
                    IDocumentPaginatorSource idoc =
                        flowDocument as IDocumentPaginatorSource;
                    serializerWriter.Write(idoc.DocumentPaginator, null);
                    package.Close();
                    return true;
                }
               else if (fileName.EndsWith(".xml"))
                {
                    // Save as a WordXML document.
                    WordXmlSerializer.SaveToFile(fileName, flowDocument.ContentStart, flowDocument.ContentEnd);
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Error occurred during a conversion to this file format: " +
                    fileName + "\n" + e.ToString(), this.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (fileContent == null)
            {
                MessageBox.Show("A serializer for the given file extension" +
                    "could not be found.\n" + fileName, this.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Output the formatted content to the specified file.
            try
            {   // Write the file content.
                StreamWriter writer = new StreamWriter(fileName);
                writer.WriteLine(fileContent);
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show( "Error occurred during document save: " +
                    fileName + "\n" + e.ToString(), this.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }// end:SaveToFile()


        // ------------------------ PlugInFileFilter --------------------------
        /// <summary>
        ///   Gets a filter string for installed plug-in serializers.</summary>
        /// <remark>
        ///   PlugInFileFilter is used to set the SaveFileDialog or
        ///   OpenFileDialog "Filter" property when saving or opening files
        ///   using plug-in serializers.</remark>
        private string PlugInFileFilter
        {
            get
            {   // Create a SerializerProvider for accessing plug-in serializers.
                SerializerProvider serializerProvider = new SerializerProvider();
                string filter = "";

                // For each loadable serializer, add its display
                // name and extension to the filter string.
                foreach (SerializerDescriptor serializerDescriptor in
                    serializerProvider.InstalledSerializers)
                {
                    if (serializerDescriptor.IsLoadable)
                    {
                        // After the first, separate entries with a "|".
                        if (filter.Length > 0)   filter += "|";

                        // Add an entry with the plug-in name and extension.
                        filter += serializerDescriptor.DisplayName + " (*" +
                            serializerDescriptor.DefaultFileExtension + ")|*" +
                            serializerDescriptor.DefaultFileExtension;
                    }
                }

                // Return the filter string of installed plug-in serializers.
                return filter;
            }
        }

        #endregion Load and Save Documents


        #region Public Properties
        public IDocumentPaginatorSource Document
        {
            get { return FDPV.Document;  }
            set { FDPV.Document = value; }
        }
        #endregion Public Properties


        #region Private Fields
        private double _rotateAngle = 0;
        private int _thumbnailHeight = 74;
        private int _thumbnailWidth = 60;
        private int _maxThumbnails;
        private int _borderIncrement = 20;
        private int _currentThumbnail = 0;
        private int _numDisplayedThumbnails;
        internal static MethodInfo _reflectionTextPointer_InsertTextElement;

        // Bookmarks and comments
        AnnotationService _annServ = null;
        AnnotationStore _annStore = null;
        MemoryStream _annotationBuffer = null;

        // filename open
        string _fileName = "";
        #endregion Private Fields


        #region Event Handling

        // ----------------------- PaginationCompleted ------------------------
        void PaginationCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new DispatcherOperationCallback(
                    delegate { RefreshThumbnails(); return null; } ), null );
        }


        // ------------------------ SplitterEndResize -------------------------
        /// <summary>
        ///   Resizes Thumblist on Resizing Splitter.</summary>
        void SplitterEndResize(object sender, DragCompletedEventArgs e)
        {
            LeftTabControl.Width = MainGrid.ColumnDefinitions[0].ActualWidth;
            ThumbList.Width    = LeftTabControl.Width - 24;
            BookmarkList.Width = LeftTabControl.Width - 24;
            CommentsList.Width = LeftTabControl.Width - 24;
            foreach (StackPanel sp in BookmarkList.Items)
            {
                AdjustMarkSizes(sp);
            }

            foreach (StackPanel sp in CommentsList.Items)
            {
                AdjustMarkSizes(sp);
            }

            TrackerRect.Visibility = Visibility.Hidden;
        }


        // ------------------------- AdjustMarkSizes --------------------------
        private void AdjustMarkSizes(StackPanel sp)
        {
            sp.Width = BookmarkList.Width - 10;

            System.Windows.Shapes.Path path =
                LogicalTreeHelper.FindLogicalNode(sp, "MarkPath")
                    as System.Windows.Shapes.Path;

            IncrementPathData(path, sp.Width);
            Button btn = LogicalTreeHelper.FindLogicalNode(sp, "GoToMark") as Button;
            TextBlock tb = LogicalTreeHelper.FindLogicalNode(sp, "TB") as TextBlock;
            if (btn != null)
                btn.Width = sp.Width - 10;
            if (tb != null)
                tb.Width = btn.Width - 8;
        }


        // ------------------------ IncrementPathData -------------------------
        private void IncrementPathData(System.Windows.Shapes.Path path, double p)
        {
            if (path == null)  return;
            PathGeometry pGeom = path.Data as PathGeometry;

            if (pGeom == null) return;
            PathFigure pFig = pGeom.Figures[0] as PathFigure;

            // Left Top to Left Bottom
            LineSegment lSegLTLB = pFig.Segments[0] as LineSegment;
            lSegLTLB.Point = new System.Windows.Point(0, 24);

            // Left Bottom to Right Bottom
            LineSegment lSegLBRB = pFig.Segments[1] as LineSegment;
            lSegLBRB.Point = new System.Windows.Point(p - 10, 24);

            // Right Bottom to Right Top
            LineSegment lSegRBRT = pFig.Segments[2] as LineSegment;
            lSegRBRT.Point = new System.Windows.Point(p - 10, 7);

            // Right Top to Right Further Top
            LineSegment lSegRTRT = pFig.Segments[3] as LineSegment;
            lSegRTRT.Point = new System.Windows.Point(p - 2, 0);
        }


        // ------------------------- miEnlarge_Click --------------------------
        void miEnlarge_Click(object sender, RoutedEventArgs e)
        {
            _thumbnailHeight *= 2;
            _thumbnailWidth  *= 2;
            RefreshThumbnailSizes();
        }


        // ------------------------- miReduce_Click ---------------------------
        void miReduce_Click(object sender, RoutedEventArgs e)
        {
            _thumbnailHeight /= 2;
            _thumbnailWidth  /= 2;
            RefreshThumbnailSizes();
        }


        // ------------------------- miRotate_Click ---------------------------
        void miRotate_Click(object sender, RoutedEventArgs e)
        {
            _rotateAngle = (_rotateAngle + 90) % 360;
            RotateThumbnails();
        }


        // ---------------------- RefreshThumbnailSizes -----------------------
        private void RefreshThumbnailSizes()
        {
            // TODO: Refresh only displayed.
            foreach (Border OuterBorder in ThumbList.Items)
            {
                if (OuterBorder.Child != null)
                {
                    StackPanel sp = OuterBorder.Child as StackPanel;
                    System.Windows.Controls.Image img = sp.Children[0] as System.Windows.Controls.Image;
                    img.Height = _thumbnailHeight;
                }
                OuterBorder.Width = _thumbnailWidth + _borderIncrement;
                OuterBorder.Height = _thumbnailHeight + _borderIncrement;
            }

            // Refresh thumbnails
            RefreshThumbnails();
        }


        // ------------------------ RefreshThumbnails -------------------------
        private void RefreshThumbnails()
        {
            // Invalidate displayed thumbnails
            InvalidateDisplayedThumbs();
            _maxThumbnails = CalculateMaxThumbnails();
            if (_currentThumbnail > FDPV.PageCount)
            {
                if (_maxThumbnails <= FDPV.PageCount)
                {
                    _currentThumbnail = FDPV.PageCount - _maxThumbnails;
                    _numDisplayedThumbnails = _maxThumbnails;
                }
                else
                {
                    _currentThumbnail = 0;
                    _numDisplayedThumbnails = FDPV.PageCount;
                }

                ThumbList.Items.MoveCurrentToPosition(_currentThumbnail);
            }
            else
            {
                if (_currentThumbnail + _maxThumbnails <= FDPV.PageCount)
                {
                    _numDisplayedThumbnails = _maxThumbnails;
                }
                else
                {
                    _numDisplayedThumbnails = FDPV.PageCount - _currentThumbnail;
                }
            }
            RefreshThumbnailDisplay();
        }


        // --------------------- RefreshThumbnailDisplay ----------------------
        void RefreshThumbnailDisplay()
        {
            //FDPV.Focus();
            bool createMode =
                (ThumbList.Items.CurrentItem is Border) ?  false : true;

            for (int i = 0; i < _numDisplayedThumbnails; i++)
            {
                DocumentPage docPage =
                    FDPV.Document.DocumentPaginator.GetPage(_currentThumbnail + i);

                StackPanel pageThumb =
                    CreatePageThumb(
                        docPage.Visual, docPage.Size, _currentThumbnail + i + 1 );

                if (!createMode)
                {
                    Object currentItem = ThumbList.Items.CurrentItem;
                    if ( !(currentItem is Border) )  continue;
                    PrepareBorderForDisplay((Border)currentItem);
                    ((Border)currentItem).Child = pageThumb;
                }
                else
                {
                    Border border = CreateThumbBorder( _currentThumbnail + i + 1);
                    ThumbList.Items.Add(border);
                    ThumbList.Items.MoveCurrentToNext();
                    if ( !(ThumbList.Items.CurrentItem is Border) )  continue;
                    PrepareBorderForDisplay(border);
                    border.Child = pageThumb;
                    continue;
                }

                // If ThumbList doesn't have enough items,
                // the FixedDocument pages may have increased.
                if ( !ThumbList.Items.MoveCurrentToNext() )  createMode = true;
            }

            for (int i = _numDisplayedThumbnails; i > 0; i--)
            {
                ThumbList.Items.MoveCurrentToPrevious();
            }
        }// end:RefreshThumbnailDisplay()


        // ------------------- InvalidateDisplayedThumbs() --------------------
        void InvalidateDisplayedThumbs()
        {
            // TODO: Can this be combined with the refresh operation?  The only
            // issue to deal with is that number of displayed thumbnails may be
            // different and if less than before we have some more invalidations
            // to carry out.
            for (int i = 0; i < _numDisplayedThumbnails; i++)
            {
                Object currentItem = ThumbList.Items.CurrentItem;
                Debug.Assert(currentItem is Border);
                Border border = (Border)currentItem;
                if (border == null) continue; // for (int i=0
                border.Child = null;
                border.BorderBrush = null;
                border.ContextMenu = null;
                border.MouseDown -= ThumbnailClick;
                ThumbList.Items.MoveCurrentToNext();
            }

            // Go back to the current position
            for (int i = _numDisplayedThumbnails; i > 0; i--)
            {
                ThumbList.Items.MoveCurrentToPrevious();
            }
        }// end:InvalidateDisplayedThumbs()


        // ----------------------- RotateThumbnails() -------------------------
        private void RotateThumbnails()
        {
            foreach (Border OuterBorder in ThumbList.Items)
            {
                StackPanel sp = OuterBorder.Child as StackPanel;
                System.Windows.Controls.Image img = sp.Children[0] as System.Windows.Controls.Image;
                RotateTransform rt = new RotateTransform(_rotateAngle);
                img.LayoutTransform = rt;
            }
        }


        // ------------------------ ThumbnailClick() --------------------------
        void ThumbnailClick(object sender, MouseButtonEventArgs e)
        {
            Border OuterBorder = sender as Border;
            //TrackerRect.Height = OuterBorder.Height;
            //TrackerRect.Width = OuterBorder.Width;
            //Point pt = OuterBorder.TranslatePoint(new Point(0,0), ThumbList);
            //RectGeom.Rect = new Rect(pt.X + OuterBorder.Margin.Left,
            //                         pt.Y + OuterBorder.Margin.Top,
            //                         OuterBorder.Width, OuterBorder.Height);
            //TrackerRect.Visibility = Visibility.Visible;
            FDPV.GoToPage(int.Parse(OuterBorder.Tag.ToString()));
        }


        // ------------------------ OnAddBookmark() ---------------------------
        void OnAddBookmark(object sender, RoutedEventArgs args)
        {
            try
            {
                System.Windows.Media.Color col = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#A6FFFF00");
                System.Windows.Media.Brush myBrush = new SolidColorBrush(col);
                string userName = System.Windows.Forms.SystemInformation.UserName;
                AnnotationHelper.CreateHighlightForSelection(
                                            _annServ, userName, myBrush);
            }
            catch (InvalidOperationException)
            {
                return;
            }
            //AddBookmarkOrComment(BookmarkList);
        }


        // -------------------------- OnAddComment ----------------------------
        void OnAddComment(object sender, RoutedEventArgs args)
        {
            try
            {
                string userName = System.Windows.Forms.SystemInformation.UserName;
                AnnotationHelper.CreateTextStickyNoteForSelection(
                                                            _annServ, userName);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            //AddBookmarkOrComment(CommentsList);
            //Annotation ann1 = _annStore.GetAnnotations()[0];
            //ColorConverter converter = new ColorConverter();
            //Nullable<Color> color = ((SolidColorBrush)Brushes.Yellow).Color;
            //ann1.Cargos[0].Contents[0].Attributes[0].Value =
            //    converter.ConvertToInvariantString(color.Value);
        }


        // ------------------ _annStore_StoreContentChanged -------------------
        void _annStore_StoreContentChanged(object sender, StoreContentChangedEventArgs e)
        {
            if (e.Action == StoreContentAction.Deleted) return;
            Annotation ann = e.Annotation;
            if (ann.Cargos.Count > 0)
            {
                AnnotationResource annResource = ann.Cargos[0] as AnnotationResource;
                if (annResource.Name == "Highlight")
                    AddBookmarkOrComment(BookmarkList, ann);
                else
                    AddBookmarkOrComment(CommentsList, ann);
            }
            else
            {
                AddBookmarkOrComment(CommentsList, ann);
            }
        }


        // ----------------------- AddBookmarkOrComment -----------------------
        private void AddBookmarkOrComment(ListBox collection, Annotation ann)
        {
            if (ann.Cargos.Count <= 1)
            {
                ann.Cargos.Add(
                    new AnnotationResource( FDPV.MasterPageNumber.ToString() ) );
            }

            Assembly a = System.Reflection.Assembly.GetExecutingAssembly();

            string path = System.IO.Path.Combine(
                a.Location.Remove(a.Location.LastIndexOf('\\')), "GoButton.xaml");

            StackPanel EntryInList =
                XamlReader.Load(File.OpenRead(path)) as StackPanel;

            EntryInList.Width = BookmarkList.Width - 10;

            Button GoToMark = LogicalTreeHelper.FindLogicalNode(
                                            EntryInList, "GoToMark") as Button;

            if (GoToMark != null)
            {
                GoToMark.Tag = ann;
                GoToMark.Click += new RoutedEventHandler(GoToMark_Click);
            }

            MenuItem GoToMenu = LogicalTreeHelper.FindLogicalNode(
                                  GoToMark.ContextMenu, "GoToMenu") as MenuItem;

            GoToMenu.Click += new RoutedEventHandler(GoToMark_Click);
            GoToMenu.Tag = ann;

            MenuItem DeleteMark = LogicalTreeHelper.FindLogicalNode(
                                  GoToMark.ContextMenu, "DeleteMark") as MenuItem;

            DeleteMark.Click += new RoutedEventHandler(DeleteMark_Click);
            DeleteMark.Tag = ann;

            System.Windows.Shapes.Path markPath =
                LogicalTreeHelper.FindLogicalNode(EntryInList, "MarkPath")
                    as System.Windows.Shapes.Path;

            if ( (collection == CommentsList) && (markPath != null) )
            {
                LinearGradientBrush lBrush = new LinearGradientBrush();
                GradientStopCollection gColl = new GradientStopCollection();
                GradientStop gStop = new GradientStop(Colors.LightGreen, 0);
                gColl.Add(gStop);
                lBrush.GradientStops = gColl;
                markPath.Fill = lBrush;
            }

            collection.Items.Add(EntryInList);

            TextBlock spText =
                LogicalTreeHelper.FindLogicalNode(EntryInList, "TB") as TextBlock;

            string MarkText = "";
            if (spText != null)
            {
                ContentLocator cloc =
                    ann.Anchors[0].ContentLocators[0] as ContentLocator;
                if (cloc == null)         return;
                if (cloc.Parts.Count < 2) return;

                ContentLocatorPart cPart = cloc.Parts[1];
                if (cPart == null)        return;
                if (cPart.NameValuePairs["Segment0"] != null)
                {
                    string[] charPos = cPart.NameValuePairs["Segment0"].Split(',');
                    FlowDocument fd = FDPV.Document as FlowDocument;
                    TextPointer tp = fd.ContentStart.GetPositionAtOffset(
                        int.Parse(charPos[0]), LogicalDirection.Forward);

                    if (tp == null)       return;
                    if (   tp.GetPointerContext(LogicalDirection.Forward)
                        == TextPointerContext.Text )
                    {
                        MarkText += tp.GetTextInRun(LogicalDirection.Forward);
                    }
                    spText.Text = MarkText.Substring( 0,
                        (MarkText.Length > 150) ? 150 : MarkText.Length );
                }
            }
        }// end:AddBookmarkOrComment()


        // ------------------------- DeleteMark_Click -------------------------
        void DeleteMark_Click(object sender, RoutedEventArgs e)
        {
            Annotation ann = ((MenuItem)sender).Tag as Annotation;
            _annStore.DeleteAnnotation(ann.Id);
            _annStore.Flush();

            MenuItem thisMenu = sender as MenuItem;
            ContextMenu parentMenu = thisMenu.Parent as ContextMenu;
            FrameworkElement dObj =
                parentMenu.PlacementTarget as FrameworkElement;
            while (!(dObj is StackPanel))
            {
                dObj = dObj.Parent as FrameworkElement;
            }

            ListBox collection = dObj.Parent as ListBox;
            collection.Items.Remove(dObj);
            Util.FlushDispatcher();
        }


        // -------------------------- GoToMark_Click --------------------------
        void GoToMark_Click(object sender, RoutedEventArgs e)
        {
            Annotation ann = null;
            if (sender is Button)
                ann = ((Button)sender).Tag as Annotation;
            else if (sender is MenuItem)
                ann = ((MenuItem)sender).Tag as Annotation;
            if (ann == null)          return;

            ContentLocator cloc =
                ann.Anchors[0].ContentLocators[0] as ContentLocator;
            if (cloc == null)         return;
            if (cloc.Parts.Count < 2) return;

            ContentLocatorPart cPart = cloc.Parts[1];
            if (cPart == null)        return;
            if (cPart.NameValuePairs["Segment0"] != null)
            {
                string[] charPos = cPart.NameValuePairs["Segment0"].Split(',');
                FlowDocument fd = FDPV.Document as FlowDocument;
                TextPointer tp = fd.ContentStart.GetPositionAtOffset(
                                 int.Parse(charPos[0]), LogicalDirection.Forward);
                if (tp == null)   return;

                FrameworkContentElement fce = tp.Parent as FrameworkContentElement;
                if (fce == null)  return;

                fce.BringIntoView();
            }
        }


        // ---------------------- SetStatusOfContextMenu ----------------------
        void SetStatusOfContextMenu(object sender, RoutedEventArgs args)
        {
            FlowDocument fd = sender as FlowDocument;
            if (fd == null) return;

            cm_Bookmark.IsEnabled = true;
        }


        private static double _articleZoomValue = 110;

        // ------------------------- ZoomSlider_Loaded ------------------------
        /// <summary>
        ///   Event Handler for the ZoomSlider's Loaded event.
        ///     Sets the slider's Value property to the
        ///     application "ArticleZoomValue" property.</summary>
        private void ZoomSlider_Loaded(object sender, RoutedEventArgs e)
        {
            Slider zoomSlider = sender as Slider;
            if (zoomSlider != null)  zoomSlider.Value = _articleZoomValue;
        }


        // ---------------------- ZoomSlider_ValueChanged ---------------------
        /// <summary>
        /// Event Handler for the ZoomSlider's ValueChanged event.
        ///   Updates the application "ArticleZoomValue" property
        ///   with new value of slider.</summary>
        private void ZoomSlider_ValueChanged(
            object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider zoomSlider = sender as Slider;

            if (zoomSlider != null)
            {
                double newValue = e.NewValue;
                double oldValue = e.OldValue;

                //NOTE: We have to do a check to make sure old value is greater
                //      than zero.  This only occurs when the slider is being
                //      initialized.  Basically we don't want to set the
                //      ArticleZoomValue property BEFORE ZoomSlider_Loaded is
                //      called.  There might be a better way of guarranting this.
                if ( (newValue != oldValue) && (oldValue > 0) )
                    _articleZoomValue = e.NewValue;
            }
        }// end:ZoomSlider_ValueChanged()

        #endregion Event Handling

    }// end:partial class ThumbViewer

}// end:namespace DocumentSerialization

