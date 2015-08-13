// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FontDialog
{
    /// <summary>
    ///     Interaction logic for FontChooser.xaml
    /// </summary>
    public partial class FontChooser : Window
    {
        #region Dependency property tables

        // Array of all font chooser dependency properties
        private static readonly double[] CommonlyUsedFontSizes =
        {
            3.0, 4.0, 5.0, 6.0, 6.5,
            7.0, 7.5, 8.0, 8.5, 9.0,
            9.5, 10.0, 10.5, 11.0, 11.5,
            12.0, 12.5, 13.0, 13.5, 14.0,
            15.0, 16.0, 17.0, 18.0, 19.0,
            20.0, 22.0, 24.0, 26.0, 28.0, 30.0, 32.0, 34.0, 36.0, 38.0,
            40.0, 44.0, 48.0, 52.0, 56.0, 60.0, 64.0, 68.0, 72.0, 76.0,
            80.0, 88.0, 96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
        };

        public static readonly DependencyProperty StandardLigaturesProperty =
            RegisterTypographicProperty(Typography.StandardLigaturesProperty);

        public static readonly DependencyProperty ContextualLigaturesProperty =
            RegisterTypographicProperty(Typography.ContextualLigaturesProperty);

        public static readonly DependencyProperty DiscretionaryLigaturesProperty =
            RegisterTypographicProperty(Typography.DiscretionaryLigaturesProperty);

        public static readonly DependencyProperty HistoricalLigaturesProperty =
            RegisterTypographicProperty(Typography.HistoricalLigaturesProperty);

        public static readonly DependencyProperty ContextualAlternatesProperty =
            RegisterTypographicProperty(Typography.ContextualAlternatesProperty);

        public static readonly DependencyProperty HistoricalFormsProperty =
            RegisterTypographicProperty(Typography.HistoricalFormsProperty);

        public static readonly DependencyProperty KerningProperty =
            RegisterTypographicProperty(Typography.KerningProperty);

        public static readonly DependencyProperty CapitalSpacingProperty =
            RegisterTypographicProperty(Typography.CapitalSpacingProperty);

        public static readonly DependencyProperty CaseSensitiveFormsProperty =
            RegisterTypographicProperty(Typography.CaseSensitiveFormsProperty);

        public static readonly DependencyProperty StylisticSet1Property =
            RegisterTypographicProperty(Typography.StylisticSet1Property);

        public static readonly DependencyProperty StylisticSet2Property =
            RegisterTypographicProperty(Typography.StylisticSet2Property);

        public static readonly DependencyProperty StylisticSet3Property =
            RegisterTypographicProperty(Typography.StylisticSet3Property);

        public static readonly DependencyProperty StylisticSet4Property =
            RegisterTypographicProperty(Typography.StylisticSet4Property);

        public static readonly DependencyProperty StylisticSet5Property =
            RegisterTypographicProperty(Typography.StylisticSet5Property);

        public static readonly DependencyProperty StylisticSet6Property =
            RegisterTypographicProperty(Typography.StylisticSet6Property);

        public static readonly DependencyProperty StylisticSet7Property =
            RegisterTypographicProperty(Typography.StylisticSet7Property);

        public static readonly DependencyProperty StylisticSet8Property =
            RegisterTypographicProperty(Typography.StylisticSet8Property);

        public static readonly DependencyProperty StylisticSet9Property =
            RegisterTypographicProperty(Typography.StylisticSet9Property);

        public static readonly DependencyProperty StylisticSet10Property =
            RegisterTypographicProperty(Typography.StylisticSet10Property);

        public static readonly DependencyProperty StylisticSet11Property =
            RegisterTypographicProperty(Typography.StylisticSet11Property);

        public static readonly DependencyProperty StylisticSet12Property =
            RegisterTypographicProperty(Typography.StylisticSet12Property);

        public static readonly DependencyProperty StylisticSet13Property =
            RegisterTypographicProperty(Typography.StylisticSet13Property);

        public static readonly DependencyProperty StylisticSet14Property =
            RegisterTypographicProperty(Typography.StylisticSet14Property);

        public static readonly DependencyProperty StylisticSet15Property =
            RegisterTypographicProperty(Typography.StylisticSet15Property);

        public static readonly DependencyProperty StylisticSet16Property =
            RegisterTypographicProperty(Typography.StylisticSet16Property);

        public static readonly DependencyProperty StylisticSet17Property =
            RegisterTypographicProperty(Typography.StylisticSet17Property);

        public static readonly DependencyProperty StylisticSet18Property =
            RegisterTypographicProperty(Typography.StylisticSet18Property);

        public static readonly DependencyProperty StylisticSet19Property =
            RegisterTypographicProperty(Typography.StylisticSet19Property);

        public static readonly DependencyProperty StylisticSet20Property =
            RegisterTypographicProperty(Typography.StylisticSet20Property);

        public static readonly DependencyProperty SlashedZeroProperty =
            RegisterTypographicProperty(Typography.SlashedZeroProperty, "Digits");

        public static readonly DependencyProperty MathematicalGreekProperty =
            RegisterTypographicProperty(Typography.MathematicalGreekProperty);

        public static readonly DependencyProperty EastAsianExpertFormsProperty =
            RegisterTypographicProperty(Typography.EastAsianExpertFormsProperty);

        public static readonly DependencyProperty FractionProperty =
            RegisterTypographicProperty(Typography.FractionProperty,
                "OneHalf");

        public static readonly DependencyProperty VariantsProperty =
            RegisterTypographicProperty(Typography.VariantsProperty);

        public static readonly DependencyProperty CapitalsProperty =
            RegisterTypographicProperty(Typography.CapitalsProperty);

        public static readonly DependencyProperty NumeralStyleProperty =
            RegisterTypographicProperty(Typography.NumeralStyleProperty, "Digits");

        public static readonly DependencyProperty NumeralAlignmentProperty =
            RegisterTypographicProperty(Typography.NumeralAlignmentProperty, "Digits");

        public static readonly DependencyProperty EastAsianWidthsProperty =
            RegisterTypographicProperty(Typography.EastAsianWidthsProperty);

        public static readonly DependencyProperty EastAsianLanguageProperty =
            RegisterTypographicProperty(Typography.EastAsianLanguageProperty);

        public static readonly DependencyProperty AnnotationAlternatesProperty =
            RegisterTypographicProperty(Typography.AnnotationAlternatesProperty);

        public static readonly DependencyProperty StandardSwashesProperty =
            RegisterTypographicProperty(Typography.StandardSwashesProperty);

        public static readonly DependencyProperty ContextualSwashesProperty =
            RegisterTypographicProperty(Typography.ContextualSwashesProperty);

        public static readonly DependencyProperty StylisticAlternatesProperty =
            RegisterTypographicProperty(Typography.StylisticAlternatesProperty);

        public static readonly DependencyProperty SelectedFontFamilyProperty = RegisterFontProperty(
            "SelectedFontFamily",
            TextBlock.FontFamilyProperty,
            SelectedFontFamilyChangedCallback
            );

        public static readonly DependencyProperty SelectedFontWeightProperty = RegisterFontProperty(
            "SelectedFontWeight",
            TextBlock.FontWeightProperty,
            SelectedTypefaceChangedCallback
            );

        public static readonly DependencyProperty SelectedFontStyleProperty = RegisterFontProperty(
            "SelectedFontStyle",
            TextBlock.FontStyleProperty,
            SelectedTypefaceChangedCallback
            );

        public static readonly DependencyProperty SelectedFontStretchProperty = RegisterFontProperty(
            "SelectedFontStretch",
            TextBlock.FontStretchProperty,
            SelectedTypefaceChangedCallback
            );

        public static readonly DependencyProperty SelectedFontSizeProperty = RegisterFontProperty(
            "SelectedFontSize",
            TextBlock.FontSizeProperty,
            SelectedFontSizeChangedCallback
            );

        public static readonly DependencyProperty SelectedTextDecorationsProperty = RegisterFontProperty(
            "SelectedTextDecorations",
            TextBlock.TextDecorationsProperty,
            SelectedTextDecorationsChangedCallback
            );

        private static readonly DependencyProperty[] ChooserProperties =
        {
            // typography properties
            StandardLigaturesProperty,
            ContextualLigaturesProperty,
            DiscretionaryLigaturesProperty,
            HistoricalLigaturesProperty,
            ContextualAlternatesProperty,
            HistoricalFormsProperty,
            KerningProperty,
            CapitalSpacingProperty,
            CaseSensitiveFormsProperty,
            StylisticSet1Property,
            StylisticSet2Property,
            StylisticSet3Property,
            StylisticSet4Property,
            StylisticSet5Property,
            StylisticSet6Property,
            StylisticSet7Property,
            StylisticSet8Property,
            StylisticSet9Property,
            StylisticSet10Property,
            StylisticSet11Property,
            StylisticSet12Property,
            StylisticSet13Property,
            StylisticSet14Property,
            StylisticSet15Property,
            StylisticSet16Property,
            StylisticSet17Property,
            StylisticSet18Property,
            StylisticSet19Property,
            StylisticSet20Property,
            SlashedZeroProperty,
            MathematicalGreekProperty,
            EastAsianExpertFormsProperty,
            FractionProperty,
            VariantsProperty,
            CapitalsProperty,
            NumeralStyleProperty,
            NumeralAlignmentProperty,
            EastAsianWidthsProperty,
            EastAsianLanguageProperty,
            AnnotationAlternatesProperty,
            StandardSwashesProperty,
            ContextualSwashesProperty,
            StylisticAlternatesProperty,

            // other properties
            SelectedFontFamilyProperty,
            SelectedFontWeightProperty,
            SelectedFontStyleProperty,
            SelectedFontStretchProperty,
            SelectedFontSizeProperty,
            SelectedTextDecorationsProperty
        };

        #endregion

        #region Private fields and types

        private ICollection<FontFamily> _familyCollection; // see FamilyCollection property
        private string _defaultSampleText;
        private string _previewSampleText;
        private string _pointsText;

        private bool _updatePending; // indicates a call to OnUpdate is scheduled
        private bool _familyListValid; // indicates the list of font families is valid
        private bool _typefaceListValid; // indicates the list of typefaces is valid
        private bool _typefaceListSelectionValid; // indicates the current selection in the typeface list is valid
        private bool _previewValid; // indicates the preview control is valid
        private Dictionary<TabItem, TabState> _tabDictionary; // state and logic for each tab
        private DependencyProperty _currentFeature;
        private TypographyFeaturePage _currentFeaturePage;


        // Specialized metadata object for font chooser dependency properties
        private class FontPropertyMetadata : FrameworkPropertyMetadata
        {
            public readonly DependencyProperty TargetProperty;

            public FontPropertyMetadata(
                object defaultValue,
                PropertyChangedCallback changeCallback,
                DependencyProperty targetProperty
                )
                : base(defaultValue, changeCallback)
            {
                TargetProperty = targetProperty;
            }
        }

        // Specialized metadata object for typographic font chooser properties
        private class TypographicPropertyMetadata : FontPropertyMetadata
        {
            private static readonly PropertyChangedCallback Callback = TypographicPropertyChangedCallback;
            public readonly TypographyFeaturePage FeaturePage;
            public readonly string SampleTextTag;

            public TypographicPropertyMetadata(object defaultValue, DependencyProperty targetProperty,
                TypographyFeaturePage featurePage, string sampleTextTag)
                : base(defaultValue, Callback, targetProperty)
            {
                FeaturePage = featurePage;
                SampleTextTag = sampleTextTag;
            }
        }

        // Object used to initialize the right-hand side of the typographic properties tab
        private class TypographyFeaturePage
        {
            public static readonly TypographyFeaturePage BooleanFeaturePage = new TypographyFeaturePage(
                new[]
                {
                    new Item("Disabled", false),
                    new Item("Enabled", true)
                }
                );

            public static readonly TypographyFeaturePage IntegerFeaturePage = new TypographyFeaturePage(
                new[]
                {
                    new Item("_0", 0),
                    new Item("_1", 1),
                    new Item("_2", 2),
                    new Item("_3", 3),
                    new Item("_4", 4),
                    new Item("_5", 5),
                    new Item("_6", 6),
                    new Item("_7", 7),
                    new Item("_8", 8),
                    new Item("_9", 9)
                }
                );

            public readonly Item[] Items;

            private TypographyFeaturePage(Item[] items)
            {
                Items = items;
            }

            public TypographyFeaturePage(Type enumType)
            {
                var names = Enum.GetNames(enumType);
                var values = Enum.GetValues(enumType);

                Items = new Item[names.Length];

                for (var i = 0; i < names.Length; ++i)
                {
                    Items[i] = new Item(names[i], values.GetValue(i));
                }
            }

            public struct Item
            {
                public readonly string Tag;
                public readonly object Value;

                public Item(string tag, object value)
                {
                    Tag = tag;
                    Value = value;
                }
            }
        }

        private delegate void UpdateCallback();

        // Encapsulates the state and initialization logic of a tab control item.
        private class TabState
        {
            public readonly UpdateCallback InitializeTab;
            public bool IsValid;

            public TabState(UpdateCallback initMethod)
            {
                InitializeTab = initMethod;
            }
        }

        #endregion

        #region Construction and initialization

        public FontChooser()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _previewSampleText = _defaultSampleText = previewTextBox.Text;
            _pointsText = typefaceNameRun.Text;

            // Hook up events for the font family list and associated text box.
            fontFamilyTextBox.SelectionChanged += fontFamilyTextBox_SelectionChanged;
            fontFamilyTextBox.TextChanged += fontFamilyTextBox_TextChanged;
            fontFamilyTextBox.PreviewKeyDown += fontFamilyTextBox_PreviewKeyDown;
            fontFamilyList.SelectionChanged += fontFamilyList_SelectionChanged;

            // Hook up events for the typeface list.
            typefaceList.SelectionChanged += typefaceList_SelectionChanged;

            // Hook up events for the font size list and associated text box.
            sizeTextBox.TextChanged += sizeTextBox_TextChanged;
            sizeTextBox.PreviewKeyDown += sizeTextBox_PreviewKeyDown;
            sizeList.SelectionChanged += sizeList_SelectionChanged;

            // Hook up events for text decoration check boxes.
            RoutedEventHandler textDecorationEventHandler = TextDecorationCheckStateChanged;
            underlineCheckBox.Checked += textDecorationEventHandler;
            underlineCheckBox.Unchecked += textDecorationEventHandler;
            baselineCheckBox.Checked += textDecorationEventHandler;
            baselineCheckBox.Unchecked += textDecorationEventHandler;
            strikethroughCheckBox.Checked += textDecorationEventHandler;
            strikethroughCheckBox.Unchecked += textDecorationEventHandler;
            overlineCheckBox.Checked += textDecorationEventHandler;
            overlineCheckBox.Unchecked += textDecorationEventHandler;

            // Initialize the dictionary that maps tab control items to handler objects.
            _tabDictionary = new Dictionary<TabItem, TabState>(tabControl.Items.Count)
            {
                {samplesTab, new TabState(InitializeSamplesTab)},
                {typographyTab, new TabState(InitializeTypographyTab)},
                {descriptiveTextTab, new TabState(InitializeDescriptiveTextTab)}
            };

            // Hook up events for the tab control.
            tabControl.SelectionChanged += tabControl_SelectionChanged;

            // Initialize the list of font sizes and select the nearest size.
            foreach (var value in CommonlyUsedFontSizes)
            {
                sizeList.Items.Add(new FontSizeListItem(value));
            }
            OnSelectedFontSizeChanged(SelectedFontSize);

            // Initialize the font family list and the current family.
            if (!_familyListValid)
            {
                InitializeFontFamilyList();
                _familyListValid = true;
                OnSelectedFontFamilyChanged(SelectedFontFamily);
            }

            // Schedule background updates.
            ScheduleUpdate();
        }

        #endregion

        #region Event handlers

        private void OnOkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private int _fontFamilyTextBoxSelectionStart;

        private void fontFamilyTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            _fontFamilyTextBoxSelectionStart = fontFamilyTextBox.SelectionStart;
        }

        private void fontFamilyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = fontFamilyTextBox.Text;

            // Update the current list item.
            if (SelectFontFamilyListItem(text) == null)
            {
                // The text does not exactly match a family name so consider applying auto-complete behavior.
                // However, only do so if the following conditions are met:
                //   (1)  The user is typing more text rather than deleting (i.e., the new text length is
                //        greater than the most recent selection start index), and
                //   (2)  The caret is at the end of the text box.
                if (text.Length > _fontFamilyTextBoxSelectionStart
                    && fontFamilyTextBox.SelectionStart == text.Length)
                {
                    // Get the current list item, which should be the nearest match for the text.
                    var item = fontFamilyList.Items.CurrentItem as FontFamilyListItem;
                    if (item != null)
                    {
                        // Does the text box text match the beginning of the family name?
                        var familyDisplayName = item.ToString();
                        if (
                            string.Compare(text, 0, familyDisplayName, 0, text.Length, true, CultureInfo.CurrentCulture) ==
                            0)
                        {
                            // Set the text box text to the complete family name and select the part not typed in.
                            fontFamilyTextBox.Text = familyDisplayName;
                            fontFamilyTextBox.SelectionStart = text.Length;
                            fontFamilyTextBox.SelectionLength = familyDisplayName.Length - text.Length;
                        }
                    }
                }
            }
        }

        private void sizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double sizeInPoints;
            if (double.TryParse(sizeTextBox.Text, out sizeInPoints))
            {
                var sizeInPixels = FontSizeListItem.PointsToPixels(sizeInPoints);
                if (!FontSizeListItem.FuzzyEqual(sizeInPixels, SelectedFontSize))
                {
                    SelectedFontSize = sizeInPixels;
                }
            }
        }

        private void fontFamilyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnComboBoxPreviewKeyDown(fontFamilyTextBox, fontFamilyList, e);
        }

        private void sizeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnComboBoxPreviewKeyDown(sizeTextBox, sizeList, e);
        }

        private void fontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = fontFamilyList.SelectedItem as FontFamilyListItem;
            if (item != null)
            {
                SelectedFontFamily = item.FontFamily;
            }
        }

        private void sizeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sizeList.SelectedItem as FontSizeListItem;
            if (item != null)
            {
                SelectedFontSize = item.SizeInPixels;
            }
        }

        private void typefaceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = typefaceList.SelectedItem as TypefaceListItem;
            if (item != null)
            {
                SelectedFontWeight = item.FontWeight;
                SelectedFontStyle = item.FontStyle;
                SelectedFontStretch = item.FontStretch;
            }
        }

        private void TextDecorationCheckStateChanged(object sender, RoutedEventArgs e)
        {
            var textDecorations = new TextDecorationCollection();

            if (underlineCheckBox.IsChecked.Value)
            {
                textDecorations.Add(TextDecorations.Underline[0]);
            }
            if (baselineCheckBox.IsChecked.Value)
            {
                textDecorations.Add(TextDecorations.Baseline[0]);
            }
            if (strikethroughCheckBox.IsChecked.Value)
            {
                textDecorations.Add(TextDecorations.Strikethrough[0]);
            }
            if (overlineCheckBox.IsChecked.Value)
            {
                textDecorations.Add(TextDecorations.OverLine[0]);
            }

            textDecorations.Freeze();
            SelectedTextDecorations = textDecorations;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = CurrentTabState;
            if (tab != null && !tab.IsValid)
            {
                tab.InitializeTab();
                tab.IsValid = true;
            }
        }

        private void featureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeTypographyTab();
        }

        #endregion

        #region Public properties and methods

        /// <summary>
        ///     Collection of font families to display in the font family list. By default this is Fonts.SystemFontFamilies,
        ///     but a client could set this to another collection returned by Fonts.GetFontFamilies, e.g., a collection of
        ///     application-defined fonts.
        /// </summary>
        public ICollection<FontFamily> FontFamilyCollection
        {
            get { return _familyCollection ?? Fonts.SystemFontFamilies; }

            set
            {
                if (value != _familyCollection)
                {
                    _familyCollection = value;
                    InvalidateFontFamilyList();
                }
            }
        }

        /// <summary>
        ///     Sets the font chooser selection properties to match the properites of the specified object.
        /// </summary>
        public void SetPropertiesFromObject(DependencyObject obj)
        {
            foreach (var property in ChooserProperties)
            {
                var metadata = property.GetMetadata(typeof (FontChooser)) as FontPropertyMetadata;
                if (metadata != null)
                {
                    SetValue(property, obj.GetValue(metadata.TargetProperty));
                }
            }
        }

        /// <summary>
        ///     Sets the properites of the specified object to match the font chooser selection properties.
        /// </summary>
        public void ApplyPropertiesToObject(DependencyObject obj)
        {
            foreach (var property in ChooserProperties)
            {
                var metadata = property.GetMetadata(typeof (FontChooser)) as FontPropertyMetadata;
                if (metadata != null)
                {
                    obj.SetValue(metadata.TargetProperty, GetValue(property));
                }
            }
        }

        private void ApplyPropertiesToObjectExcept(DependencyObject obj, DependencyProperty except)
        {
            foreach (var property in ChooserProperties)
            {
                if (property != except)
                {
                    var metadata = property.GetMetadata(typeof (FontChooser)) as FontPropertyMetadata;
                    if (metadata != null)
                    {
                        obj.SetValue(metadata.TargetProperty, GetValue(property));
                    }
                }
            }
        }

        /// <summary>
        ///     Sample text used in the preview box and family and typeface samples tab.
        /// </summary>
        public string PreviewSampleText
        {
            get { return _previewSampleText; }

            set
            {
                var newValue = string.IsNullOrEmpty(value) ? _defaultSampleText : value;
                if (newValue != _previewSampleText)
                {
                    _previewSampleText = newValue;

                    // Update the preview text box.
                    previewTextBox.Text = newValue;

                    // The preview sample text is also used in the family and typeface samples tab.
                    InvalidateTab(samplesTab);
                }
            }
        }

        #endregion

        #region Dependency properties for typographic features

        public bool StandardLigatures
        {
            get { return (bool) GetValue(StandardLigaturesProperty); }
            set { SetValue(StandardLigaturesProperty, value); }
        }


        public bool ContextualLigatures
        {
            get { return (bool) GetValue(ContextualLigaturesProperty); }
            set { SetValue(ContextualLigaturesProperty, value); }
        }


        public bool DiscretionaryLigatures
        {
            get { return (bool) GetValue(DiscretionaryLigaturesProperty); }
            set { SetValue(DiscretionaryLigaturesProperty, value); }
        }


        public bool HistoricalLigatures
        {
            get { return (bool) GetValue(HistoricalLigaturesProperty); }
            set { SetValue(HistoricalLigaturesProperty, value); }
        }


        public bool ContextualAlternates
        {
            get { return (bool) GetValue(ContextualAlternatesProperty); }
            set { SetValue(ContextualAlternatesProperty, value); }
        }


        public bool HistoricalForms
        {
            get { return (bool) GetValue(HistoricalFormsProperty); }
            set { SetValue(HistoricalFormsProperty, value); }
        }


        public bool Kerning
        {
            get { return (bool) GetValue(KerningProperty); }
            set { SetValue(KerningProperty, value); }
        }


        public bool CapitalSpacing
        {
            get { return (bool) GetValue(CapitalSpacingProperty); }
            set { SetValue(CapitalSpacingProperty, value); }
        }


        public bool CaseSensitiveForms
        {
            get { return (bool) GetValue(CaseSensitiveFormsProperty); }
            set { SetValue(CaseSensitiveFormsProperty, value); }
        }


        public bool StylisticSet1
        {
            get { return (bool) GetValue(StylisticSet1Property); }
            set { SetValue(StylisticSet1Property, value); }
        }


        public bool StylisticSet2
        {
            get { return (bool) GetValue(StylisticSet2Property); }
            set { SetValue(StylisticSet2Property, value); }
        }


        public bool StylisticSet3
        {
            get { return (bool) GetValue(StylisticSet3Property); }
            set { SetValue(StylisticSet3Property, value); }
        }


        public bool StylisticSet4
        {
            get { return (bool) GetValue(StylisticSet4Property); }
            set { SetValue(StylisticSet4Property, value); }
        }


        public bool StylisticSet5
        {
            get { return (bool) GetValue(StylisticSet5Property); }
            set { SetValue(StylisticSet5Property, value); }
        }


        public bool StylisticSet6
        {
            get { return (bool) GetValue(StylisticSet6Property); }
            set { SetValue(StylisticSet6Property, value); }
        }


        public bool StylisticSet7
        {
            get { return (bool) GetValue(StylisticSet7Property); }
            set { SetValue(StylisticSet7Property, value); }
        }


        public bool StylisticSet8
        {
            get { return (bool) GetValue(StylisticSet8Property); }
            set { SetValue(StylisticSet8Property, value); }
        }


        public bool StylisticSet9
        {
            get { return (bool) GetValue(StylisticSet9Property); }
            set { SetValue(StylisticSet9Property, value); }
        }


        public bool StylisticSet10
        {
            get { return (bool) GetValue(StylisticSet10Property); }
            set { SetValue(StylisticSet10Property, value); }
        }


        public bool StylisticSet11
        {
            get { return (bool) GetValue(StylisticSet11Property); }
            set { SetValue(StylisticSet11Property, value); }
        }


        public bool StylisticSet12
        {
            get { return (bool) GetValue(StylisticSet12Property); }
            set { SetValue(StylisticSet12Property, value); }
        }


        public bool StylisticSet13
        {
            get { return (bool) GetValue(StylisticSet13Property); }
            set { SetValue(StylisticSet13Property, value); }
        }


        public bool StylisticSet14
        {
            get { return (bool) GetValue(StylisticSet14Property); }
            set { SetValue(StylisticSet14Property, value); }
        }


        public bool StylisticSet15
        {
            get { return (bool) GetValue(StylisticSet15Property); }
            set { SetValue(StylisticSet15Property, value); }
        }


        public bool StylisticSet16
        {
            get { return (bool) GetValue(StylisticSet16Property); }
            set { SetValue(StylisticSet16Property, value); }
        }


        public bool StylisticSet17
        {
            get { return (bool) GetValue(StylisticSet17Property); }
            set { SetValue(StylisticSet17Property, value); }
        }


        public bool StylisticSet18
        {
            get { return (bool) GetValue(StylisticSet18Property); }
            set { SetValue(StylisticSet18Property, value); }
        }


        public bool StylisticSet19
        {
            get { return (bool) GetValue(StylisticSet19Property); }
            set { SetValue(StylisticSet19Property, value); }
        }


        public bool StylisticSet20
        {
            get { return (bool) GetValue(StylisticSet20Property); }
            set { SetValue(StylisticSet20Property, value); }
        }


        public bool SlashedZero
        {
            get { return (bool) GetValue(SlashedZeroProperty); }
            set { SetValue(SlashedZeroProperty, value); }
        }


        public bool MathematicalGreek
        {
            get { return (bool) GetValue(MathematicalGreekProperty); }
            set { SetValue(MathematicalGreekProperty, value); }
        }


        public bool EastAsianExpertForms
        {
            get { return (bool) GetValue(EastAsianExpertFormsProperty); }
            set { SetValue(EastAsianExpertFormsProperty, value); }
        }


        public FontFraction Fraction
        {
            get { return (FontFraction) GetValue(FractionProperty); }
            set { SetValue(FractionProperty, value); }
        }


        public FontVariants Variants
        {
            get { return (FontVariants) GetValue(VariantsProperty); }
            set { SetValue(VariantsProperty, value); }
        }


        public FontCapitals Capitals
        {
            get { return (FontCapitals) GetValue(CapitalsProperty); }
            set { SetValue(CapitalsProperty, value); }
        }


        public FontNumeralStyle NumeralStyle
        {
            get { return (FontNumeralStyle) GetValue(NumeralStyleProperty); }
            set { SetValue(NumeralStyleProperty, value); }
        }


        public FontNumeralAlignment NumeralAlignment
        {
            get { return (FontNumeralAlignment) GetValue(NumeralAlignmentProperty); }
            set { SetValue(NumeralAlignmentProperty, value); }
        }


        public FontEastAsianWidths EastAsianWidths
        {
            get { return (FontEastAsianWidths) GetValue(EastAsianWidthsProperty); }
            set { SetValue(EastAsianWidthsProperty, value); }
        }


        public FontEastAsianLanguage EastAsianLanguage
        {
            get { return (FontEastAsianLanguage) GetValue(EastAsianLanguageProperty); }
            set { SetValue(EastAsianLanguageProperty, value); }
        }


        public int AnnotationAlternates
        {
            get { return (int) GetValue(AnnotationAlternatesProperty); }
            set { SetValue(AnnotationAlternatesProperty, value); }
        }


        public int StandardSwashes
        {
            get { return (int) GetValue(StandardSwashesProperty); }
            set { SetValue(StandardSwashesProperty, value); }
        }


        public int ContextualSwashes
        {
            get { return (int) GetValue(ContextualSwashesProperty); }
            set { SetValue(ContextualSwashesProperty, value); }
        }


        public int StylisticAlternates
        {
            get { return (int) GetValue(StylisticAlternatesProperty); }
            set { SetValue(StylisticAlternatesProperty, value); }
        }

        private static void TypographicPropertyChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var chooser = obj as FontChooser;
            chooser?.InvalidatePreview();
        }

        #endregion

        #region Other dependency properties

        public FontFamily SelectedFontFamily
        {
            get { return GetValue(SelectedFontFamilyProperty) as FontFamily; }
            set { SetValue(SelectedFontFamilyProperty, value); }
        }

        private static void SelectedFontFamilyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooser) obj).OnSelectedFontFamilyChanged(e.NewValue as FontFamily);
        }


        public FontWeight SelectedFontWeight
        {
            get { return (FontWeight) GetValue(SelectedFontWeightProperty); }
            set { SetValue(SelectedFontWeightProperty, value); }
        }


        public FontStyle SelectedFontStyle
        {
            get { return (FontStyle) GetValue(SelectedFontStyleProperty); }
            set { SetValue(SelectedFontStyleProperty, value); }
        }


        public FontStretch SelectedFontStretch
        {
            get { return (FontStretch) GetValue(SelectedFontStretchProperty); }
            set { SetValue(SelectedFontStretchProperty, value); }
        }

        private static void SelectedTypefaceChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooser) obj).InvalidateTypefaceListSelection();
        }


        public double SelectedFontSize
        {
            get { return (double) GetValue(SelectedFontSizeProperty); }
            set { SetValue(SelectedFontSizeProperty, value); }
        }

        private static void SelectedFontSizeChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FontChooser) obj).OnSelectedFontSizeChanged((double) (e.NewValue));
        }


        public TextDecorationCollection SelectedTextDecorations
        {
            get { return GetValue(SelectedTextDecorationsProperty) as TextDecorationCollection; }
            set { SetValue(SelectedTextDecorationsProperty, value); }
        }

        private static void SelectedTextDecorationsChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var chooser = (FontChooser) obj;
            chooser.OnTextDecorationsChanged();
        }

        #endregion

        #region Dependency property helper functions

        // Helper function for registering typographic dependency properties with property-specific sample text.
        private static DependencyProperty RegisterTypographicProperty(DependencyProperty targetProperty,
            string sampleTextTag = null)
        {
            var t = targetProperty.PropertyType;

            var featurePage = (t == typeof (bool))
                ? TypographyFeaturePage.BooleanFeaturePage
                : (t == typeof (int))
                    ? TypographyFeaturePage.IntegerFeaturePage
                    : new TypographyFeaturePage(t);

            return DependencyProperty.Register(
                targetProperty.Name,
                t,
                typeof (FontChooser),
                new TypographicPropertyMetadata(
                    targetProperty.DefaultMetadata.DefaultValue,
                    targetProperty,
                    featurePage,
                    sampleTextTag
                    )
                );
        }

        // Helper function for registering typographic dependency properties with default sample text for the type.

        // Helper function for registering font chooser dependency properties other than typographic properties.
        private static DependencyProperty RegisterFontProperty(
            string propertyName,
            DependencyProperty targetProperty,
            PropertyChangedCallback changeCallback
            ) => DependencyProperty.Register(
                propertyName,
                targetProperty.PropertyType,
                typeof(FontChooser),
                new FontPropertyMetadata(
                    targetProperty.DefaultMetadata.DefaultValue,
                    changeCallback,
                    targetProperty
                    )
                );

        #endregion

        #region Property change handlers

        // Handle changes to the SelectedFontFamily property
        private void OnSelectedFontFamilyChanged(FontFamily family)
        {
            // If the family list is not valid do nothing for now. 
            // We'll be called again after the list is initialized.
            if (_familyListValid)
            {
                // Select the family in the list; this will return null if the family is not in the list.
                var item = SelectFontFamilyListItem(family);

                // Set the text box to the family name, if it isn't already.
                var displayName = item?.ToString() ?? FontFamilyListItem.GetDisplayName(family);
                if (string.Compare(fontFamilyTextBox.Text, displayName, true, CultureInfo.CurrentCulture) != 0)
                {
                    fontFamilyTextBox.Text = displayName;
                }

                // The typeface list is no longer valid; update it in the background to improve responsiveness.
                InvalidateTypefaceList();
            }
        }

        // Handle changes to the SelectedFontSize property
        private void OnSelectedFontSizeChanged(double sizeInPixels)
        {
            // Select the list item, if the size is in the list.
            var sizeInPoints = FontSizeListItem.PixelsToPoints(sizeInPixels);
            if (!SelectListItem(sizeList, sizeInPoints))
            {
                sizeList.SelectedIndex = -1;
            }

            // Set the text box contents if it doesn't already match the current size.
            double textBoxValue;
            if (!double.TryParse(sizeTextBox.Text, out textBoxValue) ||
                !FontSizeListItem.FuzzyEqual(textBoxValue, sizeInPoints))
            {
                sizeTextBox.Text = sizeInPoints.ToString(CultureInfo.InvariantCulture);
            }

            // Schedule background updates.
            InvalidateTab(typographyTab);
            InvalidatePreview();
        }

        // Handle changes to any of the text decoration properties.
        private void OnTextDecorationsChanged()
        {
            var underline = false;
            var baseline = false;
            var strikethrough = false;
            var overline = false;

            var textDecorations = SelectedTextDecorations;
            if (textDecorations != null)
            {
                foreach (var td in textDecorations)
                {
                    switch (td.Location)
                    {
                        case TextDecorationLocation.Underline:
                            underline = true;
                            break;
                        case TextDecorationLocation.Baseline:
                            baseline = true;
                            break;
                        case TextDecorationLocation.Strikethrough:
                            strikethrough = true;
                            break;
                        case TextDecorationLocation.OverLine:
                            overline = true;
                            break;
                    }
                }
            }

            underlineCheckBox.IsChecked = underline;
            baselineCheckBox.IsChecked = baseline;
            strikethroughCheckBox.IsChecked = strikethrough;
            overlineCheckBox.IsChecked = overline;

            // Schedule background updates.
            InvalidateTab(typographyTab);
            InvalidatePreview();
        }

        #endregion

        #region Background update logic

        // Schedule background initialization of the font famiy list.
        private void InvalidateFontFamilyList()
        {
            if (_familyListValid)
            {
                InvalidateTypefaceList();

                fontFamilyList.Items.Clear();
                fontFamilyTextBox.Clear();
                _familyListValid = false;

                ScheduleUpdate();
            }
        }

        // Schedule background initialization of the typeface list.
        private void InvalidateTypefaceList()
        {
            if (_typefaceListValid)
            {
                typefaceList.Items.Clear();
                _typefaceListValid = false;

                ScheduleUpdate();
            }
        }

        // Schedule background selection of the current typeface list item.
        private void InvalidateTypefaceListSelection()
        {
            if (_typefaceListSelectionValid)
            {
                _typefaceListSelectionValid = false;
                ScheduleUpdate();
            }
        }

        // Mark a specific tab as invalid and schedule background initialization if necessary.
        private void InvalidateTab(TabItem tab)
        {
            TabState tabState;
            if (_tabDictionary.TryGetValue(tab, out tabState))
            {
                if (tabState.IsValid)
                {
                    tabState.IsValid = false;

                    if (tabControl.SelectedItem == tab)
                    {
                        ScheduleUpdate();
                    }
                }
            }
        }

        // Mark all the tabs as invalid and schedule background initialization of the current tab.
        private void InvalidateTabs()
        {
            foreach (var item in _tabDictionary)
            {
                item.Value.IsValid = false;
            }

            ScheduleUpdate();
        }

        // Schedule background initialization of the preview control.
        private void InvalidatePreview()
        {
            if (_previewValid)
            {
                _previewValid = false;
                ScheduleUpdate();
            }
        }

        // Schedule background initialization.
        private void ScheduleUpdate()
        {
            if (!_updatePending)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new UpdateCallback(OnUpdate));
                _updatePending = true;
            }
        }

        // Dispatcher callback that performs background initialization.
        private void OnUpdate()
        {
            _updatePending = false;

            if (!_familyListValid)
            {
                // Initialize the font family list.
                InitializeFontFamilyList();
                _familyListValid = true;
                OnSelectedFontFamilyChanged(SelectedFontFamily);

                // Defer any other initialization until later.
                ScheduleUpdate();
            }
            else if (!_typefaceListValid)
            {
                // Initialize the typeface list.
                InitializeTypefaceList();
                _typefaceListValid = true;

                // Select the current typeface in the list.
                InitializeTypefaceListSelection();
                _typefaceListSelectionValid = true;

                // Defer any other initialization until later.
                ScheduleUpdate();
            }
            else if (!_typefaceListSelectionValid)
            {
                // Select the current typeface in the list.
                InitializeTypefaceListSelection();
                _typefaceListSelectionValid = true;

                // Defer any other initialization until later.
                ScheduleUpdate();
            }
            else
            {
                // Perform any remaining initialization.
                var tab = CurrentTabState;
                if (tab != null && !tab.IsValid)
                {
                    // Initialize the current tab.
                    tab.InitializeTab();
                    tab.IsValid = true;
                }
                if (!_previewValid)
                {
                    // Initialize the preview control.
                    InitializePreview();
                    _previewValid = true;
                }
            }
        }

        #endregion

        #region Content initialization

        private void InitializeFontFamilyList()
        {
            var familyCollection = FontFamilyCollection;
            if (familyCollection != null)
            {
                var items = new FontFamilyListItem[familyCollection.Count];

                var i = 0;

                foreach (var family in familyCollection)
                {
                    items[i++] = new FontFamilyListItem(family);
                }

                Array.Sort(items);

                foreach (var item in items)
                {
                    fontFamilyList.Items.Add(item);
                }
            }
        }

        private void InitializeTypefaceList()
        {
            var family = SelectedFontFamily;
            if (family != null)
            {
                var faceCollection = family.GetTypefaces();

                var items = new TypefaceListItem[faceCollection.Count];

                var i = 0;

                foreach (var face in faceCollection)
                {
                    items[i++] = new TypefaceListItem(face);
                }

                Array.Sort(items);

                foreach (var item in items)
                {
                    typefaceList.Items.Add(item);
                }
            }
        }

        private void InitializeTypefaceListSelection()
        {
            // If the typeface list is not valid, do nothing for now.
            // We'll be called again after the list is initialized.
            if (_typefaceListValid)
            {
                var typeface = new Typeface(SelectedFontFamily, SelectedFontStyle, SelectedFontWeight,
                    SelectedFontStretch);

                // Select the typeface in the list.
                SelectTypefaceListItem(typeface);

                // Schedule background updates.
                InvalidateTabs();
                InvalidatePreview();
            }
        }

        private void InitializeFeatureList()
        {
            var items = new TypographicFeatureListItem[ChooserProperties.Length];

            var count = 0;

            foreach (var property in ChooserProperties)
            {
                if (property.GetMetadata(typeof (FontChooser)) is TypographicPropertyMetadata)
                {
                    var displayName = LookupString(property.Name);
                    items[count++] = new TypographicFeatureListItem(displayName, property);
                }
            }

            Array.Sort(items, 0, count);

            for (var i = 0; i < count; ++i)
            {
                featureList.Items.Add(items[i]);
            }
        }

        private static string LookupString(string tag) => Properties.Resources.ResourceManager.GetString(tag, CultureInfo.CurrentUICulture);

        private TabState CurrentTabState
        {
            get
            {
                TabState tab;
                return _tabDictionary.TryGetValue(tabControl.SelectedItem as TabItem, out tab) ? tab : null;
            }
        }

        private void InitializeSamplesTab()
        {
            var selectedFamily = SelectedFontFamily;

            var selectedFace = new Typeface(
                selectedFamily,
                SelectedFontStyle,
                SelectedFontWeight,
                SelectedFontStretch
                );

            fontFamilyNameRun.Text = FontFamilyListItem.GetDisplayName(selectedFamily);
            typefaceNameRun.Text = TypefaceListItem.GetDisplayName(selectedFace);

            // Create FontFamily samples document.
            var doc = new FlowDocument();
            foreach (var face in selectedFamily.GetTypefaces())
            {
                var labelPara = new Paragraph(new Run(TypefaceListItem.GetDisplayName(face)))
                {
                    Margin = new Thickness(0)
                };
                doc.Blocks.Add(labelPara);

                var samplePara = new Paragraph(new Run(_previewSampleText))
                {
                    FontFamily = selectedFamily,
                    FontWeight = face.Weight,
                    FontStyle = face.Style,
                    FontStretch = face.Stretch,
                    FontSize = 16.0,
                    Margin = new Thickness(0, 0, 0, 8)
                };
                doc.Blocks.Add(samplePara);
            }

            fontFamilySamples.Document = doc;

            // Create typeface samples document.
            doc = new FlowDocument();
            foreach (var sizeInPoints in new[] {9.0, 10.0, 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0})
            {
                var labelText = $"{sizeInPoints} {_pointsText}";
                var labelPara = new Paragraph(new Run(labelText)) {Margin = new Thickness(0)};
                doc.Blocks.Add(labelPara);

                var samplePara = new Paragraph(new Run(_previewSampleText))
                {
                    FontFamily = selectedFamily,
                    FontWeight = selectedFace.Weight,
                    FontStyle = selectedFace.Style,
                    FontStretch = selectedFace.Stretch,
                    FontSize = FontSizeListItem.PointsToPixels(sizeInPoints),
                    Margin = new Thickness(0, 0, 0, 8)
                };
                doc.Blocks.Add(samplePara);
            }

            typefaceSamples.Document = doc;
        }

        private void InitializeTypographyTab()
        {
            if (featureList.Items.IsEmpty)
            {
                InitializeFeatureList();
                featureList.SelectedIndex = 0;

                featureList.SelectionChanged += featureList_SelectionChanged;
            }

            DependencyProperty chooserProperty = null;
            TypographyFeaturePage featurePage = null;

            var listItem = featureList.SelectedItem as TypographicFeatureListItem;
            var metadata = listItem?.ChooserProperty.GetMetadata(typeof (FontChooser)) as TypographicPropertyMetadata;
            if (metadata != null)
            {
                chooserProperty = listItem.ChooserProperty;
                featurePage = metadata.FeaturePage;
            }

            InitializeFeaturePage(typographyFeaturePage, chooserProperty, featurePage);
        }

        private void InitializeFeaturePage(Grid grid, DependencyProperty chooserProperty, TypographyFeaturePage page)
        {
            if (page == null)
            {
                grid.Children.Clear();
                grid.RowDefinitions.Clear();
            }
            else
            {
                // Get the property value and metadata.
                var value = GetValue(chooserProperty);
                var metadata = (TypographicPropertyMetadata) chooserProperty.GetMetadata(typeof (FontChooser));

                // Look up the sample text.
                var sampleText = (metadata.SampleTextTag != null)
                    ? LookupString(metadata.SampleTextTag)
                    : _defaultSampleText;

                if (page == _currentFeaturePage)
                {
                    // Update the state of the controls.
                    for (var i = 0; i < page.Items.Length; ++i)
                    {
                        // Check the radio button if it matches the current property value.
                        if (page.Items[i].Value.Equals(value))
                        {
                            var radioButton = (RadioButton) grid.Children[i*2];
                            radioButton.IsChecked = true;
                        }

                        // Apply properties to the sample text block.
                        var sample = (TextBlock) grid.Children[i*2 + 1];
                        sample.Text = sampleText;
                        ApplyPropertiesToObjectExcept(sample, chooserProperty);
                        sample.SetValue(metadata.TargetProperty, page.Items[i].Value);
                    }
                }
                else
                {
                    grid.Children.Clear();
                    grid.RowDefinitions.Clear();

                    // Add row definitions.
                    for (var i = 0; i < page.Items.Length; ++i)
                    {
                        var row = new RowDefinition {Height = GridLength.Auto};
                        grid.RowDefinitions.Add(row);
                    }

                    // Add the controls.
                    for (var i = 0; i < page.Items.Length; ++i)
                    {
                        var tag = page.Items[i].Tag;
                        var radioContent = new TextBlock(new Run(LookupString(tag))) {TextWrapping = TextWrapping.Wrap};

                        // Add the radio button.
                        var radioButton = new RadioButton
                        {
                            Name = tag,
                            Content = radioContent,
                            Margin = new Thickness(5.0, 0.0, 0.0, 0.0),
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        Grid.SetRow(radioButton, i);
                        grid.Children.Add(radioButton);

                        // Check the radio button if it matches the current property value.
                        if (page.Items[i].Value.Equals(value))
                        {
                            radioButton.IsChecked = true;
                        }

                        // Hook up the event.
                        radioButton.Checked += featureRadioButton_Checked;

                        // Add the block with sample text.
                        var sample = new TextBlock(new Run(sampleText))
                        {
                            Margin = new Thickness(5.0, 5.0, 5.0, 0.0),
                            TextWrapping = TextWrapping.WrapWithOverflow
                        };
                        ApplyPropertiesToObjectExcept(sample, chooserProperty);
                        sample.SetValue(metadata.TargetProperty, page.Items[i].Value);
                        Grid.SetRow(sample, i);
                        Grid.SetColumn(sample, 1);
                        grid.Children.Add(sample);
                    }

                    // Add borders between rows.
                    for (var i = 0; i < page.Items.Length; ++i)
                    {
                        var border = new Border
                        {
                            BorderThickness = new Thickness(0.0, 0.0, 0.0, 1.0),
                            BorderBrush = SystemColors.ControlLightBrush
                        };
                        Grid.SetRow(border, i);
                        Grid.SetColumnSpan(border, 2);
                        grid.Children.Add(border);
                    }
                }
            }

            _currentFeature = chooserProperty;
            _currentFeaturePage = page;
        }

        private void featureRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_currentFeature != null && _currentFeaturePage != null)
            {
                var tag = ((RadioButton) sender).Name;

                foreach (var item in _currentFeaturePage.Items)
                {
                    if (item.Tag == tag)
                    {
                        SetValue(_currentFeature, item.Value);
                    }
                }
            }
        }

        private void AddTableRow(TableRowGroup rowGroup, string leftText, string rightText)
        {
            var row = new TableRow();

            row.Cells.Add(new TableCell(new Paragraph(new Run(leftText))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(rightText))));

            rowGroup.Rows.Add(row);
        }

        private void AddTableRow(TableRowGroup rowGroup, string leftText, IDictionary<CultureInfo, string> rightStrings)
        {
            var rightText = NameDictionaryHelper.GetDisplayName(rightStrings);
            AddTableRow(rowGroup, leftText, rightText);
        }

        private void InitializeDescriptiveTextTab()
        {
            var selectedTypeface = new Typeface(
                SelectedFontFamily,
                SelectedFontStyle,
                SelectedFontWeight,
                SelectedFontStretch
                );

            GlyphTypeface glyphTypeface;
            if (selectedTypeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                // Create a table with two columns.
                var table = new Table {CellSpacing = 5};
                var leftColumn = new TableColumn {Width = new GridLength(2.0, GridUnitType.Star)};
                table.Columns.Add(leftColumn);
                var rightColumn = new TableColumn {Width = new GridLength(3.0, GridUnitType.Star)};
                table.Columns.Add(rightColumn);

                var rowGroup = new TableRowGroup();
                AddTableRow(rowGroup, "Family:", glyphTypeface.FamilyNames);
                AddTableRow(rowGroup, "Face:", glyphTypeface.FaceNames);
                AddTableRow(rowGroup, "Description:", glyphTypeface.Descriptions);
                AddTableRow(rowGroup, "Version:", glyphTypeface.VersionStrings);
                AddTableRow(rowGroup, "Copyright:", glyphTypeface.Copyrights);
                AddTableRow(rowGroup, "Trademark:", glyphTypeface.Trademarks);
                AddTableRow(rowGroup, "Manufacturer:", glyphTypeface.ManufacturerNames);
                AddTableRow(rowGroup, "Designer:", glyphTypeface.DesignerNames);
                AddTableRow(rowGroup, "Designer URL:", glyphTypeface.DesignerUrls);
                AddTableRow(rowGroup, "Vendor URL:", glyphTypeface.VendorUrls);
                AddTableRow(rowGroup, "Win32 Family:", glyphTypeface.Win32FamilyNames);
                AddTableRow(rowGroup, "Win32 Face:", glyphTypeface.Win32FaceNames);

                try
                {
                    AddTableRow(rowGroup, "Font File URI:", glyphTypeface.FontUri.ToString());
                }
                catch (SecurityException)
                {
                    // Font file URI is privileged information; just skip it if we don't have access.
                }

                table.RowGroups.Add(rowGroup);

                fontDescriptionBox.Document = new FlowDocument(table);

                fontLicenseBox.Text = NameDictionaryHelper.GetDisplayName(glyphTypeface.LicenseDescriptions);
            }
            else
            {
                fontDescriptionBox.Document = new FlowDocument();
                fontLicenseBox.Text = string.Empty;
            }
        }

        private void InitializePreview()
        {
            ApplyPropertiesToObject(previewTextBox);
        }

        #endregion

        #region List box helpers

        // Update font family list based on selection.
        // Return list item if there's an exact match, or null if not.
        private FontFamilyListItem SelectFontFamilyListItem(string displayName)
        {
            var listItem = fontFamilyList.SelectedItem as FontFamilyListItem;
            if (listItem != null &&
                string.Compare(listItem.ToString(), displayName, true, CultureInfo.CurrentCulture) == 0)
            {
                // Already selected
                return listItem;
            }
            if (SelectListItem(fontFamilyList, displayName))
            {
                // Exact match found
                return fontFamilyList.SelectedItem as FontFamilyListItem;
            }
            // Not in the list
            return null;
        }

        // Update font family list based on selection.
        // Return list item if there's an exact match, or null if not.
        private FontFamilyListItem SelectFontFamilyListItem(FontFamily family)
        {
            var listItem = fontFamilyList.SelectedItem as FontFamilyListItem;
            if (listItem != null && listItem.FontFamily.Equals(family))
            {
                // Already selected
                return listItem;
            }
            if (SelectListItem(fontFamilyList, FontFamilyListItem.GetDisplayName(family)))
            {
                // Exact match found
                return fontFamilyList.SelectedItem as FontFamilyListItem;
            }
            // Not in the list
            return null;
        }

        // Update typeface list based on selection.
        // Return list item if there's an exact match, or null if not.
        private TypefaceListItem SelectTypefaceListItem(Typeface typeface)
        {
            var listItem = typefaceList.SelectedItem as TypefaceListItem;
            if (listItem != null && listItem.Typeface.Equals(typeface))
            {
                // Already selected
                return listItem;
            }
            if (SelectListItem(typefaceList, new TypefaceListItem(typeface)))
            {
                // Exact match found
                return typefaceList.SelectedItem as TypefaceListItem;
            }
            // Not in list
            return null;
        }

        // Update list based on selection.
        // Return true if there's an exact match, or false if not.
        private bool SelectListItem(ListBox list, object value)
        {
            var itemList = list.Items;

            // Perform a binary search for the item.
            var first = 0;
            var limit = itemList.Count;

            while (first < limit)
            {
                var i = first + (limit - first)/2;
                var item = (IComparable) (itemList[i]);
                var comparison = item.CompareTo(value);
                if (comparison < 0)
                {
                    // Value must be after i
                    first = i + 1;
                }
                else if (comparison > 0)
                {
                    // Value must be before i
                    limit = i;
                }
                else
                {
                    // Exact match; select the item.
                    list.SelectedIndex = i;
                    itemList.MoveCurrentToPosition(i);
                    list.ScrollIntoView(itemList[i]);
                    return true;
                }
            }

            // Not an exact match; move current position to the nearest item but don't select it.
            if (itemList.Count > 0)
            {
                var i = Math.Min(first, itemList.Count - 1);
                itemList.MoveCurrentToPosition(i);
                list.ScrollIntoView(itemList[i]);
            }

            return false;
        }

        // Logic to handle UP and DOWN arrow keys in the text box associated with a list.
        // Behavior is similar to a Win32 combo box.
        private void OnComboBoxPreviewKeyDown(TextBox textBox, ListBox listBox, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    // Move up from the current position.
                    MoveListPosition(listBox, -1);
                    e.Handled = true;
                    break;

                case Key.Down:
                    // Move down from the current position, unless the item at the current position is
                    // not already selected in which case select it.
                    if (listBox.Items.CurrentPosition == listBox.SelectedIndex)
                    {
                        MoveListPosition(listBox, +1);
                    }
                    else
                    {
                        MoveListPosition(listBox, 0);
                    }
                    e.Handled = true;
                    break;
            }
        }

        private void MoveListPosition(ListBox listBox, int distance)
        {
            var i = listBox.Items.CurrentPosition + distance;
            if (i >= 0 && i < listBox.Items.Count)
            {
                listBox.Items.MoveCurrentToPosition(i);
                listBox.SelectedIndex = i;
                listBox.ScrollIntoView(listBox.Items[i]);
            }
        }

        #endregion
    }
}