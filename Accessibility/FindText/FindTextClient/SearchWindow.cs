// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Application = System.Windows.Forms.Application;

namespace FindTextClient
{
    /// --------------------------------------------------------------------
    /// <summary>
    ///     Interaction logic for client and target
    /// </summary>
    /// --------------------------------------------------------------------
    public class SearchWindow
    {
        #region Expand Selection

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the expand to enclosing unit item selected event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void ExpandToTextUnit_Change(
            object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox) sender;

            // Expand the target selection to the selected text unit.
            var selectionRanges =
                _targetTextPattern.GetSelection();
            foreach (var textRange in selectionRanges)
            {
                switch (cb.SelectedValue.ToString())
                {
                    case "None":
                        textRange.MoveEndpointByRange(
                            TextPatternRangeEndpoint.End,
                            textRange,
                            TextPatternRangeEndpoint.Start);
                        break;
                    case "Character":
                        textRange.ExpandToEnclosingUnit(TextUnit.Character);
                        break;
                    case "Format":
                        textRange.ExpandToEnclosingUnit(TextUnit.Format);
                        break;
                    case "Word":
                        textRange.ExpandToEnclosingUnit(TextUnit.Word);
                        break;
                    case "Line":
                        textRange.ExpandToEnclosingUnit(TextUnit.Line);
                        break;
                    case "Paragraph":
                        textRange.ExpandToEnclosingUnit(TextUnit.Paragraph);
                        break;
                    case "Page":
                        textRange.ExpandToEnclosingUnit(TextUnit.Page);
                        break;
                    case "Document":
                        textRange.ExpandToEnclosingUnit(TextUnit.Document);
                        break;
                }
                textRange.Select();
                // The WPF target doesn't show selected text as highlighted unless
                // the window has focus.
                _targetWindow.SetFocus();
            }
        }

        #endregion Expand Highlight

        #region Member Variables

        // Client application window.
        private readonly Window _clientWindow;
        // Container for all client controls.
        private readonly DockPanel _clientDockPanel;
        // Target application window.
        private AutomationElement _targetWindow;
        // Target text control.
        private AutomationElement _targetDocument;
        // Text pattern obtained from the target text control.
        private TextPattern _targetTextPattern;
        // Text range for entire target text control. 
        private TextPatternRange _documentRange;
        // Text range for current selection in target text control.
        private TextPatternRange _searchRange;
        // Target applications based on underlying framework.
        private readonly Button _startWpfTargetButton;
        private readonly Button _startW32TargetButton;
        private readonly string _wpfTarget;
        private readonly string _w32Target;
        // Find the target text control.
        private readonly Button _findEditButton;
        // Display target status.
        private readonly Label _targetResult;
        // Layout grid for client controls.
        private readonly Grid _infoGrid;
        // String to search for in the target text control.
        private readonly TextBox _searchString;
        // Depending on the location of the selection in the target text
        // control, the client can search forward or backward for the 
        // search string.
        private readonly Button _searchBackwardButton;
        private readonly Button _searchForwardButton;
        // Direction of search.
        private bool _searchBackward;
        // Expand the target text selection by the specified text unit.
        private readonly ComboBox _expandHighlight;
        // Move the target text selection by the specified text unit. 
        private readonly ComboBox _navigateTarget;
        private TextUnit _navigationUnit;
        // Display the target text selection and attributes.
        private readonly Label _targetSelectionLabel;
        private readonly Label _targetSelectionAttributeLabel;
        private readonly TextBox _targetSelection;
        private readonly TextBox _targetSelectionAttributes;
        // Display target text selection details such as child and enclosing unit.
        private readonly TextBox _targetSelectionDetails;
        // Delegates for updating the client UI based on target application events.
        private delegate void TextChangeDelegate(string message);

        private delegate void SelectionChangeDelegate();

        private delegate void ProviderCloseDelegate();

        // Target applications.
        private enum TargetApplication
        {
            FlowDocument,
            WordPad
        }

        // Search and navigation directions.
        private enum TraversalDirection
        {
            Backward,
            Forward
        }

        #endregion Member Variables

