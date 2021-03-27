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

        private void SubmitProduct(object sender, RoutedEventArgs e)
        {
            var item = (AuctionItem) (DataContext);
            ((App) Application.Current).AuctionItems.Add(item);
            Close();
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
