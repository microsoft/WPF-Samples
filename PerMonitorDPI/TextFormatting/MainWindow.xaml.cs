using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextFormatting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
        {
            _pixelsPerDip = newDpiScaleInfo.PixelsPerDip;
            UpdateFormattedText(_pixelsPerDip);
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            _pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            // Enumerate the fonts and add them to the font family combobox.
            foreach (System.Windows.Media.FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                fontFamilyCB.Items.Add(fontFamily.Source);
            }
            fontFamilyCB.SelectedIndex = 7;

            // Load the font size combo box with common font sizes.
            for (int i = 0; i < CommonFontSizes.Length; i++)
            {
                fontSizeCB.Items.Add(CommonFontSizes[i]);
            }
            fontSizeCB.SelectedIndex = 21;

            //Load capitals combo box
            typographyMenuBar.Visibility = Visibility.Collapsed;

            //Set up the initial render state of the drawn text.
            if (_currentRendering == null)
            {
                _currentRendering = new FontRendering(
                   (double)fontSizeCB.SelectedItem,
                   TextAlignment.Left,
                   null,
                   System.Windows.Media.Brushes.Black,
                   new Typeface("Arial"));
            }
            
            _UILoaded = true;    //All UI is loaded, can handle events now
            UpdateFormattedText(_pixelsPerDip);
        }

        /// <summary>
        /// Method for starting the text formatting process. Each event handler
        /// will call this after the current fontrendering is updated.
        /// </summary>
        private void UpdateFormattedText(double pixelsPerDip)
        {
            // Make sure all UI is loaded
            if (!_UILoaded)
                return;

            // Initialize the text store.
            _textStore = new CustomTextSource(_pixelsPerDip);

            int textStorePosition = 0;                //Index into the text of the textsource
            System.Windows.Point linePosition = new System.Windows.Point(0, 0);     //current line

            // Create a DrawingGroup object for storing formatted text.
            textDest = new DrawingGroup();
            DrawingContext dc = textDest.Open();

            // Update the text store.
            _textStore.Text = textToFormat.Text;
            _textStore.FontRendering = _currentRendering;

            // Create a TextFormatter object.
            TextFormatter formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            while (textStorePosition < _textStore.Text.Length)
            {
                // Create a textline from the text store using the TextFormatter object.
                using (TextLine myTextLine = formatter.FormatLine(
                    _textStore,
                    textStorePosition,
                    96 * 6,
                    new GenericTextParagraphProperties(_currentRendering, _pixelsPerDip),
                    null))
                {
                    // Draw the formatted text into the drawing context.
                    myTextLine.Draw(dc, linePosition, InvertAxes.None);

                    // Update the index position in the text store.
                    textStorePosition += myTextLine.Length;

                    // Update the line position coordinate for the displayed line.
                    linePosition.Y += myTextLine.Height;
                }
            }

            // Persist the drawn text content.
            dc.Close();

            // Display the formatted text in the DrawingGroup object.
            myDrawingBrush.Drawing = textDest;
        }

        /// <summary>
        /// Event handler for when the user selects a new font size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FontSizeChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            // Make sure all UI is loaded
            if (!_UILoaded)
                return;

            // Determine whether a new font size has been selected.
            if (_currentRendering.FontSize != (double)fontSizeCB.SelectedItem)
            {
                _currentRendering.FontSize = (double)fontSizeCB.SelectedItem;
                UpdateFormattedText(_pixelsPerDip);
            }
        }

        /// <summary>
        /// Event handler for when the user selects a new font family
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FontFamilyChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            // Make sure all UI is loaded
            if (!_UILoaded)
                return;

            // Determine whether a new font family has been selected.
            if (_currentRendering.Typeface.FontFamily.Source != (string)fontFamilyCB.SelectedItem)
            {
                Typeface oldFace = _currentRendering.Typeface;
                Typeface newFace = new Typeface(
                   new System.Windows.Media.FontFamily((string)fontFamilyCB.SelectedItem),
                   oldFace.Style, oldFace.Weight, oldFace.Stretch);

                _currentRendering.Typeface = newFace;
                UpdateFormattedText(_pixelsPerDip);
            }
        }

        /// <summary>
        /// Event handler for when the bold button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoldClickedEventHandler(object sender, RoutedEventArgs e)
        {
            // Make sure All UI is loaded
            if (!_UILoaded)
                return;

            ToggleButton toggle = sender as ToggleButton;

            Typeface oldFace = _currentRendering.Typeface;
            Typeface newFace;

            if (toggle.IsChecked == true)
            {
                newFace = new Typeface(oldFace.FontFamily,
                   oldFace.Style, FontWeights.Bold, oldFace.Stretch);
            }
            else
            {
                newFace = new Typeface(oldFace.FontFamily,
                   oldFace.Style, FontWeights.Normal, oldFace.Stretch);
            }
            _currentRendering.Typeface = newFace;
            UpdateFormattedText(_pixelsPerDip);
        }

        /// <summary>
        /// Event handler for when the italic button is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItalicClickEventHandler(object sender, RoutedEventArgs e)
        {
            // Make sure All UI is loaded
            if (!_UILoaded)
                return;

            ToggleButton toggle = sender as ToggleButton;

            Typeface oldFace = _currentRendering.Typeface;
            Typeface newFace;

            if (toggle.IsChecked == true)
            {
                newFace = new Typeface(oldFace.FontFamily,
                   FontStyles.Italic, oldFace.Weight, oldFace.Stretch);
            }
            else
            {
                newFace = new Typeface(oldFace.FontFamily,
                   FontStyles.Normal, oldFace.Weight, oldFace.Stretch);
            }
            _currentRendering.Typeface = newFace;
            UpdateFormattedText(_pixelsPerDip);
        }

        /// <summary>
        /// Event handler for when a new decoration is selected/deselected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecorationClickEventHandler(object sender, RoutedEventArgs e)
        {
            // Make sure All UI is loaded
            if (!_UILoaded)
                return;

            TextDecorationCollection tds = new TextDecorationCollection();

            if (underlineButton.IsChecked == true)
            {
                TextDecoration underline = new TextDecoration();
                underline.Location = TextDecorationLocation.Underline;
                underline.Pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 1);
                underline.PenThicknessUnit = TextDecorationUnit.FontRecommended;
                tds.Add(underline);
            }
            if (strikeButton.IsChecked == true)
            {
                TextDecoration strikethrough = new TextDecoration();
                strikethrough.Location = TextDecorationLocation.Strikethrough;
                strikethrough.Pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 1);
                strikethrough.PenThicknessUnit = TextDecorationUnit.FontRecommended;
                tds.Add(strikethrough);
            }
            _currentRendering.TextDecorations = tds;
            UpdateFormattedText(_pixelsPerDip);
        }

        /// <summary>
        /// Event handler for text changed events from the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            //Make sure All UI is loaded
            if (!_UILoaded)
                return;

            UpdateFormattedText(_pixelsPerDip);
        }

        /// <summary>
        /// Event handler for the handling checked events on the Alignment buttons
        /// </summary>
        /// <param name="sender">The button that was checked.</param>
        /// <param name="e">N/A</param>
        private void AlignmentChanged(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (ToggleButton)sender;

            // Make sure All UI is loaded
            if (!_UILoaded)
                return;
            // Ignore all non-checked events.
            if (btn.IsChecked == false)
                return;

            switch (btn.Name)
            {
                case ("leftAlign"):
                    _currentRendering.TextAlignment = TextAlignment.Left;
                    centerAlign.IsChecked = false;
                    rightAlign.IsChecked = false;
                    justify.IsChecked = false;
                    break;
                case ("centerAlign"):
                    _currentRendering.TextAlignment = TextAlignment.Center;
                    leftAlign.IsChecked = false;
                    rightAlign.IsChecked = false;
                    justify.IsChecked = false;
                    break;
                case ("rightAlign"):
                    _currentRendering.TextAlignment = TextAlignment.Right;
                    leftAlign.IsChecked = false;
                    centerAlign.IsChecked = false;
                    justify.IsChecked = false;
                    break;
                case ("justify"):
                    _currentRendering.TextAlignment = TextAlignment.Justify;
                    leftAlign.IsChecked = false;
                    centerAlign.IsChecked = false;
                    rightAlign.IsChecked = false;
                    break;
            }
            UpdateFormattedText(_pixelsPerDip);
        }

        // Some common font sizes.
        public static double[] CommonFontSizes = new double[] {
         3.0d, 4.0d, 5.0d, 6.0d, 6.5d, 7.0d, 7.5d, 8.0d, 8.5d, 9.0d,
         9.5d, 10.0d, 10.5d, 11.0d, 11.5d, 12.0d, 12.5d, 13.0d, 13.5d, 14.0d,
         15.0d, 16.0d, 17.0d, 18.0d, 19.0d, 20.0d, 22.0d, 24.0d, 26.0d, 28.0d,
         30.0d, 32.0d, 34.0d, 36.0d, 38.0d, 40.0d, 44.0d, 48.0d, 52.0d, 56.0d,
         60.0d, 64.0d, 68.0d, 72.0d, 76.0d, 80.0d, 88.0d, 96.0d, 104.0d, 112.0d,
         120.0d, 128.0d, 136.0d, 144.0d, 152.0d, 160.0d,  176.0d,  192.0d,  208.0d,
         224.0d, 240.0d, 256.0d, 272.0d, 288.0d, 304.0d, 320.0d, 352.0d, 384.0d,
         416.0d, 448.0d, 480.0d, 512.0d, 544.0d, 576.0d, 608.0d, 640.0d};

        private FontRendering _currentRendering;
        private CustomTextSource _textStore;
        private bool _UILoaded = false;
        private double _pixelsPerDip = 1.0;
    }
}
