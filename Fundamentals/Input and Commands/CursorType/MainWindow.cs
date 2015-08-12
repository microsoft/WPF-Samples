// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CursorType
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Cursor _customCursor;
        private bool _cursorScopeElementOnly = true;

        public MainWindow()
        {
            InitializeComponent();
            // Setting CustomCursor to the CustomCursor.cur file.
            // This assumes the file CustomCursor.cur has been added to the project
            // as a resource.  One way to accomplish this to add the following 
            // ItemGroup section to the project file
            //
            //  <ItemGroup>
            //    <Content Include="CustomCursor.cur">
            //       <CopyToOutputDirectory>Always</CopyToOutputDirectory>
            //    </Content>
            //  </ItemGroup>
            _customCursor = new Cursor(Directory.GetCurrentDirectory() +
                                       @"\" +
                                       "CustomCursor.cur");
        }

        private void CursorTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            var source = e.Source as ComboBox;

            if (source != null)
            {
                var selectedCursor = source.SelectedItem as ComboBoxItem;

                // Changing the cursor of the Border control 
                // by setting the Cursor property
                switch (selectedCursor.Content.ToString())
                {
                    case "AppStarting":
                        DisplayArea.Cursor = Cursors.AppStarting;
                        break;
                    case "ArrowCD":
                        DisplayArea.Cursor = Cursors.ArrowCD;
                        break;
                    case "Arrow":
                        DisplayArea.Cursor = Cursors.Arrow;
                        break;
                    case "Cross":
                        DisplayArea.Cursor = Cursors.Cross;
                        break;
                    case "HandCursor":
                        DisplayArea.Cursor = Cursors.Hand;
                        break;
                    case "Help":
                        DisplayArea.Cursor = Cursors.Help;
                        break;
                    case "IBeam":
                        DisplayArea.Cursor = Cursors.IBeam;
                        break;
                    case "No":
                        DisplayArea.Cursor = Cursors.No;
                        break;
                    case "None":
                        DisplayArea.Cursor = Cursors.None;
                        break;
                    case "Pen":
                        DisplayArea.Cursor = Cursors.Pen;
                        break;
                    case "ScrollSE":
                        DisplayArea.Cursor = Cursors.ScrollSE;
                        break;
                    case "ScrollWE":
                        DisplayArea.Cursor = Cursors.ScrollWE;
                        break;
                    case "SizeAll":
                        DisplayArea.Cursor = Cursors.SizeAll;
                        break;
                    case "SizeNESW":
                        DisplayArea.Cursor = Cursors.SizeNESW;
                        break;
                    case "SizeNS":
                        DisplayArea.Cursor = Cursors.SizeNS;
                        break;
                    case "SizeNWSE":
                        DisplayArea.Cursor = Cursors.SizeNWSE;
                        break;
                    case "SizeWE":
                        DisplayArea.Cursor = Cursors.SizeWE;
                        break;
                    case "UpArrow":
                        DisplayArea.Cursor = Cursors.UpArrow;
                        break;
                    case "WaitCursor":
                        DisplayArea.Cursor = Cursors.Wait;
                        break;
                    case "Custom":
                        DisplayArea.Cursor = _customCursor;
                        break;
                }

                // If the cursor scope is set to the entire application
                // Use OverrideCursor to force the cursor for all elements
                if (_cursorScopeElementOnly == false)
                {
                    Mouse.OverrideCursor = DisplayArea.Cursor;
                }
            }
        }

        // Determines the scope the new cursor will have.
        //
        // If the RadioButton rbScopeElement is selected, then the cursor
        // will only change on the display element.
        // 
        // If the Radiobutton rbScopeApplication is selected, then the cursor
        // will be changed for the entire application
        //
        private void CursorScopeSelected(object sender, RoutedEventArgs e)
        {
            var source = e.Source as RadioButton;

            if (source != null)
            {
                if (source.Name == "rbScopeElement")
                {
                    // Setting the element only scope flag to true
                    _cursorScopeElementOnly = true;

                    // Clearing out the OverrideCursor.  
                    Mouse.OverrideCursor = null;
                }
                if (source.Name == "rbScopeApplication")
                {
                    // Setting the element only scope flag to false
                    _cursorScopeElementOnly = false;

                    // Forcing the cursor for all elements. 
                    Mouse.OverrideCursor = DisplayArea.Cursor;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // When the UI is finished loading, make the arrorw cursor
            // the default cursor in the CursorSelector combobox
            ((ComboBoxItem) CursorSelector.Items[2]).IsSelected = true;
        }
    }
}