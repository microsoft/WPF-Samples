// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace InvokePatternApp
{
    /// ------------------------------------------------------------------------
    /// <summary>
    ///     Interaction logic for WCP client and Win32 target
    /// </summary>
    /// ------------------------------------------------------------------------
    public class InvokePatternApp : Application
    {
        #region Member variables

        private Window _clientWindow;
        private AutomationElement _rootElement;
        private StackPanel[] _clientTreeViews;
        private TextBlock _statusText;
        private StackPanel _treeviewPanel;
        private StringBuilder _elementInfoCompile;
        private ScrollViewer _clientScrollViewer;
        private string _targetApp;
        // Delegates for updating the client UI based on target application events.
        private delegate void OutputDelegate(string message);

        #endregion Member variables

        #region Target listeners

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Listens for a structure changed event in the target.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Since the events are happening on a different thread than the
        ///     client we need to use a Dispatcher delegate to handle them.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void ChildElementsAdded(object sender, StructureChangedEventArgs e)
        {
            _clientWindow.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(NotifyChildElementsAdded), null);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The delegate for the structure changed event in the target.
        /// </summary>
        /// <param name="arg">null argument</param>
        /// <returns>A null object.</returns>
        /// --------------------------------------------------------------------
        private object NotifyChildElementsAdded(object arg)
        {
            FindTreeViewsInTarget();
            return null;
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
                DispatcherPriority.Background,
                new DispatcherOperationCallback(CloseApp), null);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The delegate for the window closed event in the target.
        /// </summary>
        /// <param name="arg">null argument</param>
        /// <returns>A null object.</returns>
        /// --------------------------------------------------------------------
        private object CloseApp(object arg)
        {
            _clientWindow.Close();
            return (null);
        }

        #endregion Target listeners

        #region Client setup

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Initialize both client and target applications.
        /// </summary>
        /// <param name="args">Startup arguments</param>
        /// --------------------------------------------------------------------
        protected override void OnStartup(StartupEventArgs args)
        {
            // Create our informational window
            CreateWindow();

            // Get the root element from our target application.
            // In general, you should try to obtain only direct children of 
            // the RootElement. A search for descendants may iterate through 
            // hundreds or even thousands of elements, possibly resulting in 
            // a stack overflow. If you are attempting to obtain a specific 
            // element at a lower level, you should start your search from the 
            // application window or from a container at a lower level.
            _targetApp =
                System.Windows.Forms.Application.StartupPath + "\\Target.exe";
            _rootElement = StartApp(_targetApp);
            if (_rootElement == null)
            {
                return;
            }

            // Add a structure change listener for the root element.
            var structureChange =
                new StructureChangedEventHandler(ChildElementsAdded);
            Automation.AddStructureChangedEventHandler(
                _rootElement, TreeScope.Descendants, structureChange);

            // Iterate through the controls in the target application.
            FindTreeViewsInTarget();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Start the target application.
        /// </summary>
        /// <param name="app">The target appliation.</param>
        /// <remarks>
        ///     Starts the application that we are going to use for as our
        ///     root element for this sample.
        /// </remarks>
        /// --------------------------------------------------------------------
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private AutomationElement StartApp(string app)
        {
            if (File.Exists(app))
            {
                AutomationElement targetElement;

                // Start application.
                var p = Process.Start(app);

                //// Give application a second to startup.
                Thread.Sleep(2000);

                targetElement = AutomationElement.FromHandle(p.MainWindowHandle);
                if (targetElement == null)
                {
                    return null;
                }
                var targetClosedHandler =
                    new AutomationEventHandler(OnTargetClose);
                Automation.AddAutomationEventHandler(
                    WindowPattern.WindowClosedEvent,
                    targetElement, TreeScope.Element, targetClosedHandler);

                // Set size and position of target.
                var targetTransformPattern =
                    targetElement.GetCurrentPattern(TransformPattern.Pattern)
                        as TransformPattern;
                if (targetTransformPattern == null)
                    return null;
                targetTransformPattern.Resize(550, 400);
                targetTransformPattern.Move(
                    _clientWindow.Left + _clientWindow.Width + 25, _clientWindow.Top);

                Output("Target started.");

                // Return the AutomationElement
                return (targetElement);
            }
            Output(app + " not found.");
            return (null);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Creates a client window to which output may be written.
        /// </summary>
        /// --------------------------------------------------------------------
        private void CreateWindow()
        {
            // Define a window.
            _clientWindow = new Window
            {
                Width = 400,
                Height = 600,
                Left = 50,
                Top = 50
            };

            // Define a ScrollViewer.
            _clientScrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            // Add Layout control.
            var clientStackPanel = new StackPanel();
            var statusPanel = new StackPanel {HorizontalAlignment = HorizontalAlignment.Center};
            _treeviewPanel = new StackPanel();

            _statusText = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap
            };

            // Add child elements to the parent StackPanel
            statusPanel.Children.Add(_statusText);
            clientStackPanel.Children.Add(statusPanel);
            clientStackPanel.Children.Add(_treeviewPanel);

            // Add the StackPanel as the lone Child of the Border
            _clientScrollViewer.Content = clientStackPanel;

            // Add the Border as the Content of the Parent Window Object
            _clientWindow.Content = _clientScrollViewer;
            _clientWindow.Show();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Finds all TreeView controls in the target.
        /// </summary>
        /// --------------------------------------------------------------------
        private void FindTreeViewsInTarget()
        {
            // Initialize the client area used to report target controls.
            _treeviewPanel.Children.Clear();
            // Set up our search condition
            var rootTreeViewCondition =
                new PropertyCondition(
                    AutomationElement.ControlTypeProperty, ControlType.Tree);
            // Find all controls that support the condition.
            var targetTreeViewElements =
                _rootElement.FindAll(TreeScope.Children, rootTreeViewCondition);
            if (targetTreeViewElements.Count <= 0)
            {
                Output("TreeView control(s) not found.");
                return;
            }
            // Create an area in the client to report each TreeView in the target.
            _clientTreeViews = new StackPanel[targetTreeViewElements.Count];
            for (var elementIndex = 0;
                elementIndex < targetTreeViewElements.Count;
                elementIndex++)
            {
                _clientTreeViews[elementIndex] = new StackPanel();
                var clientTreeViewBorder = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                    Margin = new Thickness(5),
                    Background = Brushes.LightGray,
                    Child = _clientTreeViews[elementIndex]
                };
                _treeviewPanel.Children.Add(clientTreeViewBorder);
                // Iterate through the descendant controls of each TreeView.
                FindTreeViewDescendants(
                    targetTreeViewElements[elementIndex], elementIndex);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Walks the UI Automation tree of the target and reports the control
        ///     type of each element it finds in the control view to the client.
        /// </summary>
        /// <param name="targetTreeViewElement">
        ///     The root of the search on this iteration.
        /// </param>
        /// <param name="elementIndex">
        ///     The TreeView index for this iteration.
        /// </param>
        /// <remarks>
        ///     This is a recursive function that maps out the structure of the
        ///     subtree of the target beginning at the AutomationElement passed in
        ///     as the rootElement on the first call. This could be, for example,
        ///     an application window.
        ///     CAUTION: Do not pass in AutomationElement.RootElement. Attempting
        ///     to map out the entire subtree of the desktop could take a very
        ///     long time and even lead to a stack overflow.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void FindTreeViewDescendants(
            AutomationElement targetTreeViewElement, int treeviewIndex)
        {
            if (targetTreeViewElement == null)
                return;

            var elementNode =
                TreeWalker.ControlViewWalker.GetFirstChild(targetTreeViewElement);

            while (elementNode != null)
            {
                var elementInfo = new Label {Margin = new Thickness(0)};
                _clientTreeViews[treeviewIndex].Children.Add(elementInfo);

                // Compile information about the control.
                _elementInfoCompile = new StringBuilder();
                var controlName =
                    (elementNode.Current.Name == "")
                        ? "Unnamed control"
                        : elementNode.Current.Name;
                var autoIdName =
                    (elementNode.Current.AutomationId == "")
                        ? "No AutomationID"
                        : elementNode.Current.AutomationId;

                _elementInfoCompile.Append(controlName)
                    .Append(" (")
                    .Append(elementNode.Current.ControlType.LocalizedControlType)
                    .Append(" - ")
                    .Append(autoIdName)
                    .Append(")");

                // Test for the control patterns of interest for this sample.
                object objPattern;
                if (elementNode.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out objPattern))
                {
                    var expcolPattern = objPattern as ExpandCollapsePattern;
                    if (expcolPattern.Current.ExpandCollapseState != ExpandCollapseState.LeafNode)
                    {
                        var expcolButton = new Button
                        {
                            Margin = new Thickness(0, 0, 0, 5),
                            Height = 20,
                            Width = 100,
                            Content = "ExpandCollapse",
                            Tag = expcolPattern
                        };
                        expcolButton.Click +=
                            ExpandCollapse_Click;
                        _clientTreeViews[treeviewIndex].Children.Add(expcolButton);
                    }
                }
                if (elementNode.TryGetCurrentPattern(TogglePattern.Pattern, out objPattern))
                {
                    var togPattern = objPattern as TogglePattern;
                    var togButton = new Button
                    {
                        Margin = new Thickness(0, 0, 0, 5),
                        Height = 20,
                        Width = 100,
                        Content = "Toggle",
                        Tag = togPattern
                    };
                    togButton.Click += Toggle_Click;
                    _clientTreeViews[treeviewIndex].Children.Add(togButton);
                }
                if (elementNode.TryGetCurrentPattern(InvokePattern.Pattern, out objPattern))
                {
                    var invPattern = objPattern as InvokePattern;
                    var invButton = new Button
                    {
                        Margin = new Thickness(0),
                        Height = 20,
                        Width = 100,
                        Content = "Invoke",
                        Tag = invPattern
                    };
                    invButton.Click += Invoke_Click;
                    _clientTreeViews[treeviewIndex].Children.Add(invButton);
                }
                // Display compiled information about the control.
                elementInfo.Content = _elementInfoCompile;
                var sep = new Separator();
                _clientTreeViews[treeviewIndex].Children.Add(sep);

                // Iterate to next element.
                // elementNode - Current element.
                // treeviewIndex - Index of parent TreeView.
                FindTreeViewDescendants(elementNode, treeviewIndex);
                elementNode =
                    TreeWalker.ControlViewWalker.GetNextSibling(elementNode);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the Toggle click event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            var clientButton = sender as Button;
            var t = clientButton.Tag as TogglePattern;
            if (t == null)
                return;
            t.Toggle();
            _statusText.Text = "Element toggled " + t.Current.ToggleState;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the Invoke click event on the client control.
        ///     The client click handler calls Invoke() on the equivalent target control.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     The Tag property of the FrameworkElement, the client button in this
        ///     case, is used to store the InvokePattern object previously obtained
        ///     from the associated target control.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void Invoke_Click(object sender, RoutedEventArgs e)
        {
            var clientButton = sender as Button;
            var targetInvokePattern = clientButton.Tag as InvokePattern;
            if (targetInvokePattern == null)
                return;
            targetInvokePattern.Invoke();
            _statusText.Text = "Button invoked.";
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the ExpandCollapse click event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void ExpandCollapse_Click(object sender, RoutedEventArgs e)
        {
            var clientButton = sender as Button;
            var ec = clientButton.Tag as ExpandCollapsePattern;
            if (ec == null)
                return;
            var currentState = ec.Current.ExpandCollapseState;
            try
            {
                if ((currentState == ExpandCollapseState.Collapsed) ||
                    (currentState == ExpandCollapseState.PartiallyExpanded))
                {
                    ec.Expand();
                    _statusText.Text = "TreeItem expanded.";
                }
                else if (currentState == ExpandCollapseState.Expanded)
                {
                    ec.Collapse();
                    _statusText.Text = "TreeItem collapsed.";
                }
            }
            catch (InvalidOperationException)
            {
                // The current state of the element is LeafNode.
                Output("Unable to expand or collapse the element.");
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Output target information to the client.
        /// </summary>
        /// <param name="message">The feedback string to display.</param>
        /// <remarks>
        ///     Since the events are happening on a different thread than the
        ///     client we need to use a Dispatcher delegate to handle them.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void Output(string message)
        {
            _clientWindow.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new OutputDelegate(NotifyTargetEvent), message);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The delegate for the text changed event in the target.
        /// </summary>
        /// <param name="arg">string argument</param>
        /// --------------------------------------------------------------------
        private void NotifyTargetEvent(object arg)
        {
            var message = arg as string;
            _statusText.Text = message;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// --------------------------------------------------------------------
        internal sealed class TestMain
        {
            [STAThread]
            private static void Main()
            {
                // Create an instance of the sample class 
                // and call its Run() method to start it.
                var app = new InvokePatternApp();

                app.Run();
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles our application shutdown.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        /// --------------------------------------------------------------------
        protected override void OnExit(ExitEventArgs args)
        {
            if (_rootElement != null)
            {
                Automation.RemoveAllEventHandlers();
            }
            base.OnExit(args);
        }

        #endregion Client setup
    }
}