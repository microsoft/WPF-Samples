// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageBrush
{
    /// <summary>
    ///     Interaction logic for TextFillsExample.xaml
    /// </summary>
    public partial class InteractiveExample : Page
    {
        private RadioButton _selectedButton;

        private void InteractiveExampleLoaded(object sender, RoutedEventArgs args)
        {
            LoadInteractiveMenus();
            MyDefaultImageButton.IsChecked = true;
        }

        // Initializes the image brush menu options.
        private void LoadInteractiveMenus()
        {
            var values = Enum.GetNames(typeof (Stretch));
            foreach (var stretchMode in values)
                stretchSelector.Items.Add(stretchMode);
            stretchSelector.SelectedItem = myImageBrush.Stretch.ToString();

            values = Enum.GetNames(typeof (AlignmentX));
            foreach (var hAlign in values)
                horizontalAlignmentSelector.Items.Add(hAlign);
            horizontalAlignmentSelector.SelectedItem = myImageBrush.AlignmentX.ToString();

            values = Enum.GetNames(typeof (AlignmentY));
            foreach (var vAlign in values)
                verticalAlignmentSelector.Items.Add(vAlign);
            verticalAlignmentSelector.SelectedItem = myImageBrush.AlignmentY.ToString();

            values = Enum.GetNames(typeof (TileMode));
            foreach (var tileMode in values)
                tileSelector.Items.Add(tileMode);
            tileSelector.SelectedItem = myImageBrush.TileMode.ToString();

            values = Enum.GetNames(typeof (BrushMappingMode));
            foreach (var mappingMode in values)
            {
                viewportUnitsSelector.Items.Add(mappingMode);
                viewboxUnitsSelector.Items.Add(mappingMode);
            }
            viewportUnitsSelector.SelectedItem = myImageBrush.ViewportUnits.ToString();
            viewboxUnitsSelector.SelectedItem = myImageBrush.ViewboxUnits.ToString();
            viewportEntry.Text = myImageBrush.Viewport.ToString();
            viewboxEntry.Text = myImageBrush.Viewbox.ToString();
        }

        private void SetSelectedButton(object sender, RoutedEventArgs args)
        {
            _selectedButton = sender as RadioButton;
        }

        // Applies the selected options to the image brush.
        private void UpdateBrush(object sender, RoutedEventArgs args)
        {
            try
            {
                myImageBrush.ImageSource = (_selectedButton.Content as Image).Source;
                myImageBrush.Stretch = (Stretch) Enum.Parse(typeof (Stretch), (string) stretchSelector.SelectedItem);
                myImageBrush.AlignmentX =
                    (AlignmentX) Enum.Parse(typeof (AlignmentX), (string) horizontalAlignmentSelector.SelectedItem);
                myImageBrush.AlignmentY =
                    (AlignmentY) Enum.Parse(typeof (AlignmentY), (string) verticalAlignmentSelector.SelectedItem);
                myImageBrush.TileMode = (TileMode) Enum.Parse(typeof (TileMode), (string) tileSelector.SelectedItem);
                myImageBrush.ViewportUnits =
                    (BrushMappingMode)
                        Enum.Parse(typeof (BrushMappingMode), (string) viewportUnitsSelector.SelectedItem);
                myImageBrush.ViewboxUnits =
                    (BrushMappingMode) Enum.Parse(typeof (BrushMappingMode), (string) viewboxUnitsSelector.SelectedItem);

                var myRectConverter = new RectConverter();
                var parseString = viewportEntry.Text;

                if (!string.IsNullOrEmpty(parseString))
                    myImageBrush.Viewport = (Rect) myRectConverter.ConvertFromString(parseString);
                else
                {
                    myImageBrush.Viewport = Rect.Empty;
                    viewportEntry.Text = "Empty";
                }

                parseString = viewboxEntry.Text;

                if (!string.IsNullOrEmpty(parseString) && parseString.ToLower() != "(auto)")
                    myImageBrush.Viewbox = (Rect) myRectConverter.ConvertFromString(parseString);
                else
                {
                    viewboxEntry.Text = "Empty";
                    myImageBrush.Viewbox = Rect.Empty;
                }
            }
            catch (InvalidOperationException invalidOpEx)
            {
                MessageBox.Show("Invalid Viewport or Viewbox. " + invalidOpEx);
            }
            catch (FormatException formatEx)
            {
                MessageBox.Show("Invalid Viewport or Viewbox. " + formatEx);
            }
        }
    }
}