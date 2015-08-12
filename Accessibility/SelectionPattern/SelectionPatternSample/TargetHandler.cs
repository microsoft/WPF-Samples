// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Windows.Automation;

namespace SelectionPatternSample
{
    /// --------------------------------------------------------------------
    /// <summary>
    ///     UI Automation worker class.
    /// </summary>
    /// --------------------------------------------------------------------
    internal class TargetHandler
    {
        private readonly MainWindow _clientApp;
        private readonly AutomationElement _targetApp;
        private StringBuilder _feedbackText;
        private AutomationElement _rootElement;
        private AutomationEventHandler _targetCloseListener;

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="client">The client application.</param>
        /// <param name="target">The target application.</param>
        /// <remarks>
        ///     Initializes components.
        /// </remarks>
        /// --------------------------------------------------------------------
        internal TargetHandler(MainWindow client, AutomationElement target)
        {
            // Initialize member variables.
            _clientApp = client;
            _targetApp = target;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The collection of Selection controls in the target.
        /// </summary>
        /// --------------------------------------------------------------------
        internal AutomationElementCollection TargetControls { get; private set; }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Start the UI Automation worker.
        /// </summary>
        /// --------------------------------------------------------------------
        internal void StartWork()
        {
            // Get UI Automation root element.
            _rootElement = AutomationElement.RootElement;

            // Position the target relative to the client.
            var clientLocationTop = _clientApp.Top;
            var clientLocationRight = _clientApp.Width + _clientApp.Left + 100;
            var transformPattern =
                _targetApp.GetCurrentPattern(TransformPattern.Pattern)
                    as TransformPattern;
            transformPattern?.Move(clientLocationRight, clientLocationTop);

            RegisterTargetClosedListener();

            // Get the target controls.
            CompileTargetControls();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Register the target closed event listener.
        /// </summary>
        /// --------------------------------------------------------------------
        private void RegisterTargetClosedListener()
        {
            _targetCloseListener = OnTargetClosed;
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowClosedEvent,
                _targetApp,
                TreeScope.Element,
                _targetCloseListener);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     The target closed event handler.
        /// </summary>
        /// <param name="src">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Changes the state of client controls and removes event listeners.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void OnTargetClosed(object src, AutomationEventArgs e)
        {
            Feedback("Target has been closed. Please wait.");
            // Automation.RemoveAllEventHandlers is not used here since we don't
            // want to lose the window closed event listener for the target.
            RemoveTargetSelectionEventHandlers();

            Automation.RemoveAutomationEventHandler(
                WindowPattern.WindowClosedEvent, _targetApp, _targetCloseListener);
            _clientApp.SetClientControlState(MainWindow.ControlState.UIAStopped);

            Feedback(null);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Finds the selection and selection item controls of interest
        ///     and initializes necessary event listeners.
        /// </summary>
        /// <remarks>
        ///     Handles the special case of two Selection controls being returned
        ///     for a ComboBox control.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void CompileTargetControls()
        {
            var condition =
                new PropertyCondition(
                    AutomationElement.IsSelectionPatternAvailableProperty, true);
            // A ComboBox is an aggregate control containing a ListBox 
            // as a child. 
            // Both the ComboBox and its child ListBox support the 
            // SelectionPattern control pattern but all related 
            // functionality is delegated to the child.
            // For the purposes of this sample we can filter the child 
            // as we do not need to display the redundant information.
            var andCondition = new AndCondition(
                condition,
                new NotCondition(
                    new PropertyCondition(
                        AutomationElement.ClassNameProperty, "ComboLBox")));
            TargetControls =
                _targetApp.FindAll(TreeScope.Children |
                                   TreeScope.Descendants, andCondition);
            _clientApp.EchoTargetControls();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Gets the currently selected SelectionItem objects from target.
        /// </summary>
        /// <param name="selectionContainer">
        ///     The target Selection container.
        /// </param>
        /// --------------------------------------------------------------------
        internal AutomationElement[] GetTargetCurrentSelection(
            AutomationElement selectionContainer)
        {
            try
            {
                var selectionPattern =
                    selectionContainer.GetCurrentPattern(
                        SelectionPattern.Pattern) as SelectionPattern;
                return selectionPattern.Current.GetSelection();
            }
            catch (InvalidOperationException exc)
            {
                Feedback(exc.Message);
                return null;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Subscribe to the selection events.
        /// </summary>
        /// <remarks>
        ///     The events are raised by the SelectionItem elements,
        ///     not the Selection container.
        /// </remarks>
        /// --------------------------------------------------------------------
        internal void SetTargetSelectionEventHandlers()
        {
            foreach (AutomationElement control in TargetControls)
            {
                var selectionHandler =
                    new AutomationEventHandler(TargetSelectionHandler);
                Automation.AddAutomationEventHandler(
                    SelectionItemPattern.ElementSelectedEvent,
                    control,
                    TreeScope.Element | TreeScope.Descendants,
                    selectionHandler);
                Automation.AddAutomationEventHandler(
                    SelectionItemPattern.ElementAddedToSelectionEvent,
                    control,
                    TreeScope.Element | TreeScope.Descendants,
                    selectionHandler);
                Automation.AddAutomationEventHandler(
                    SelectionItemPattern.ElementRemovedFromSelectionEvent,
                    control,
                    TreeScope.Element | TreeScope.Descendants,
                    selectionHandler);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Unsubscribe from the selection events.
        /// </summary>
        /// <remarks>
        ///     The events are raised by the SelectionItem elements,
        ///     not the Selection container.
        /// </remarks>
        /// --------------------------------------------------------------------
        internal void RemoveTargetSelectionEventHandlers()
        {
            foreach (AutomationElement control in TargetControls)
            {
                var selectionHandler =
                    new AutomationEventHandler(TargetSelectionHandler);
                Automation.RemoveAutomationEventHandler(
                    SelectionItemPattern.ElementSelectedEvent,
                    control,
                    selectionHandler);
                Automation.RemoveAutomationEventHandler(
                    SelectionItemPattern.ElementAddedToSelectionEvent,
                    control,
                    selectionHandler);
                Automation.RemoveAutomationEventHandler(
                    SelectionItemPattern.ElementRemovedFromSelectionEvent,
                    control,
                    selectionHandler);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles user input in the target.
        /// </summary>
        /// <param name="src">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void TargetSelectionHandler(
            object src, AutomationEventArgs e)
        {
            _feedbackText = new StringBuilder();

            // Get the name of the item, which is equivalent to its text.
            var sourceItem = src as AutomationElement;

            var selectionItemPattern =
                sourceItem.GetCurrentPattern(SelectionItemPattern.Pattern)
                    as SelectionItemPattern;

            AutomationElement sourceContainer;

            // Special case handling for composite controls.
            var treeWalker = new TreeWalker(new PropertyCondition(
                AutomationElement.IsSelectionPatternAvailableProperty,
                true));
            sourceContainer =
                (treeWalker.GetParent(
                    selectionItemPattern.Current.SelectionContainer) == null)
                    ? selectionItemPattern.Current.SelectionContainer
                    : treeWalker.GetParent(
                        selectionItemPattern.Current.SelectionContainer);

            switch (e.EventId.ProgrammaticName)
            {
                case
                    "SelectionItemPatternIdentifiers.ElementSelectedEvent":
                    _feedbackText.Append(sourceItem.Current.Name)
                        .Append(" of the ")
                        .Append(sourceContainer.Current.AutomationId)
                        .Append(" control was selected.");
                    break;
                case
                    "SelectionItemPatternIdentifiers.ElementAddedToSelectionEvent":
                    _feedbackText.Append(sourceItem.Current.Name)
                        .Append(" of the ")
                        .Append(sourceContainer.Current.AutomationId)
                        .Append(" control was added to the selection.");
                    break;
                case
                    "SelectionItemPatternIdentifiers.ElementRemovedFromSelectionEvent":
                    _feedbackText.Append(sourceItem.Current.Name)
                        .Append(" of the ")
                        .Append(sourceContainer.Current.AutomationId)
                        .Append(" control was removed from the selection.");
                    break;
            }
            Feedback(_feedbackText.ToString());

            for (var controlCounter = 0;
                controlCounter < TargetControls.Count;
                controlCounter++)
            {
                _clientApp.SetControlPropertiesText(controlCounter);
                _clientApp.EchoTargetControlSelections(
                    TargetControls[controlCounter],
                    controlCounter);
                _clientApp.EchoTargetControlProperties(
                    TargetControls[controlCounter],
                    controlCounter);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Gets the selection items from each target control.
        /// </summary>
        /// <param name="selectionContainer">
        ///     The target Selection container.
        /// </param>
        /// <returns>
        ///     Automation elements that satisfy the specified conditions.
        /// </returns>
        /// --------------------------------------------------------------------
        internal AutomationElementCollection
            GetSelectionItemsFromTarget(AutomationElement selectionContainer)
        {
            var condition =
                new PropertyCondition(
                    AutomationElement.IsSelectionItemPatternAvailableProperty,
                    true);
            return selectionContainer.FindAll(TreeScope.Children |
                                              TreeScope.Descendants, condition);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Prints a line of text to the textbox.
        /// </summary>
        /// <param name="outputStr">The string to print.</param>
        /// --------------------------------------------------------------------
        private void Feedback(string outputStr)
        {
            _clientApp.Feedback(outputStr);
        }
    }
}