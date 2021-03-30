// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataBindingDemo
{
    /// <summary>
    ///     Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void OnInit(object sender, RoutedEventArgs e)
        {
            DataContext = new AuctionItem("Type your description here",
                ProductCategory.DvDs, 1, DateTime.Now, ((App) Application.Current).CurrentUser,
                SpecialFeatures.None);
        }

        private void AnnounceError(string message)
        {
            ErrorTextBlock.Visibility = Visibility.Visible;
            ErrorTextBlock.Text = message;
            if (AutomationPeer.ListenerExists(AutomationEvents.LiveRegionChanged))
            {
                var automationPeer = UIElementAutomationPeer.CreatePeerForElement(ErrorTextBlock);
                automationPeer?.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }

        private void SubmitProduct(object sender, RoutedEventArgs e)
        {
            var automationPeer = UIElementAutomationPeer.CreatePeerForElement(ErrorTextBlock);

            if(StartDateEntryForm.Text.Length == 0 || StartPriceEntryForm.Text.Length == 0)
            {
                AnnounceError("Please, fill both date and start price");
            }
            else if (Validation.GetHasError(StartDateEntryForm))
            {
                AnnounceError("Please, enter a valid date");
            }
            else if (Validation.GetHasError(StartPriceEntryForm))
            {
                AnnounceError("Please, enter a valid price");
            }
            else
            {
                var item = (AuctionItem)(DataContext);
                ((App) Application.Current).AuctionItems.Add(item);
                Close();
            }

        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var peer = UIElementAutomationPeer.FromElement(sender as ComboBox);
            if(peer != null)
            {
                peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            // Get the current UIA ItemStatus from the element element. 
            var oldStatus = AutomationProperties.GetItemStatus((DependencyObject)sender);

            // Set some sample new ItemStatus here... 
            var newStatus = e.Action == ValidationErrorEventAction.Added ? e.Error.ErrorContent.ToString() : String.Empty;
            AutomationProperties.SetItemStatus((DependencyObject)sender, newStatus);
            
            // Having just set the new ItemStatus, raise a UIA property changed event. Note that the peer may 
            // be null here unless a UIA client app such as Narrator or the AccEvent SDK tool are running. 
            var automationPeer = UIElementAutomationPeer.FromElement((UIElement)sender);
            automationPeer?.RaisePropertyChangedEvent(AutomationElementIdentifiers.ItemStatusProperty, oldStatus, newStatus);
        }
    }
}