        #region Client Setup

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The class constructor.
        /// </summary>
        /// <remarks>
        ///     Initialize the WPF client application.
        /// </remarks>
        /// --------------------------------------------------------------------
        public SearchWindow()
        {
            // Specify the target applications.
            _wpfTarget =
                Application.StartupPath + "\\FindText.exe";
            _w32Target = "WordPad.exe";

            // Initialize search direction. 
            // Search direction buttons are enabled or disabled based on this value.
            _searchBackward = false;

            // Layout the client controls.
            try
            {
                // Specs for Window.
                double clientHeight = 600;
                double clientWidth = 550;

                // Specs for TextBox.
                var maxSearchLines = 1;
                var maxSearchLength = 25;

                // Specs for Buttons.
                double buttonWidth = 140;

                // Instantiate the client window and set location and size.
                _clientWindow = new Window
                {
                    Height = clientHeight,
                    Width = clientWidth,
                    Left = SystemParameters.WorkArea.Location.X + 50,
                    Top = SystemParameters.WorkArea.Location.Y + 50,
                    Title = "Find Text",
                    WindowStyle = WindowStyle.ToolWindow
                };

                // Create a dock panel to hold all controls.
                _clientDockPanel = new DockPanel
                {
                    Margin = new Thickness(10),
                    LastChildFill = true
                };

                // Add the start target buttons.
                _startWpfTargetButton = new Button
                {
                    Width = buttonWidth,
                    Content = "Start _FlowDocument (WPF)",
                    Tag = TargetApplication.FlowDocument
                };
                _startWpfTargetButton.Click +=
                    StartTargetApplication_Click;
                _startW32TargetButton = new Button
                {
                    Width = buttonWidth,
                    Content = "Start _WordPad (Win32)",
                    Tag = TargetApplication.WordPad
                };
                _startW32TargetButton.Click +=
                    StartTargetApplication_Click;
                DockPanel.SetDock(_startWpfTargetButton, Dock.Top);
                DockPanel.SetDock(_startW32TargetButton, Dock.Top);
                _clientDockPanel.Children.Add(_startWpfTargetButton);
                _clientDockPanel.Children.Add(_startW32TargetButton);

                // Add the find text control button.
                _findEditButton = new Button
                {
                    Width = buttonWidth,
                    Content = "_Find text provider"
                };
                _findEditButton.Click +=
                    FindTextProvider_Click;
                _findEditButton.Visibility = Visibility.Collapsed;
                DockPanel.SetDock(_findEditButton, Dock.Top);
                _clientDockPanel.Children.Add(_findEditButton);

                // Add the target status label.
                _targetResult = new Label
                {
                    Content = "",
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10),
                    Visibility = Visibility.Hidden
                };
                DockPanel.SetDock(_targetResult, Dock.Top);
                _clientDockPanel.Children.Add(_targetResult);

                // Add the client control layout grid.
                _infoGrid = new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    ShowGridLines = false,
                    Visibility = Visibility.Collapsed,
                    MinWidth = 400
                };
                // Define the columns.
                var colDef1 = new ColumnDefinition();
                var colDef2 = new ColumnDefinition();
                _infoGrid.ColumnDefinitions.Add(colDef1);
                _infoGrid.ColumnDefinitions.Add(colDef2);
                // Define the rows.
                var rowDef1 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef2 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef3 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef4 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef5 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef6 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef7 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef8 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef9 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef10 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                var rowDef11 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)};
                _infoGrid.RowDefinitions.Add(rowDef1);
                _infoGrid.RowDefinitions.Add(rowDef2);
                _infoGrid.RowDefinitions.Add(rowDef3);
                _infoGrid.RowDefinitions.Add(rowDef4);
                _infoGrid.RowDefinitions.Add(rowDef5);
                _infoGrid.RowDefinitions.Add(rowDef6);
                _infoGrid.RowDefinitions.Add(rowDef7);
                _infoGrid.RowDefinitions.Add(rowDef8);
                _infoGrid.RowDefinitions.Add(rowDef9);
                _infoGrid.RowDefinitions.Add(rowDef10);
                _infoGrid.RowDefinitions.Add(rowDef11);

                // Define the content of the cells.
                // Row 1 - Search string details.
                _searchString = new TextBox
                {
                    Name = "SearchString",
                    MaxLines = maxSearchLines,
                    MaxLength = maxSearchLength,
                    Width = 200
                };
                //searchString.Height = 25;
                _searchString.TextChanged +=
                    SearchString_Change;
                _searchString.IsEnabled = false;
                Grid.SetRow(_searchString, 0);
                Grid.SetColumn(_searchString, 1);
                var searchLabel = new Label
                {
                    Target = _searchString,
                    Content = "_Search for: ",
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(searchLabel, 0);
                Grid.SetColumn(searchLabel, 0);
                _infoGrid.Children.Add(searchLabel);
                _infoGrid.Children.Add(_searchString);

                // Row 2 - Search direction buttons.
                _searchBackwardButton = new Button
                {
                    Width = buttonWidth,
                    Content = "Search _Backward",
                    Tag = TraversalDirection.Backward
                };
                //searchBackwardButton.Height = buttonHeight;
                _searchBackwardButton.Click +=
                    SearchDirection_Click;
                _searchBackwardButton.Margin = new Thickness(0, 0, 0, 10);
                _searchBackwardButton.IsEnabled = false;
                Grid.SetRow(_searchBackwardButton, 1);
                Grid.SetColumn(_searchBackwardButton, 0);
                _searchForwardButton = new Button
                {
                    Width = buttonWidth,
                    Content = "Search _Forward",
                    Tag = TraversalDirection.Forward
                };
                //searchForwardButton.Height = buttonHeight;
                _searchForwardButton.Click +=
                    SearchDirection_Click;
                _searchForwardButton.Margin = new Thickness(0, 0, 0, 10);
                _searchForwardButton.IsEnabled = false;
                Grid.SetRow(_searchForwardButton, 1);
                Grid.SetColumn(_searchForwardButton, 1);
                _infoGrid.Children.Add(_searchBackwardButton);
                _infoGrid.Children.Add(_searchForwardButton);

                // Row 3 - Target selection.
                _targetSelectionLabel = new Label
                {
                    Target = _targetSelection,
                    Content = "Currently selected:"
                };
                Grid.SetRow(_targetSelectionLabel, 2);
                Grid.SetColumn(_targetSelectionLabel, 0);
                Grid.SetColumnSpan(_targetSelectionLabel, 2);
                _infoGrid.Children.Add(_targetSelectionLabel);
                // Row 4.
                _targetSelection = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 400,
                    Height = 100,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    IsReadOnly = true,
                    Margin = new Thickness(0, 0, 0, 0)
                };
                Grid.SetRow(_targetSelection, 3);
                Grid.SetColumn(_targetSelection, 0);
                Grid.SetColumnSpan(_targetSelection, 2);
                _infoGrid.Children.Add(_targetSelection);

                // Row 5 - Target selection attributes.
                _targetSelectionAttributeLabel = new Label
                {
                    Target = _targetSelection,
                    Content = "Attribute values: "
                };
                Grid.SetRow(_targetSelectionAttributeLabel, 4);
                Grid.SetColumn(_targetSelectionAttributeLabel, 0);
                Grid.SetColumnSpan(_targetSelectionAttributeLabel, 2);
                _infoGrid.Children.Add(_targetSelectionAttributeLabel);
                // Row 6.
                _targetSelectionAttributes = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 400,
                    Height = 100,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    IsReadOnly = true,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Grid.SetRow(_targetSelectionAttributes, 5);
                Grid.SetColumn(_targetSelectionAttributes, 0);
                Grid.SetColumnSpan(_targetSelectionAttributes, 2);
                _infoGrid.Children.Add(_targetSelectionAttributes);

                // Row 7 - Navigate target details.
                _navigateTarget = new ComboBox {Width = buttonWidth};
                _navigateTarget.Items.Add(TextUnit.Character);
                _navigateTarget.Items.Add(TextUnit.Format);
                _navigateTarget.Items.Add(TextUnit.Word);
                _navigateTarget.Items.Add(TextUnit.Line);
                _navigateTarget.Items.Add(TextUnit.Paragraph);
                _navigateTarget.Items.Add(TextUnit.Page);
                _navigateTarget.SelectedIndex = 0;
                _navigateTarget.SelectionChanged +=
                    NavigationUnit_Change;
                Grid.SetRow(_navigateTarget, 6);
                Grid.SetColumn(_navigateTarget, 1);
                var navigateLabel = new Label
                {
                    Target = _navigateTarget,
                    Content = "_Navigate target by text unit of: ",
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(navigateLabel, 6);
                Grid.SetColumn(navigateLabel, 0);
                _infoGrid.Children.Add(navigateLabel);
                _infoGrid.Children.Add(_navigateTarget);

                // Row 8 - Navigate target direction buttons.
                var navigateBackwardButton = new Button
                {
                    Width = buttonWidth,
                    Content = "_Backward",
                    Tag = TraversalDirection.Backward
                };
                navigateBackwardButton.Click += Navigate_Click;
                navigateBackwardButton.Margin = new Thickness(0, 0, 0, 10);
                Grid.SetRow(navigateBackwardButton, 7);
                Grid.SetColumn(navigateBackwardButton, 0);
                var navigateForwardButton = new Button
                {
                    Width = buttonWidth,
                    Content = "_Forward",
                    Tag = TraversalDirection.Forward
                };
                navigateForwardButton.Click += Navigate_Click;
                navigateForwardButton.Margin = new Thickness(0, 0, 0, 10);
                Grid.SetRow(navigateForwardButton, 7);
                Grid.SetColumn(navigateForwardButton, 1);
                _infoGrid.Children.Add(navigateBackwardButton);
                _infoGrid.Children.Add(navigateForwardButton);

                // Row 9 - Expand the target selection controls.
                _expandHighlight = new ComboBox {Width = buttonWidth};
                _expandHighlight.Items.Add("");
                _expandHighlight.Items.Add("None");
                _expandHighlight.Items.Add(TextUnit.Character);
                _expandHighlight.Items.Add(TextUnit.Format);
                _expandHighlight.Items.Add(TextUnit.Word);
                _expandHighlight.Items.Add(TextUnit.Line);
                _expandHighlight.Items.Add(TextUnit.Paragraph);
                _expandHighlight.Items.Add(TextUnit.Page);
                _expandHighlight.Items.Add(TextUnit.Document);
                _expandHighlight.SelectedIndex = 0;
                _expandHighlight.SelectionChanged +=
                    ExpandToTextUnit_Change;
                _expandHighlight.Margin = new Thickness(0, 0, 0, 10);
                Grid.SetRow(_expandHighlight, 8);
                Grid.SetColumn(_expandHighlight, 1);
                var expandLabel = new Label
                {
                    Target = _expandHighlight,
                    Content = "_Expand selection to text unit of: ",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Grid.SetRow(expandLabel, 8);
                Grid.SetColumn(expandLabel, 0);
                _infoGrid.Children.Add(expandLabel);
                _infoGrid.Children.Add(_expandHighlight);

                // Row 10 - target selection details such as child elements 
                //         and enclosing unit.
                _targetSelectionDetails = new TextBox
                {
                    Height = 100,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    IsReadOnly = true
                };
                Grid.SetRow(_targetSelectionDetails, 9);
                Grid.SetColumn(_targetSelectionDetails, 0);
                Grid.SetColumnSpan(_targetSelectionDetails, 2);
                _infoGrid.Children.Add(_targetSelectionDetails);

                // Row 11 - get the child elements and the enclosing unit 
                //         of the target selection.
                var getChildren = new Button
                {
                    Width = buttonWidth,
                    Content = "Get children of selection"
                };
                getChildren.Click += GetChildren_Click;
                Grid.SetRow(getChildren, 10);
                Grid.SetColumn(getChildren, 0);
                var getEnclosingElement = new Button
                {
                    Width = buttonWidth,
                    Content = "Get enclosing element"
                };
                getEnclosingElement.Click += GetEnclosingElement_Click;
                Grid.SetRow(getEnclosingElement, 10);
                Grid.SetColumn(getEnclosingElement, 1);
                _infoGrid.Children.Add(getChildren);
                _infoGrid.Children.Add(getEnclosingElement);

                // Add the grid to the dock panel.
                DockPanel.SetDock(_infoGrid, Dock.Top);
                _clientDockPanel.Children.Add(_infoGrid);

                // Add the dock panel to the window.
                _clientWindow.Content = _clientDockPanel;
                _clientWindow.Show();
            }
                // Do we successfully create the client app?
            catch (InvalidOperationException)
            {
                // TODO: error handling.
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the Start Application button click.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Starts the application that we are going to use for as our
        ///     root element for this sample.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void StartTargetApplication_Click(object sender, RoutedEventArgs e)
        {
            var targetOption = (Button) sender;

            // Start the target selected by the user.
            _targetWindow =
                StartApp((TargetApplication) targetOption.Tag == TargetApplication.WordPad ? _w32Target : _wpfTarget);

            // Target is not available.
            Debug.Assert(_targetWindow != null);

            // Set a listener for the window closed event on the target.
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowClosedEvent,
                _targetWindow, TreeScope.Element, OnTargetClose);

            // Set size and position of target.
            // Since the target is started and manipulated from the client 
            // and both windows show UI changes this section of code just
            // ensures neither window obscures the other.
            var targetTransformPattern =
                _targetWindow.GetCurrentPattern(TransformPattern.Pattern)
                    as TransformPattern;
            targetTransformPattern?.Resize(_clientWindow.Width, _clientWindow.Height);
            targetTransformPattern.Move(
                _clientWindow.Left + _clientWindow.Width + 25, _clientWindow.Top);

            // Set visibility of client controls.
            _startWpfTargetButton.Visibility = Visibility.Collapsed;
            _startW32TargetButton.Visibility = Visibility.Collapsed;
            _findEditButton.Visibility = Visibility.Visible;
            _targetResult.Visibility = Visibility.Visible;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Starts the target application.
        /// </summary>
        /// <param name="app">
        ///     The application to start.
        /// </param>
        /// <returns>The automation element for the app main window.</returns>
        /// <remarks>
        ///     Three WPF documents, a rich text document, and a plain text document
        ///     are provided in the Content folder of the TextProvider project.
        /// </remarks>
        /// --------------------------------------------------------------------
        private AutomationElement StartApp(string app)
        {
            // Start application.
            var p = Process.Start(app);

            // Give the target application some time to start.
            // For Win32 applications, WaitForInputIdle can be used instead.
            // Another alternative is to listen for WindowOpened events.
            // Otherwise, an ArgumentException results when you try to
            // retrieve an automation element from the window handle.
            Thread.Sleep(2000);

            _targetResult.Content =
                _wpfTarget +
                " started. \n\nPlease load a document into the target " +
                "application and click the 'Find edit control' button above. " +
                "\n\nNOTE: Documents can be found in the 'Content' folder of the FindText project.";
            _targetResult.Background = Brushes.LightGreen;

            // Return the automation element for the app main window.
            return (AutomationElement.FromHandle(p.MainWindowHandle));
        }

        #endregion Client Setup

        #region Target Text Info

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Finds the text control in our target.
        /// </summary>
        /// <param name="src">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Initializes the TextPattern object and event handlers.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void FindTextProvider_Click(object src, RoutedEventArgs e)
        {
            // Set up the conditions for finding the text control.
            var documentControl = new PropertyCondition(
                AutomationElement.ControlTypeProperty,
                ControlType.Document);
            var textPatternAvailable = new PropertyCondition(
                AutomationElement.IsTextPatternAvailableProperty, true);
            var findControl =
                new AndCondition(documentControl, textPatternAvailable);

            // Get the Automation Element for the first text control found.
            // For the purposes of this sample it is sufficient to find the 
            // first text control. In other cases there may be multiple text
            // controls to sort through.
            _targetDocument =
                _targetWindow.FindFirst(TreeScope.Descendants, findControl);

            // Didn't find a text control.
            if (_targetDocument == null)
            {
                _targetResult.Content =
                    _wpfTarget +
                    " does not contain a Document control type.";
                _targetResult.Background = Brushes.Salmon;
                _startWpfTargetButton.IsEnabled = false;
                return;
            }

            // Get required control patterns 
            _targetTextPattern =
                _targetDocument.GetCurrentPattern(
                    TextPattern.Pattern) as TextPattern;

            // Didn't find a text control that supports TextPattern.
            if (_targetTextPattern == null)
            {
                _targetResult.Content =
                    _wpfTarget +
                    " does not contain an element that supports TextPattern.";
                _targetResult.Background = Brushes.Salmon;
                _startWpfTargetButton.IsEnabled = false;
                return;
            }

            // Text control is available so display the client controls.
            _infoGrid.Visibility = Visibility.Visible;

            _targetResult.Content =
                "Text provider found.";
            _targetResult.Background = Brushes.LightGreen;

            // Initialize the document range for the text of the document.
            _documentRange = _targetTextPattern.DocumentRange;

            // Initialize the client's search buttons.
            if (_targetTextPattern.DocumentRange.GetText(1).Length > 0)
            {
                _searchForwardButton.IsEnabled = true;
            }
            // Initialize the client's search TextBox.
            _searchString.IsEnabled = true;

            // Check if the text control supports text selection
            if (_targetTextPattern.SupportedTextSelection ==
                SupportedTextSelection.None)
            {
                _targetResult.Content = "Unable to select text.";
                _targetResult.Background = Brushes.Salmon;
                return;
            }

            // Edit control found so remove the find button from the client.
            _findEditButton.Visibility = Visibility.Collapsed;

            // Initialize the client with the current target selection, if any.
            NotifySelectionChanged();

            // Search starts at beginning of doc and goes forward
            _searchBackward = false;

            // Initialize a text changed listener.
            // An instance of TextPatternRange will become invalid if 
            // one of the following occurs:
            // 1) The text in the provider changes via some user activity.
            // 2) ValuePattern.SetValue is used to programatically change 
            // the value of the text in the provider.
            // The only way the client application can detect if the text 
            // has changed (to ensure that the ranges are still valid), 
            // is by setting a listener for the TextChanged event of 
            // the TextPattern. If this event is raised, the client needs 
            // to update the targetDocumentRange member data to ensure the 
            // user is working with the updated text. 
            // Clients must always anticipate the possibility that the text 
            // can change underneath them.
            Automation.AddAutomationEventHandler(
                TextPattern.TextChangedEvent,
                _targetDocument,
                TreeScope.Element,
                TextChanged);

            // Initialize a selection changed listener.
            // The target selection is reflected in the client.
            Automation.AddAutomationEventHandler(
                TextPattern.TextSelectionChangedEvent,
                _targetDocument,
                TreeScope.Element,
                OnTextSelectionChange);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Gets the enclosing element of the target selection.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void GetEnclosingElement_Click(object sender, RoutedEventArgs e)
        {
            // Obtain the enclosing element.
            AutomationElement enclosingElement;
            try
            {
                enclosingElement = _searchRange.GetEnclosingElement();
            }
            catch (ElementNotAvailableException)
            {
                // TODO: error handling.
                return;
            }

            // Assemble the information about the enclosing element.
            var enclosingElementInformation = new StringBuilder();
            enclosingElementInformation.Append(
                "Enclosing element:\t").AppendLine(
                    enclosingElement.Current.ControlType.ProgrammaticName);

            // The WPF target doesn't show selected text as highlighted unless
            // the window has focus.
            _targetWindow.SetFocus();

            // Display the enclosing element information in the client.
            _targetSelectionDetails.Text = enclosingElementInformation.ToString();

            // Is the enclosing element the entire document?
            // If so, select the document.
            if (enclosingElement == _targetDocument)
            {
                _documentRange = _targetTextPattern.DocumentRange;
                _documentRange.Select();
                return;
            }
            // Otherwise, select the range from the child element.
            var childRange =
                _documentRange.TextPattern.RangeFromChild(enclosingElement);
            childRange.Select();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Gets the children of the target selection.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void GetChildren_Click(object sender, RoutedEventArgs e)
        {
            // Obtain an array of child elements.
            AutomationElement[] textProviderChildren;
            try
            {
                textProviderChildren = _searchRange.GetChildren();
            }
            catch (ElementNotAvailableException)
            {
                // TODO: error handling.
                return;
            }

            // Assemble the information about the enclosing element.
            var childInformation = new StringBuilder();
            childInformation.Append(textProviderChildren.Length)
                .AppendLine(" child element(s).");

            // Iterate through the collection of child elements and obtain
            // information of interest about each.
            for (var i = 0; i < textProviderChildren.Length; i++)
            {
                childInformation.Append("Child").Append(i).AppendLine(":");
                // Obtain the name of the child control.
                childInformation.Append("\tName:\t")
                    .AppendLine(textProviderChildren[i].Current.Name);
                // Obtain the control type.
                childInformation.Append("\tControl Type:\t")
                    .AppendLine(textProviderChildren[i].Current.ControlType.ProgrammaticName);

                // Obtain the supported control patterns.
                // NOTE: For the purposes of this sample we use GetSupportedPatterns(). 
                // However, the use of GetSupportedPatterns() is strongly discouraged 
                // as it calls GetCurrentPattern() internally on every known pattern. 
                // It is therefore much more efficient to use GetCurrentPattern() for 
                // the specific patterns you are interested in.
                var childPatterns =
                    textProviderChildren[i].GetSupportedPatterns();
                childInformation.AppendLine("\tSupported Control Patterns:");
                if (childPatterns.Length <= 0)
                {
                    childInformation.AppendLine("\t\t\tNone");
                }
                else
                {
                    foreach (var pattern in childPatterns)
                    {
                        childInformation.Append("\t\t\t")
                            .AppendLine(pattern.ProgrammaticName);
                    }
                }

                // Obtain the child elements, if any, of the child control.
                var childRange =
                    _documentRange.TextPattern.RangeFromChild(textProviderChildren[i]);
                var childRangeChildren =
                    childRange.GetChildren();
                childInformation.Append("\tChildren: \t").Append(childRangeChildren.Length).AppendLine();
            }
            // Display the information about the child controls.
            _targetSelectionDetails.Text = childInformation.ToString();
        }

        #endregion Target Text Info

        #region Search Target

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles changes to the search text in the client.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Reset all controls if user changes search text
        /// </remarks>
        /// --------------------------------------------------------------------
        private void SearchString_Change(object sender, TextChangedEventArgs e)
        {
            var startPoints = _documentRange.CompareEndpoints(
                TextPatternRangeEndpoint.Start,
                _searchRange,
                TextPatternRangeEndpoint.Start);
            var endPoints = _documentRange.CompareEndpoints(
                TextPatternRangeEndpoint.End,
                _searchRange,
                TextPatternRangeEndpoint.End);

            // If the starting endpoints of the document range and the search
            // range are equivalent then we can search forward only since the 
            // search range is at the start of the document.
            if (startPoints == 0)
            {
                _searchForwardButton.IsEnabled = true;
                _searchBackwardButton.IsEnabled = false;
            }
            // If the ending endpoints of the document range and the search
            // range are identical then we can search backward only since the 
            // search range is at the end of the document.
            else if (endPoints == 0)
            {
                _searchForwardButton.IsEnabled = false;
                _searchBackwardButton.IsEnabled = true;
            }
            // Otherwise we can search both directions.
            else
            {
                _searchForwardButton.IsEnabled = true;
                _searchBackwardButton.IsEnabled = true;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the Search button click.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>Find the text specified in the text box.</remarks>
        /// --------------------------------------------------------------------
        private void SearchDirection_Click(object sender, RoutedEventArgs e)
        {
            var searchDirection = (Button) sender;

            // Are we searching backward through the text control?
            _searchBackward =
                ((TraversalDirection) searchDirection.Tag == TraversalDirection.Backward);

            // Check if search text entered
            if (_searchString.Text.Trim() == "")
            {
                _targetResult.Content = "No search criteria.";
                _targetResult.Background = Brushes.Salmon;
                return;
            }

            // Does target range support text selection?
            if (_targetTextPattern.SupportedTextSelection ==
                SupportedTextSelection.None)
            {
                _targetResult.Content = "Unable to select text.";
                _targetResult.Background = Brushes.Salmon;
                return;
            }
            // Does target range support multiple selections?
            if (_targetTextPattern.SupportedTextSelection ==
                SupportedTextSelection.Multiple)
            {
                _targetResult.Content = "Multiple selections present.";
                _targetResult.Background = Brushes.Salmon;
                return;
            }

            // Clone the document range since we modify the endpoints 
            // as we search.
            var documentRangeClone = _documentRange.Clone();

            // Move the cloned document range endpoints to enable the 
            // selection of the next matching text range.
            var selectionRange =
                _targetTextPattern.GetSelection();
            if (selectionRange[0] != null)
            {
                if (_searchBackward)
                {
                    documentRangeClone.MoveEndpointByRange(
                        TextPatternRangeEndpoint.End,
                        selectionRange[0],
                        TextPatternRangeEndpoint.Start);
                }
                else
                {
                    documentRangeClone.MoveEndpointByRange(
                        TextPatternRangeEndpoint.Start,
                        selectionRange[0],
                        TextPatternRangeEndpoint.End);
                }
            }

            // Find the text specified in the Search textbox.
            // Clone the search range since we need to modify it.
            var searchRangeClone = _searchRange.Clone();
            // backward = false? -- search forward, otherwise backward.
            // ignoreCase = false? -- search is case sensitive.
            _searchRange =
                documentRangeClone.FindText(
                    _searchString.Text, _searchBackward, false);

            // Search unsuccessful.
            if (_searchRange == null)
            {
                // Search string not found at all.
                if (documentRangeClone.CompareEndpoints(
                    TextPatternRangeEndpoint.Start,
                    searchRangeClone,
                    TextPatternRangeEndpoint.Start) == 0)
                {
                    _targetResult.Content = "Text not found.";
                    _targetResult.Background = Brushes.Wheat;
                    _searchBackwardButton.IsEnabled = false;
                    _searchForwardButton.IsEnabled = false;
                }
                // End of document (either the start or end of the document 
                // range depending on search direction) was reached before 
                // finding another occurence of the search string.
                else
                {
                    _targetResult.Content = "End of document reached.";
                    _targetResult.Background = Brushes.Wheat;
                    if (!_searchBackward)
                    {
                        searchRangeClone.MoveEndpointByRange(
                            TextPatternRangeEndpoint.Start,
                            _documentRange,
                            TextPatternRangeEndpoint.End);
                        _searchBackwardButton.IsEnabled = true;
                        _searchForwardButton.IsEnabled = false;
                    }
                    else
                    {
                        searchRangeClone.MoveEndpointByRange(
                            TextPatternRangeEndpoint.End,
                            _documentRange,
                            TextPatternRangeEndpoint.Start);
                        _searchBackwardButton.IsEnabled = false;
                        _searchForwardButton.IsEnabled = true;
                    }
                }
                _searchRange = searchRangeClone;
            }
            // The search string was found.
            else
            {
                _targetResult.Content = "Text found.";
                _targetResult.Background = Brushes.LightGreen;
            }

            _searchRange.Select();
            // Scroll the selection into view and align with top of viewport
            _searchRange.ScrollIntoView(true);
            // The WPF target doesn't show selected text as highlighted unless
            // the window has focus.
            _targetWindow.SetFocus();
        }

        #endregion Search Target

        #region Target Navigation

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the navigation item selected event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void NavigationUnit_Change(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox) sender;
            _navigationUnit = (TextUnit) cb.SelectedValue;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the Navigate button click event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            var moveSelection = (Button) sender;

            // Which direction is the user searching through the text control?
            var navDirection =
                ((TraversalDirection) moveSelection.Tag == TraversalDirection.Forward) ? 1 : -1;

            // Obtain the ranges to move.
            var selectionRanges =
                _targetTextPattern.GetSelection();

            // Iterate throught the ranges for a text control that supports
            // multiple selections and move the selections the specified text
            // unit and direction.
            foreach (var textRange in selectionRanges)
            {
                textRange.Move(_navigationUnit, navDirection);
                textRange.Select();
            }
            // The WPF target doesn't show selected text as highlighted unless
            // the window has focus.
            _targetDocument.SetFocus();
        }

        #endregion Target Navigation

        #region Target Listeners

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the text changed event in the target.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Since the events are happening on a different thread than the
        ///     client we need to use a Dispatcher delegate to handle them.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void TextChanged(object sender, AutomationEventArgs e)
        {
            _clientWindow.Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new TextChangeDelegate(NotifyTextChanged),
                "Text changed, range reset.\n");
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The delegate for the target text changed event.
        /// </summary>
        /// <param name="arg">null argument</param>
        /// <returns>A null object.</returns>
        /// --------------------------------------------------------------------
        private void NotifyTextChanged(string message)
        {
            // Notify the user of the text changed event. 
            _targetSelectionLabel.Content = message;
            // Re-initialize the document range for the text of the document
            // since we don't know the extent of the changes. For example, a 
            // change in the font color attribute, such as on a hyperlink 
            // mouseover, raises this event but doesn't change the content of 
            // the text control.
            _documentRange = _targetTextPattern.DocumentRange;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the text selection changed event in the target.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Since the events are happening on a different thread than the
        ///     client we need to use a Dispatcher delegate to handle them.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void OnTextSelectionChange(object sender, AutomationEventArgs e)
        {
            _clientWindow.Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new SelectionChangeDelegate(NotifySelectionChanged));
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The delegate for the target text selection changed event.
        /// </summary>
        /// <param name="arg">null argument</param>
        /// <returns>A null object.</returns>
        /// --------------------------------------------------------------------
        private void NotifySelectionChanged()
        {
            // Get the array of disjoint selections.
            var selectionRanges =
                _targetTextPattern.GetSelection();
            // Update the current search range.
            // For the purposes of this sample only the first selection
            // range will be echoed in the client.
            _searchRange = selectionRanges[0];
            // For performance and security reasons we'll limit 
            // the length of the string retrieved to 100 characters.
            // Alternatively, GetText(-1) will retrieve all selected text.
            var selectedText = _searchRange.GetText(100);
            _targetSelectionLabel.Content =
                "Currently selected (100 character maximum): " +
                selectedText.Length + " characters.";

            // Report target selection details.
            DisplaySelectedTextWithAttributes(selectedText);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Display the target selection with attribute details in client.
        /// </summary>
        /// <param name="selectedText">The current target selection.</param>
        /// --------------------------------------------------------------------
        private void DisplaySelectedTextWithAttributes(string selectedText)
        {
            _targetSelection.Text = selectedText;
            // We're only interested in the FontNameAttribute for the purposes 
            // of this sample.
            _targetSelectionAttributes.Text =
                ParseTextRangeByAttribute(
                    selectedText, TextPattern.FontNameAttribute);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Parse the target selection based on the text attribute of interest.
        /// </summary>
        /// <param name="selectedText">The current target selection.</param>
        /// <param name="automationTextAttribute">
        ///     The text attribute of interest.
        /// </param>
        /// <returns>
        ///     A string representing the requested attribute details.
        /// </returns>
        /// --------------------------------------------------------------------
        private string ParseTextRangeByAttribute(
            string selectedText,
            AutomationTextAttribute automationTextAttribute)
        {
            var attributeDetails = new StringBuilder();
            // Initialize the current attribute value.
            var attributeValue = "";
            // Make a copy of the text range.
            var searchRangeClone = _searchRange.Clone();
            // Collapse the range to the starting endpoint.
            searchRangeClone.Move(TextUnit.Character, -1);
            // Iterate through the range character by character.
            for (var x = 1; x <= selectedText.Length; x++)
            {
                searchRangeClone.Move(TextUnit.Character, 1);
                // Get the attribute value of the current character.
                var newAttributeValue =
                    searchRangeClone.GetAttributeValue(automationTextAttribute).ToString();
                // If the new attribute value is not equal to the old then report 
                // the new value along with its location within the range.
                if (newAttributeValue != attributeValue)
                {
                    attributeDetails.Append(automationTextAttribute.ProgrammaticName)
                        .Append(":\n<")
                        .Append(newAttributeValue)
                        .Append("> at text range position ")
                        .AppendLine(x.ToString());
                    attributeValue = newAttributeValue;
                }
            }
            return attributeDetails.ToString();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the window closed event in the target.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void OnTargetClose(object sender, AutomationEventArgs e)
        {
            _clientWindow.Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new ProviderCloseDelegate(CloseApp));
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The delegate for the window closed event in the target.
        /// </summary>
        /// <param name="arg">null argument</param>
        /// <returns>A null object.</returns>
        /// --------------------------------------------------------------------
        private void CloseApp()
        {
            // Close the client window when the target window is closed.
            _clientWindow.Close();
        }

        #endregion Target Listeners
    }
}