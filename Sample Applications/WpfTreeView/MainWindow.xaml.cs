using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _LastMousePos = new Point(0, 0);

        #region Public Variables
        private List<TreeViewItem> _VisibleItems = new List<TreeViewItem>();

        public bool isCut = false;

        /// <summary>
        /// Store the Selected TreeView Item
        /// </summary>
        public Dictionary<TreeViewItem, string> selectedItems =
            new Dictionary<TreeViewItem, string>();
        private TreeViewItem lastSelectedItem = null;

        /// <summary>
        /// Dictionary for known files. If python is known, it assigns in logo as a python file.
        /// </summary>
        public IDictionary<string, ImageSource> KnownFiles { get; set; }

        /// <summary>
        /// Store the mouse button clicked last time
        /// </summary>
        Point _lastMouseDown;

        /// <summary>
        /// Item which has been dragged
        /// </summary>
        IEnumerable<TreeViewItem> draggedItem;

        /// <summary>
        /// Target location of the Item
        /// </summary>
        TreeViewItem _target;


        #endregion

        #region Copy Operation
        public ICommand CopyCommand { get; set; }

        public void ExecuteCopy(object sender)
        {
            // Clear Clipboard
            Clipboard.Clear();

            isCut = false;
            var stringCollection = new StringCollection();

            foreach (string path in selectedItems.Values)
            {
                stringCollection.Add(path);
            }

            if (stringCollection != null)
            {
                Clipboard.SetFileDropList(stringCollection);
                (PasteCommand as RelayCommand).NotifyCanExecuteChanged(sender);
            }
        }

        public bool CanCopy => selectedItems != null;
        #endregion

        #region Cut Operation

        public ICommand CutCommand { get; set; }

        public void ExecuteCut(object sender)
        {
            // Clear Clipboard
            Clipboard.Clear();

            isCut = true;
            var stringCollection = new StringCollection();

            foreach (string path in selectedItems.Values)
            {
                stringCollection.Add(path);
            }

            if (stringCollection != null)
            {
                Clipboard.SetFileDropList(stringCollection);
                var temp = Clipboard.GetFileDropList();
                (PasteCommand as RelayCommand).NotifyCanExecuteChanged(sender);
            }
        }

        public bool CanCut => selectedItems != null;

        #endregion

        #region Paste Operation
        public ICommand PasteCommand { get; set; }

        /// <summary>
        /// Execute Paste Operation
        /// </summary>
        /// <param name="sender"></param>
        public void ExecutePaste(object sender)
        {
            // Get Tree View
            var treeview = FolderView;

            if (selectedItems.Count <= 0)
                return;
            try
            {
                foreach (var KV in selectedItems)
                {
                    var path = KV.Value;
                    var item = KV.Key;
                    var files = Clipboard.GetFileDropList();
                    foreach (var file in files)
                    {
                        var newPath = Path.Combine(path, Path.GetFileName(file));
                        var backupPath = Path.ChangeExtension(newPath, Path.GetExtension(newPath) + ".bak");

                        if (File.Exists(newPath))
                        {
                            if (MessageBox.Show("The" + Path.GetFileName(newPath) + " already exist. Would you like to replace this file? ", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                // Replace the file
                                File.Delete(newPath);
                                File.Copy(file, newPath);
                            }
                        }
                        else
                        {
                            if (isCut)
                                File.Move(file, newPath);
                            else
                                File.Copy(file, newPath);
                        }
                    }
                }
            }
            catch (Exception e) { MessageBox.Show("Paste Operation Failed : " + e.Message.ToString()); }

            RefreshViews();
        }
        /// <summary>
        /// If there are some files in clipboard and file count > 0 then allow paste operation
        /// </summary>
        public bool CanPaste => Clipboard.GetFileDropList() != null && Clipboard.GetFileDropList().Count > 0;
        #endregion

        #region Open Containing folder in Windows Explorer

        /// <summary>
        /// Command to open a file or folder containing folder
        /// </summary>
        public ICommand OpenExplorerCommand { get; set; }

        /// <summary>
        /// Execute command to open the file or folder containing folder
        /// </summary>
        /// <param name="sender"></param>
        public void ExecuteOpeningExplorer(object sender)
        {
            if (selectedItems.Count != 1)
                return;

            // starts a new process of explorer.exe
            Process.Start("explorer.exe", selectedItems.Values.FirstOrDefault().ToString());
        }

        /// <summary>
        /// If only one is selected, then only open the folder otherwise not
        /// </summary>
        public bool CanOpenExplorer => selectedItems.Count == 1 && selectedItems.Values != null;

        #endregion

        #region Hard Refresh Command, recreates the treeview from scratch

        /// <summary>
        /// Hard refresh command
        /// </summary>
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// Execute the hard refresh command
        /// </summary>
        /// <param name="sender"></param>
        public void ExecuteRefresh(object sender)
        {
            FolderView.Items.Clear();
            Window_Loaded(null, null);
        }

        /// <summary>
        /// Can be refreshed without any condition
        /// </summary>
        public bool CanRefresh => true;

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            // Copy and Paste command initialized
            CopyCommand = new RelayCommand(ExecuteCopy, (_) => CanCopy);
            PasteCommand = new RelayCommand(ExecutePaste, (_) => CanPaste);
            CutCommand = new RelayCommand(ExecuteCut, (_) => CanCut);
            OpenExplorerCommand = new RelayCommand(ExecuteOpeningExplorer, (_) => CanOpenExplorer);
            RefreshCommand = new RelayCommand(ExecuteRefresh, (_) => CanRefresh);

            InitializeComponent();

            DataContext = this;

            // Add known files such as .py into known Dictionary to recoginize that file
            KnownFiles = new Dictionary<string, ImageSource>();
            KnownFiles.Add(".py", Resources["PythonIcon"] as BitmapImage);
            KnownFiles.Add(".cs", Resources["CSharpIcon"] as BitmapImage);
            KnownFiles.Add(".c", Resources["CIcon"] as BitmapImage);
            KnownFiles.Add(".cpp", Resources["CppIcon"] as BitmapImage);
            KnownFiles.Add(".java", Resources["JavaIcon"] as BitmapImage);
            KnownFiles.Add(".js", Resources["JavascriptIcon"] as BitmapImage);
            KnownFiles.Add(".txt", Resources["TextIcon"] as BitmapImage);
            KnownFiles.Add(".json", Resources["JsonIcon"] as BitmapImage);
            KnownFiles.Add(".xlsx", Resources["XslxIcon"] as BitmapImage);
            KnownFiles.Add(".pdf", Resources["PdfIcon"] as BitmapImage);
            (Resources["ImageConverter"] as HeaderToImageConverter).KnownFiles = KnownFiles;
        }
        #endregion

        #region On Loaded
        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Sender Object event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _VisibleItems.Clear();

            // Get every logical drives on the machine
            foreach (var drive in Directory.GetLogicalDrives())
            {
                // create a new item for it
                var item = new TreeViewItem()
                {
                    // set the header
                    Header = drive,
                    // Set the full path
                    Tag = drive
                };
                item.AllowDrop = true;
                item.DragOver += MyTreeViewItem_DragOver;
                item.MouseDown += MyTreeViewItem_MouseDown;
                item.MouseMove += MyTreeViewItem_MouseMove;
                item.Drop += MyTreeViewItem_Drop;


                // Add a dummy item
                item.Items.Add(null);

                // Listen for the item being expanded
                item.Expanded += Folder_Expanded;

                // Add it to main tree view
                FolderView.Items.Add(item);

                _VisibleItems.Add(item);
            }
        }
        #endregion

        #region folder expanded
        /// <summary>
        /// When the folder is expanded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            // if the item only contains the dummy data
            if (item.Items.Count != 1 && item.Items[0] != null)
                return;

            // clear dummy data
            item.Items.Clear();

            // Get full path
            var fullPath = (string)item.Tag;

            var itemIdx = _VisibleItems.IndexOf(item);

            #endregion

            #region Get Directories

            // Create a blank list
            var directories = new ObservableCollection<string>();

            // try and get directories from the folder
            // ignoring any issues doing so
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    foreach (var dir in dirs)
                        directories.Add(dir);
            }
            catch { }

            foreach (var directoryPath in directories)
            {
                // Create Directory
                var subItem = new TreeViewItem()
                {
                    // Set header as folder name
                    Header = GetFileFolderName(directoryPath),
                    // And tag as full path
                    Tag = directoryPath
                };
                subItem.AllowDrop = true;

                // Add dummy item so we can expand folder
                subItem.Items.Add(null);
                subItem.DragOver += MyTreeViewItem_DragOver;
                subItem.MouseDown += MyTreeViewItem_MouseDown;
                subItem.MouseMove += MyTreeViewItem_MouseMove;
                subItem.Drop += MyTreeViewItem_Drop;

                // Handle expanding
                subItem.Expanded += Folder_Expanded;

                // Add this item to parent
                item.Items.Add(subItem);

                _VisibleItems.Insert(++itemIdx, subItem);
            }

            #endregion

            #region Get Files
            // Create a blank list
            var files = new ObservableCollection<string>();

            // try and get files f1rom the folder
            // ignoreing any issues doing so
            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    foreach (var f in fs)
                        files.Add(f);
            }
            catch { }

            foreach (var filePath in files)
            {
                // Create file
                var subItem = new TreeViewItem()
                {
                    // Set header as file name
                    Header = GetFileFolderName(filePath),
                    // And tag as full path
                    Tag = filePath
                };
                subItem.AllowDrop = true;
                subItem.DragOver += MyTreeViewItem_DragOver;
                subItem.MouseDown += MyTreeViewItem_MouseDown;
                subItem.MouseMove += MyTreeViewItem_MouseMove;
                subItem.Drop += MyTreeViewItem_Drop;

                // Add this item to parent
                item.Items.Add(subItem);

                _VisibleItems.Insert(++itemIdx, subItem);
            }


            #endregion            
        }

        #endregion

        #region Get File or Folder name
        /// <summary>
        /// Find the file or folder name from the full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            // if we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Make all slashes back slashes
            var normalizePath = path.Replace('/', '\\');

            // find the last back slash in the path
            var lastIndex = normalizePath.LastIndexOf('\\');

            // if we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            // return name after the last back slash
            return path.Substring(lastIndex + 1);
        }

        #endregion

        #region Folder View Selecting items for copy paste operations
        /// <summary>
        /// Triggers if selection is changed in the folder view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (CopyCommand as RelayCommand).NotifyCanExecuteChanged(this);
            (CutCommand as RelayCommand).NotifyCanExecuteChanged(this);

            TreeViewItem treeViewItem = FolderView.SelectedItem as TreeViewItem;

            if (treeViewItem == null)
                return;

            // prevent the WPF tree item selection 
            treeViewItem.IsSelected = false;

            treeViewItem.Focus();


            #region Multi-Select with Control Key
            // if the Ctrl key is pressed, then change the selected stated
            if (Keyboard.Modifiers == ModifierKeys.Control)
                if (Directory.Exists(treeViewItem.Tag.ToString()))
                {
                    // If it is directory, add all containing child items in the selection list
                    foreach(var childItems in treeViewItem.Items)
                    {
                        ChangeSelectedState(childItems as TreeViewItem);
                    }

                }
                else
                {
                    ChangeSelectedState(treeViewItem);
                }


            #endregion

            #region If No modifier Key is selected
            // If no special key is selected
            if (Keyboard.Modifiers == ModifierKeys.None)
            {
                // create a list of tree view items for keep track of the selected files
                List<TreeViewItem> selectedTreeViewItemList = new List<TreeViewItem>();

                // for each treeview item in selected items, add all the items to the selection list
                foreach (TreeViewItem treeViewItem1 in selectedItems.Keys)
                {
                    selectedTreeViewItemList.Add(treeViewItem1);
                }
                // Deselect the selected items
                foreach (TreeViewItem treeViewItem1 in selectedTreeViewItemList)
                {
                    Deselect(treeViewItem1);
                }

                // only add the item which is selected to the list
                ChangeSelectedState(treeViewItem);
            }
            #endregion

            #region Multi-Select operation with Shift
            // when the shift key is pressed
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                // make sure the last selected item is not null
                if (lastSelectedItem != null)
                {
                    bool select = false;
                    bool foundBoth = false;
                    var toSelect = new List<TreeViewItem>();

                    // create a new stack of tree view items
                    var stack = new Stack<TreeViewItem>();

                    // push all the selected items in the stack
                    foreach (TreeViewItem item in FolderView.Items)
                        stack.Push(item);

                    // Until there is no element left in the stack and 
                    // both starting and ending elements are not found,
                    // it remains in the while loop
                    // If both of the elements are found, leave the while loop
                    while (stack.Count > 0 && !foundBoth)
                    {
                        // take each item in the stack one by one
                        var item = stack.Pop();

                        // if either of the elements (starting or ending) is found
                        if (item == lastSelectedItem || item == treeViewItem)
                        {
                            // set the select variable to true if
                            // anyone of the element is founnd
                            if (!select)
                                select = true;
                            // if both the elements are found, set foundboth to true
                            else
                                foundBoth = true;
                        }

                        // if anyone of the element is found, from that point on, add all the elements until it finds the other element
                        if (select)
                            toSelect.Add(item);

                        // if there is still something in the list and the folder is expanded
                        if (item.Items.Count > 0 && item.IsExpanded == true)
                        {
                            // Push all the elements to the stack
                            foreach (TreeViewItem subItem in item.Items)
                                stack.Push(subItem);
                        }

                    }

                    foreach (var item in toSelect)
                        Select(item);
                }
            }
            #endregion

        }

        #region General Controls with Special keys

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Up)
            {

                if (lastSelectedItem != null)
                {

                    var itemIdx = _VisibleItems.IndexOf(lastSelectedItem);
                    var nextIdx = itemIdx - 1;
                    if (nextIdx >= 0)
                    {
                        SelectOnly(_VisibleItems[nextIdx]);
                    }
                }

                e.Handled = true;
            }

            if (e.Key == Key.Down)
            {
                if (lastSelectedItem != null)
                {

                    var itemIdx = _VisibleItems.IndexOf(lastSelectedItem);
                    var nextIdx = itemIdx + 1;
                    if (nextIdx < _VisibleItems.Count)
                    {
                        SelectOnly(_VisibleItems[nextIdx]);
                    }
                }

                e.Handled = true;
            }

            if (e.Key == Key.Delete)
            {
                // if no item to delete selected, then return
                if (selectedItems.Count <= 0)
                    return;

                // Ask if you really want to delete the selected items
                if (MessageBox.Show("Do you really want to delete file/files ? ", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // if yes, delete all the selected items
                    foreach (var path in selectedItems.Values)
                    {
                        File.Delete(path);
                    }
                }

                // Refresh the view after deleting views
                RefreshViews();
            }
        }

        #endregion

        /// <summary>
        /// De-Select the tree View Item
        /// </summary>
        /// <param name="treeViewItem"></param>
        void Deselect(TreeViewItem treeViewItem)
        {
            treeViewItem.Background = Brushes.White;// change background and foreground colors
            treeViewItem.Foreground = Brushes.Black;
            selectedItems.Remove(treeViewItem); // remove the item from the selected items set
            lastSelectedItem = null;
        }

        void Select(TreeViewItem treeViewItem)
        {
            if (!selectedItems.ContainsKey(treeViewItem))
            {
                treeViewItem.Background = Brushes.RoyalBlue; // change background and foreground colors
                treeViewItem.Foreground = Brushes.White;
                selectedItems.Add(treeViewItem, treeViewItem.Tag.ToString()); // add the item to selected items
            }

            lastSelectedItem = treeViewItem;
        }

        void SelectOnly(TreeViewItem treeViewItem)
        {


            // if shift button is pressed, dont deselect
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                Select(treeViewItem);
            }
            else
            {
                foreach (TreeViewItem item in selectedItems.Keys)
                {
                    Deselect(item);
                }
                Select(treeViewItem);
            }
        }

        /// <summary>
        /// changes the state of the tree item:
        /// selects it if it has not been selected and
        /// deselects it otherwise
        /// </summary>
        /// <param name="treeViewItem"></param>
        void ChangeSelectedState(TreeViewItem treeViewItem)
        {
            if (!selectedItems.ContainsKey(treeViewItem))
            { // select
                Select(treeViewItem);
            }
            else
            { // deselect
                Deselect(treeViewItem);
            }
        }


        /// <summary>
        /// return true is control is pressed
        /// </summary>

        bool CtrlPressed => System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl);

        bool ShiftPressed => System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift);
        #endregion

        #region Drag and Drop Utility functions
        /// <summary>
        /// This method get the position of the mouse cursor 
        /// when the left mouse button is clicked
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Mouse Button Event Argument</param>
        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(FolderView);
            }
        }

        /// <summary>
        /// Once the left mouse button is click and moved, this method executes
        /// </summary>
        /// <param name="sender"> Object Sender</param>
        /// <param name="e">Mouse Event Argument</param>
        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // If left mouse is pressed
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // get the current position in the tree view
                    Point currentPosition = e.GetPosition(FolderView);
                    draggedItem = selectedItems.Keys.ToArray();

                    // There is no item selected to drag, then return
                    if (draggedItem == null)
                        return;

                    // if the mouse cursor is moved more than 10 pixels from the previous position,
                    // it is considered that the item is dragged
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        foreach (TreeViewItem item in draggedItem)
                        {
                            if (selectedItems.Count != 0)
                            {
                                DragDropEffects finalDropEffects = DragDrop.DoDragDrop(FolderView, item.Header.ToString(), DragDropEffects.Move);

                                if ((finalDropEffects == DragDropEffects.Move) && (_target != null))
                                {
                                    draggedItem = selectedItems.Keys.ToArray();

                                    if (!item.Header.ToString().Equals(_target.Header.ToString()))
                                    {
                                        CopyItem(item, _target);
                                    }
                                    draggedItem = null;
                                    _target = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // In case exception happens, do nothing
            }
        }

        private void MyTreeViewItem_DragOver(object sender, DragEventArgs e)
        {
            if(sender is TreeViewItem item)
            {
                if(e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if(Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void MyTreeViewItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                _LastMousePos = e.GetPosition(item);
            }
        }

        private void MyTreeViewItem_MouseMove(object sender, MouseEventArgs e)
        {

            if (sender is TreeViewItem item && e.LeftButton == MouseButtonState.Pressed)
            {
                var currPos = e.GetPosition(item);

                var dx = currPos.X - _LastMousePos.X;
                var dy = currPos.Y - _LastMousePos.Y;

                if(Math.Sqrt(dx * dx * + dy * dy) > 20.0)
                {
                    DragDrop.DoDragDrop(item, new DataObject(DataFormats.FileDrop, new string[] { item.Tag as string }), DragDropEffects.All);
                    Debug.WriteLine("DROP");
                }
            }

        }

        private void MyTreeViewItem_Drop(object sender, DragEventArgs e)
        {
            if (sender is TreeViewItem item && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var targetDir = item.Tag as string;
                var sourcePaths = e.Data.GetData(DataFormats.FileDrop) as string[];

                while(targetDir != null && targetDir != "" && !Directory.Exists(targetDir))
                {
                    targetDir = Path.GetDirectoryName(targetDir);
                }

                if (sourcePaths == null || targetDir == null || targetDir == "")
                    return;

                foreach(var sourcePath in sourcePaths)
                {
                    var targetPath = Path.Combine(targetDir, Path.GetFileName(sourcePath));

                    if (targetPath == sourcePath)
                        continue;

                    if(File.Exists(targetPath))
                    {
                        var ret = MessageBox.Show($"File {targetPath} already exists. Overwrite?", "File Exists", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (ret == MessageBoxResult.No)
                            continue;
                    }

                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        File.Copy(sourcePath, targetPath, true);
                    }
                    else
                    {
                        File.Move(sourcePath, targetPath, true);
                    }
                }

                //RefreshViews();  
            }

        }

        /// <summary>
        /// When the mouse cursor is dragged over some item
        /// </summary>
        /// <param name="sender">bject Sender </param>
        /// <param name="e"> Drag Event </param>
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                var myitem = sender as TreeViewItem;

                // get current position of the cursor
                Point currentPosition = e.GetPosition(FolderView);

                // if the mouse cursor is moved more than 10 pixels from the previous position,
                // it is considered that the item is dragged
                if (Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0 ||
                    Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0)
                {
                    // Verify that this is a valid drop and then store the drop target
                    TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);

                    // If the dragged item list is null
                    if (draggedItem == null)
                        // TODO: Get the item as source and target which is outside the wpf app and copy the item and refresh the view

                        return;

                    foreach (TreeViewItem tvItem in draggedItem)
                    {
                        if (CheckDropTarget(tvItem, item))
                        {
                            e.Effects = DragDropEffects.Move;
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                        }
                    }

                    // Clear Clipboard
                    Clipboard.Clear();

                    var stringCollection = new StringCollection();

                    foreach (TreeViewItem tvItem in draggedItem)
                    {
                        stringCollection.Add(tvItem.Tag.ToString());
                    }

                    if (stringCollection != null)
                    {
                        Clipboard.SetFileDropList(stringCollection);
                    }
                }
                e.Handled = true;

                RefreshViews();
            }
            catch (Exception)
            {
                // In case of exception, do nothing
            }
        }

        /// <summary>
        /// This method is executed when the item is dropped over some other item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Drop(object sender, DragEventArgs e)
        {
            /*
             * File dropping from file explorer to the tree view implemented and working
             * TODO : Implementation of file dropping outside the wpf application
             * */
            try
            {
                e.Effects = DragDropEffects.None;

                // Verify that this is a valid drop and then store the drop target
                TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);

                if (TargetItem != null && draggedItem != null)
                {
                    _target = TargetItem;
                    e.Effects = DragDropEffects.Move;
                }

                // Get Tree View
                var treeview = FolderView;

                // If the dragged item list is null, then check if the file to be dropped is there or not
                if (draggedItem == null)
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        // Get the files to drop which is coming from external source
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                        // foreach external files, modify the path string and copy the file to new location
                        foreach (var f in files)
                        {
                            var newPath = Path.Combine(TargetItem.Tag.ToString(), Path.GetFileName(f));
                            File.Copy(f, newPath);
                        }

                        // Refresh treeview to update changes
                        RefreshViews();
                        return;
                    }

                    return;
                }

                foreach (TreeViewItem tvItem in draggedItem)
                {
                    var files = Clipboard.GetFileDropList();
                    foreach (var file in files)
                    {
                        var newPath = Path.Combine(_target.Tag.ToString(), Path.GetFileName(file));
                        File.Move(file, newPath);
                    }
                }

                RefreshViews();
            }
            catch (Exception)
            {
                // In case of exception, do nothing
            }
        }


        private bool CheckDropTarget(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            // Check whether the target item is meeting the conditions
            bool _isEqual = false;

            if (!_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
            {
                _isEqual = true;
            }
            return _isEqual;
        }


        /// <summary>
        /// Copy Item from source location to destination location
        /// </summary>
        /// <param name="draggedItem">Item which is dragged</param>
        /// <param name="target">Destination Item</param>
        private void CopyItem(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            // Shows dialog box to confirm dropping an item
            if (MessageBox.Show("Would you like to drop " + _sourceItem.Header.ToString() +
                " into " + _target.Header.ToString() + "", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    // adding dragged TreeViewItem in target TreeViewItem
                    addChild(_sourceItem, _target);

                    // finding Parent TreeViewItem of dragged TreeViewItem
                    TreeViewItem ParentItem = FindVisualParent<TreeViewItem>(_sourceItem);

                    // If Parent is null then remove from TreeView else remove from Parent TreeView
                    if (Parent == null)
                    {
                        FolderView.Items.Remove(_sourceItem);
                    }
                    else
                    {
                        ParentItem.Items.Remove(_sourceItem);
                    }
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Add all the child items from source to target location 
        /// </summary>
        /// <param name="_sourceItem"> Source Items </param>
        /// <param name="_targetItem"> Target Items </param>
        private void addChild(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            // add item in target TreeViewItem
            TreeViewItem item1 = new TreeViewItem();
            item1.Header = _sourceItem.Header;
            _targetItem.Items.Add(item1);
            foreach (TreeViewItem item in _sourceItem.Items)
            {
                addChild(item, item1);
            }
        }

        /// <summary>
        /// Find the Parent of the Child Item in UI
        /// </summary>
        /// <typeparam name="TObject"> UIElement as Object</typeparam>
        /// <param name="child">Child item</param>
        /// <returns>Parent item</returns>
        private TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            // get the parent item of the child element as UIElement
            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                if (parent is TObject found)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }

        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item
            TreeViewItem container = element as TreeViewItem;
            while (container == null && element != null)
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }

            return container;
        }

        #endregion

        #region Refreshing Views
        /// <summary>
        /// Refresh and Update the View
        /// </summary>
        private void RefreshViews()
        {
            foreach (TreeViewItem item in FolderView.Items)
                RefreshSubtree(item);
        }

        /// <summary>
        /// Get the current status of the each of the sub-item to know which item is expanded and which item is not
        /// </summary>
        /// <param name="item">The item or subitem</param>
        /// <param name="expansionState">If the item is expanded, then return true else false</param>
        private void CaptureExpansionState(TreeViewItem item, Dictionary<string, bool> expansionState)
        {
            // return if there is no item which means it is not expanded or not expandable
            if (item == null || item.Items.Count == 0)
                return;

            // get the path of the item: file 
            var path = item.Tag as string;

            // added the path to the expansion list
            expansionState.Add(path, item.IsExpanded);

            // for each item in treeview list
            foreach (TreeViewItem subItem in item.Items)
                // repeat the same steps for each of the sub-items
                CaptureExpansionState(subItem, expansionState);

        }

        /// <summary>
        /// Restore the expanded items
        /// </summary>
        /// <param name="item">the treeview item or subitems</param>
        /// <param name="expansionState">If true then item or sub-item is expanded otherwise not expanded</param>
        private void RestoreExpansionState(TreeViewItem item, Dictionary<string, bool> expansionState)
        {
            // get the path of the item: file
            var path = item.Tag as string;

            // retrieve the items which are expanded
            var ret = expansionState.TryGetValue(path, out bool isExpanded);

            // if item to retrieve exists
            if (ret)
            {
                // set the isExpanded flag to the current state
                item.IsExpanded = isExpanded;
            }
        }

        /// <summary>
        /// Refresh the items and the subitems for each view
        /// </summary>
        /// <param name="item">The Treeview item or sub-item</param>
        private void RefreshSubtree(TreeViewItem item)
        {
            // if the item is not expanded or the item is not expandable such as file, then return
            if (!item.IsExpanded || item.Items.Count == 0)
                return;

            // Store the expansion state of the treeview items or sub-items
            var expansionState = new Dictionary<string, bool>();

            // Get the expansion state of the Treeview item or sub-items
            CaptureExpansionState(item, expansionState);

            // set isexpanded flag to false as the initial state
            item.IsExpanded = false;

            // clear all the tree view items or sub-items
            item.Items.Clear();

            // Add a null item to the view to show that it is expandable
            item.Items.Add(null);

            // Set the isExpanded flag to true
            item.IsExpanded = true;

            // initialize the stack to store tree view items temporarily
            var stack = new Stack<TreeViewItem>();

            // foreach subitems in the list of treeview items
            foreach (TreeViewItem subItem in item.Items)
                // store the item into stack
                stack.Push(subItem);

            // as long as there are items in stack, keep running
            while (stack.Count > 0)
            {
                // get each of the subitems stored in the stack
                var subItem = stack.Pop();

                // Restore it's expansion state
                RestoreExpansionState(subItem, expansionState);

                // if the subitem is expanded
                if (subItem.IsExpanded)
                {
                    // for each subitems within subitems in the treeview items  
                    foreach (TreeViewItem subSubItem in subItem.Items)
                        // add the items to the stack
                        stack.Push(subSubItem);
                }
            }
        }

        #endregion

    }
}
