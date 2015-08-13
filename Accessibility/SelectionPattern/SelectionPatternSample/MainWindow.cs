// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Application = System.Windows.Forms.Application;

namespace SelectionPatternSample
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// --------------------------------------------------------------------
        /// <summary>
        ///     The various permutations of client controls.
        /// </summary>
        /// <remarks>
        ///     The state of the client application impacts which client controls
        ///     are available.
        /// </remarks>
        /// --------------------------------------------------------------------
        public enum ControlState
        {
            /// <summary>
            ///     Initial state of the client.
            /// </summary>
            Initial,

            /// <summary>
            ///     Target is available.
            /// </summary>
            TargetStarted,

            /// <summary>
            ///     The UI Automation worker has started.
            /// </summary>
            UIAStarted,

            /// <summary>
            ///     The UI Automation worker has stopped or target has been closed.
            /// </summary>
            UIAStopped
        };

        private readonly string _filePath =
            Application.StartupPath +
            "\\SelectionTarget.exe";

        private GroupBox[] _clientGroupBox;
        private ListBox[] _clientListBox;
        private TextBlock[] _clientTextBlockMultiple;
        private TextBlock[] _clientTextBlockRequired;
        private TextBlock[] _clientTextBlockSelected;
        private StringBuilder _feedbackText;
        private int _selectionControlCounter;
        private AutomationElement _targetApp;
        private TargetHandler _targetHandler;

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handle window loaded event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments</param>
        /// --------------------------------------------------------------------
        private void Client_OnLoad(object sender, RoutedEventArgs e)
        {
            SetClientControlState(ControlState.Initial);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the click event for the Start App button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void buttonStartTarget_Click(object sender, RoutedEventArgs e)
        {
            _targetApp = StartTargetApp(_filePath);

            if (_targetApp == null)
            {
                Feedback("Unable to start target application.");
                SetClientControlState(ControlState.UIAStopped);
                return;
            }
            Feedback("Target started.");

            _targetHandler = new TargetHandler(this, _targetApp);
            _targetHandler.StartWork();

            SetClientControlState(ControlState.TargetStarted);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Echo the target selection controls in the client.
        /// </summary>
        /// --------------------------------------------------------------------
        internal void EchoTargetControls()
        {
            textControlsCounter.Text =
                _targetHandler.TargetControls.Count.ToString();
            if (_targetHandler.TargetControls.Count <= 0)
            {
                return;
            }

            _clientGroupBox = new GroupBox[_targetHandler.TargetControls.Count];
            _clientListBox = new ListBox[_targetHandler.TargetControls.Count];
            _clientTextBlockMultiple = new TextBlock[_targetHandler.TargetControls.Count];
            _clientTextBlockRequired = new TextBlock[_targetHandler.TargetControls.Count];
            _clientTextBlockSelected = new TextBlock[_targetHandler.TargetControls.Count];

            for (var controlCounter = 0;
                controlCounter < _targetHandler.TargetControls.Count;
                controlCounter++)
            {
                // You can set the cache request tree scope to cache the 
                // children of the selection control at the same time.
                // However, this presents a problem with the combo box. 
                // Since the combo box is an aggregate control
                // the children include more than just the selection items.
                var selectionItemControls =
                    _targetHandler.GetSelectionItemsFromTarget(
                        _targetHandler.TargetControls[controlCounter]);

                // Initialize the control mappings.
                _clientGroupBox[controlCounter] = new GroupBox
                {
                    Padding = new Thickness(2),
                    Margin = new Thickness(10),
                    Header = _targetHandler.TargetControls[controlCounter].Current.AutomationId
                };
                stackControls.Children.Add(_clientGroupBox[controlCounter]);

                _clientTextBlockMultiple[controlCounter] = new TextBlock();
                _clientTextBlockRequired[controlCounter] = new TextBlock();
                _clientTextBlockSelected[controlCounter] = new TextBlock();
                SetControlPropertiesText(controlCounter);

                _clientListBox[controlCounter] = new ListBox();
                EchoTargetControlProperties(_targetHandler.TargetControls[controlCounter], controlCounter);

                var clientStackPanel = new StackPanel();
                clientStackPanel.Children.Add(_clientTextBlockMultiple[controlCounter]);
                clientStackPanel.Children.Add(_clientTextBlockRequired[controlCounter]);
                clientStackPanel.Children.Add(_clientTextBlockSelected[controlCounter]);
                clientStackPanel.Children.Add(_clientListBox[controlCounter]);
                _clientGroupBox[controlCounter].Content = clientStackPanel;

                _targetHandler.SetTargetSelectionEventHandlers();

                for (var selectionItem = 0;
                    selectionItem < selectionItemControls.Count;
                    selectionItem++)
                {
                    _clientListBox[controlCounter].Items.Add(
                        selectionItemControls[selectionItem].Current.Name);
                }
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Display the relevant target control properties.
        /// </summary>
        /// <param name="controlCounter">
        ///     Current selection control in target.
        /// </param>
        /// --------------------------------------------------------------------
        internal void SetControlPropertiesText(int controlCounter)
        {
            // Check if we need to call BeginInvoke.
            if (Dispatcher.CheckAccess() == false)
            {
                // Pass the same function to BeginInvoke,
                // but the call would come on the correct
                // thread and InvokeRequired will be false.
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new SetControlPropertiesTextDelegate(SetControlPropertiesText),
                    controlCounter);
                return;
            }

            string propertyText;
            propertyText =
                "Can select multiple: " +
                _targetHandler.TargetControls[controlCounter].GetCurrentPropertyValue(
                    SelectionPattern.CanSelectMultipleProperty);

            _clientTextBlockMultiple[controlCounter].Text = propertyText;
            _clientTextBlockMultiple[controlCounter].Foreground =
                Brushes.Black;
            _clientTextBlockMultiple[controlCounter].Background =
                ((bool) _targetHandler.TargetControls[controlCounter].GetCurrentPropertyValue(
                    SelectionPattern.CanSelectMultipleProperty))
                    ? Brushes.White
                    : Brushes.LightYellow;

            propertyText =
                "Is selection required: " +
                _targetHandler.TargetControls[controlCounter].GetCurrentPropertyValue(
                    SelectionPattern.IsSelectionRequiredProperty);
            _clientTextBlockRequired[controlCounter].Text = propertyText;
            _clientTextBlockRequired[controlCounter].Foreground =
                ((bool) _targetHandler.TargetControls[controlCounter].GetCurrentPropertyValue(
                    SelectionPattern.IsSelectionRequiredProperty))
                    ? Brushes.Blue
                    : Brushes.Black;

            propertyText =
                "Items selected: " +
                _targetHandler.GetTargetCurrentSelection(
                    _targetHandler.TargetControls[controlCounter]).Length;
            _clientTextBlockSelected[controlCounter].Text = propertyText;
            _clientTextBlockSelected[controlCounter].Foreground =
                Brushes.Black;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Echo the target control properties in the client.
        /// </summary>
        /// <param name="targetSelectionControl">
        ///     Current selection control in target.
        /// </param>
        /// <param name="controlCounter">
        ///     Index of current selection control.
        /// </param>
        /// --------------------------------------------------------------------
        internal void EchoTargetControlProperties(
            AutomationElement targetSelectionControl,
            int controlCounter)
        {
            // Check if we need to call BeginInvoke.
            if (Dispatcher.CheckAccess() == false)
            {
                // Pass the same function to BeginInvoke,
                // but the call would come on the correct
                // thread and InvokeRequired will be false.
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new EchoTargetControlPropertiesDelegate(EchoTargetControlProperties),
                    targetSelectionControl, controlCounter);
                return;
            }

            _clientListBox[controlCounter].Name =
                targetSelectionControl.Current.AutomationId;

            _clientListBox[controlCounter].SelectionMode =
                ((bool) targetSelectionControl.GetCurrentPropertyValue(
                    SelectionPattern.CanSelectMultipleProperty))
                    ? SelectionMode.Multiple
                    : SelectionMode.Single;

            _clientListBox[controlCounter].Background =
                (_clientListBox[controlCounter].SelectionMode ==
                 SelectionMode.Single)
                    ? Brushes.LightYellow
                    : Brushes.White;

            _clientListBox[controlCounter].Foreground =
                ((bool) targetSelectionControl.GetCurrentPropertyValue(
                    SelectionPattern.IsSelectionRequiredProperty))
                    ? Brushes.Blue
                    : Brushes.Black;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Mirrors target selections in client list boxes.
        /// </summary>
        /// <param name="targetSelectionControl">
        ///     Current target selection control.
        /// </param>
        /// <param name="controlCounter">
        ///     Index of current selection control.
        /// </param>
        /// --------------------------------------------------------------------
        internal void EchoTargetControlSelections(
            AutomationElement targetSelectionControl,
            int controlCounter)
        {
            // Check if we need to call BeginInvoke.
            if (Dispatcher.CheckAccess() == false)
            {
                // Pass the same function to BeginInvoke,
                // but the call would come on the correct
                // thread and InvokeRequired will be false.
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new EchoTargetControlSelectionsDelegate(EchoTargetControlSelections),
                    targetSelectionControl, controlCounter);
                return;
            }
            var targetSelectionItems =
                _targetHandler.GetSelectionItemsFromTarget(targetSelectionControl);
            for (var itemCounter = 0;
                itemCounter < targetSelectionItems.Count;
                itemCounter++)
            {
                var selectionItemPattern =
                    targetSelectionItems[itemCounter].GetCurrentPattern(
                        SelectionItemPattern.Pattern) as SelectionItemPattern;
                var listboxItem =
                    GetClientItemFromIndex(_clientListBox[controlCounter], itemCounter);
                listboxItem.IsSelected =
                    selectionItemPattern.Current.IsSelected;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Gets an item from a WPF list box based on index.
        /// </summary>
        /// <param name="listBox">
        ///     Current client list box.
        /// </param>
        /// <param name="index">
        ///     Index of current list box item.
        /// </param>
        /// --------------------------------------------------------------------
        internal ListBoxItem GetClientItemFromIndex(ListBox listBox, int index) => (ListBoxItem)(listBox.ItemContainerGenerator.ContainerFromIndex(index));

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the request to change the direction of user input from
        ///     client to target and vice versa.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void ChangeEchoDirection(object sender, RoutedEventArgs e)
        {
            Feedback("");

            var echoDirection = ((RadioButton) sender).Content.ToString();

            ResetSelectionHandlers(_clientListBox, echoDirection);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Manages the selection handlers for client and target.
        /// </summary>
        /// <param name="clientListBox">
        ///     The array of ListBox controls in the client.
        /// </param>
        /// <param name="echoDirection">
        ///     The direction to echo selection events (that is, client or target).
        /// </param>
        /// --------------------------------------------------------------------
        internal void ResetSelectionHandlers(ListBox[] clientListBox, string echoDirection)
        {
            for (var controlCounter = 0;
                controlCounter < _targetHandler.TargetControls.Count;
                controlCounter++)
            {
                _targetHandler.RemoveTargetSelectionEventHandlers();

                clientListBox[controlCounter].SelectionChanged -=
                    ClientSelectionHandler;

                if (echoDirection == "Client")
                {
                    _targetHandler.SetTargetSelectionEventHandlers();
                }
                else
                {
                    clientListBox[controlCounter].SelectionChanged +=
                        ClientSelectionHandler;
                }
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles user input in the client.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        internal void ClientSelectionHandler(
            object sender, SelectionChangedEventArgs e)
        {
            _feedbackText = new StringBuilder();
            var clientListBox = sender as ListBox;

            foreach (
                AutomationElement targetControl in
                    _targetHandler.TargetControls)
            {
                if (clientListBox.Name ==
                    targetControl.Current.AutomationId)
                {
                    var selectionPattern =
                        targetControl.GetCurrentPattern(
                            SelectionPattern.Pattern) as SelectionPattern;
                    var targetSelectionItems =
                        _targetHandler.GetSelectionItemsFromTarget(targetControl);

                    if (e.AddedItems.Count > 0)
                    {
                        for (var itemCounter = 0;
                            itemCounter < clientListBox.Items.Count;
                            itemCounter++)
                        {
                            var listboxItem =
                                GetClientItemFromIndex(
                                    clientListBox, itemCounter);
                            var selectionItemPattern =
                                targetSelectionItems[itemCounter].GetCurrentPattern(
                                    SelectionItemPattern.Pattern) as
                                    SelectionItemPattern;

                            if (!(selectionItemPattern.Current.IsSelected) &&
                                listboxItem.IsSelected)
                            {
                                if (selectionPattern.Current.GetSelection().Length < 1)
                                {
                                    try
                                    {
                                        selectionItemPattern.Select();
                                        _feedbackText.Append(
                                            targetSelectionItems[itemCounter].Current.Name)
                                            .Append(" of the ")
                                            .Append(
                                                targetControl.Current.AutomationId)
                                            .Append(" control was selected.");
                                        Feedback(_feedbackText.ToString());
                                    }
                                    catch (InvalidOperationException exc)
                                    {
                                        Feedback(exc.Message);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (selectionPattern.Current.CanSelectMultiple)
                                        {
                                            selectionItemPattern.AddToSelection();
                                            _feedbackText.Append(
                                                targetSelectionItems[itemCounter].Current.Name)
                                                .Append(" of the ")
                                                .Append(
                                                    targetControl.Current.AutomationId)
                                                .Append(
                                                    " control was added to the selection.");
                                            Feedback(_feedbackText.ToString());
                                        }
                                        else
                                        {
                                            selectionItemPattern.Select();
                                            _feedbackText.Append(
                                                targetSelectionItems[itemCounter].Current.Name)
                                                .Append(" of the ")
                                                .Append(
                                                    targetControl.Current.AutomationId)
                                                .Append(" control was selected.");
                                            Feedback(_feedbackText.ToString());
                                        }
                                    }
                                    catch (InvalidOperationException exc)
                                    {
                                        Feedback(exc.Message);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (e.RemovedItems.Count > 0)
                    {
                        for (var itemCounter = 0;
                            itemCounter < clientListBox.Items.Count;
                            itemCounter++)
                        {
                            var listboxItem =
                                GetClientItemFromIndex(clientListBox, itemCounter);
                            var selectionItemPattern =
                                targetSelectionItems[itemCounter].GetCurrentPattern(
                                    SelectionItemPattern.Pattern) as
                                    SelectionItemPattern;

                            if ((selectionItemPattern.Current.IsSelected)
                                && !listboxItem.IsSelected)
                            {
                                try
                                {
                                    selectionItemPattern.RemoveFromSelection();
                                    _feedbackText.Append(
                                        targetSelectionItems[itemCounter].Current.Name)
                                        .Append(" of the ")
                                        .Append(
                                            targetControl.Current.AutomationId)
                                        .Append(
                                            " control was removed from the selection.");
                                    Feedback(_feedbackText.ToString());
                                    break;
                                }
                                catch (InvalidOperationException exc)
                                {
                                    Feedback(exc.Message);
                                }
                            }
                        }
                    }
                }
                for (var controlCounter = 0;
                    controlCounter < _targetHandler.TargetControls.Count;
                    controlCounter++)
                {
                    SetControlPropertiesText(controlCounter);
                    EchoTargetControlProperties(
                        _targetHandler.TargetControls[controlCounter],
                        controlCounter);
                }
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Starts the target application.
        /// </summary>
        /// <returns>The target automation element.</returns>
        /// --------------------------------------------------------------------
        private AutomationElement StartTargetApp(string target)
        {
            if (!File.Exists(target))
            {
                _feedbackText.Append(target);
                _feedbackText.Append(" not found.");
                Feedback(_feedbackText.ToString());
                return null;
            }
            try
            {
                // Start target application.
                var startInfo = new ProcessStartInfo(target)
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true
                };

                var p = Process.Start(startInfo);

                // Give the target application some time to startup.
                // For Win32 applications, WaitForInputIdle can be used instead.
                // Another alternative is to listen for WindowOpened events.
                // Otherwise, an ArgumentException results when you try to
                // retrieve an automation element from the window handle.
                Thread.Sleep(5000);

                // Return the automation element
                var windowHandle = p.MainWindowHandle;
                return (AutomationElement.FromHandle(windowHandle));
            }
            catch (ArgumentException)
            {
                // To do: error handling
                return null;
            }
            catch (Win32Exception)
            {
                // To do: error handling
                return null;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Outputs information to the client window.
        /// </summary>
        /// <param name="message">The string to display.</param>
        /// --------------------------------------------------------------------
        internal void Feedback(string message)
        {
            // Check if we need to call BeginInvoke.
            if (Dispatcher.CheckAccess() == false)
            {
                // Pass the same function to BeginInvoke,
                // but the call would come on the correct
                // thread and InvokeRequired will be false.
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new FeedbackDelegate(Feedback),
                    message);
                return;
            }
            textFeedback.Text = message + Environment.NewLine;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Modifies the state of the controls on the form.
        /// </summary>
        /// <param name="controlState">
        ///     The current state of the target application.
        /// </param>
        /// <remarks>
        ///     Check thread safety for client updates based on target events.
        /// </remarks>
        /// --------------------------------------------------------------------
        internal void SetClientControlState(ControlState controlState)
        {
            // Check if we need to call BeginInvoke.
            if (Dispatcher.CheckAccess() == false)
            {
                // Pass the same function to BeginInvoke,
                // but the call would come on the correct
                // thread and InvokeRequired will be false.
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new SetClientControlStateDelegate(SetClientControlState),
                    controlState);
                return;
            }
            switch (controlState)
            {
                case ControlState.Initial:
                    buttonStartTarget.IsEnabled = true;
                    textControlsCounter.Text = _selectionControlCounter.ToString();
                    Echo.Visibility = Visibility.Hidden;
                    break;
                case ControlState.TargetStarted:
                    buttonStartTarget.IsEnabled = false;
                    Echo.Visibility = Visibility.Visible;
                    break;
                case ControlState.UIAStarted:
                    buttonStartTarget.IsEnabled = false;
                    textControlsCounter.Text = _selectionControlCounter.ToString();
                    Echo.Visibility = Visibility.Visible;
                    break;
                case ControlState.UIAStopped:
                    buttonStartTarget.IsEnabled = true;
                    buttonStartTarget.Content = "Target has been closed - Restart";
                    echoClient.IsChecked = true;
                    _selectionControlCounter = 0;
                    stackControls.Children.Clear();
                    textControlsCounter.Text = _selectionControlCounter.ToString();
                    Echo.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Thread-safe delegate.
        /// </summary>
        /// <param name="controlCounter">
        ///     Index of current selection control.
        /// </param>
        /// --------------------------------------------------------------------
        internal delegate void SetControlPropertiesTextDelegate(int controlCounter);

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Thread-safe delegate.
        /// </summary>
        /// <param name="targetSelectionControl">
        ///     Current selection control in target.
        /// </param>
        /// <param name="controlCounter">
        ///     Index of current selection control.
        /// </param>
        /// --------------------------------------------------------------------
        internal delegate void EchoTargetControlPropertiesDelegate(
            AutomationElement targetSelectionControl,
            int controlCounter);

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Thread-safe delegate.
        /// </summary>
        /// <param name="targetSelectionControl">
        ///     Current selection control in target.
        /// </param>
        /// <param name="controlCounter">
        ///     Index of current selection control.
        /// </param>
        /// --------------------------------------------------------------------
        internal delegate void EchoTargetControlSelectionsDelegate(
            AutomationElement targetSelectionControl,
            int controlCounter);

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Thread-safe delegate.
        /// </summary>
        /// <param name="message">The string to display.</param>
        /// --------------------------------------------------------------------
        internal delegate void FeedbackDelegate(string message);

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Thread-safe delegate.
        /// </summary>
        /// <param name="controlState">
        ///     The current state of the target application.
        /// </param>
        /// --------------------------------------------------------------------
        internal delegate void SetClientControlStateDelegate(ControlState controlState);
    }
}