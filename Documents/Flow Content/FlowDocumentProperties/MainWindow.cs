// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace FlowDocumentProperties
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // For quick access to the underlying DynamicPageinator.
        private DynamicDocumentPaginator _dynPaginator;
        private TextEffectTarget[] _textEffectTargets;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _dynPaginator = ((IDocumentPaginatorSource) fd1).DocumentPaginator as DynamicDocumentPaginator;
            if (_dynPaginator == null)
                throw new NullReferenceException("Can't get a DynamicDocumentPaginator for FlowDocument fd1.");
        }

        public void MakebgVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            backgroundProp.Visibility = Visibility.Visible;
            tb2.Text =
                "If you set the Foreground and Background colors to the same color, TextContent will no longer be visible.";
        }

        public void MakecgVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            columngapProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakecrbVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            columnrulebrushProp.Visibility = Visibility.Visible;
            tb2.Text = "This property will have no effect if ColumnRuleWidth is not set.";
        }

        public void MakecrwVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            columnrulewidthProp.Visibility = Visibility.Visible;
            tb2.Text = "This property will have no effect if ColumnRuleBrush is not set.";
        }

        public void MakecwVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            columnwidthProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakeceVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            contentendProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakecsVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            contentstartProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakefdVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            flowdirectionProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakeffVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            fontfamilyProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakefsVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            fontsizeProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakefstVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            fontstretchProp.Visibility = Visibility.Visible;
            tb2.Text = "Some fonts do not support all FontStretch values.";
        }

        public void MakefstyleVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            fontstyleProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakefwVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            fontweightProp.Visibility = Visibility.Visible;
            tb2.Text = "Not all FontWeight values result in unique rendering.";
        }

        public void MakebpeVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            isbackgroundpaginationenabledProp.Visibility = Visibility.Visible;
            tb2.Text = "This setting will not effect visual rendering of content.";
        }

        public void MakecwfVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            iscolumnwidthflexibleProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakepcfVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            ispagecountfinalProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakefgVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp1.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            foregroundProp.Visibility = Visibility.Visible;
            tb2.Text =
                "If you set the Foreground and Background colors to the same color, TextContent will no longer be visible.";
        }

        public void MakelhVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            lineheightProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakemaxphVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            maxpageheightProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakemaxpwVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            maxpagewidthProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakeminphVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            minpageheightProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakeminpwVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            minpagewidthProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakepcountVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            pagecountProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakephVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            pageheightProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakeppVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            pagepaddingProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakepsVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            pagesizeProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MakepwVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            pagewidthProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        public void MaketaVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            textalignmentProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        /*
        public void makettVisible(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            texttrimmingProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }
        */

        public void MaketeVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            texteffectsProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }

        /*
        public void maketwVisible(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            textwrapProp.Visibility = Visibility.Visible;
            tb2.Text = "";
        }
        */

        public void MaketpVisible(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in sp2.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            typographyProp.Visibility = Visibility.Visible;
            fd1.FontFamily = new FontFamily("Palatino Linotype");
            tb2.Text =
                "FontFamily has been changed to Palatino as it is an OpenType font and supports Typography features.";
        }

        // Begin Background change methods
        public void SetBackgroundRed(object sender, RoutedEventArgs e)
        {
            fd1.Background = Brushes.Red;
        }

        public void SetBackgroundBlue(object sender, RoutedEventArgs e)
        {
            fd1.Background = Brushes.Blue;
        }

        public void SetBackgroundGreen(object sender, RoutedEventArgs e)
        {
            fd1.Background = Brushes.Green;
        }

        public void SetBackgroundPurple(object sender, RoutedEventArgs e)
        {
            fd1.Background = Brushes.Purple;
        }

        public void SetBackgroundWhite(object sender, RoutedEventArgs e)
        {
            fd1.Background = Brushes.White;
        }

        // Begin ColumnGap change methods
        public void Cg5(object sender, RoutedEventArgs e)
        {
            fd1.ColumnGap = 5;
        }

        public void Cg10(object sender, RoutedEventArgs e)
        {
            fd1.ColumnGap = 10;
        }

        public void Cg15(object sender, RoutedEventArgs e)
        {
            fd1.ColumnGap = 15;
        }

        public void Cg20(object sender, RoutedEventArgs e)
        {
            fd1.ColumnGap = 20;
        }

        // Begin ColumnRuleBrush change methods
        public void SetcolruleGray(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleBrush = Brushes.Gray;
        }

        public void SetcolruleBlack(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleBrush = Brushes.Black;
        }

        public void SetcolruleRed(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleBrush = Brushes.Red;
        }

        public void SetcolruleBlue(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleBrush = Brushes.Blue;
        }

        // Begin ColumnRuleWidth change methods
        public void Crw1(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleWidth = 1;
        }

        public void Crw3(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleWidth = 3;
        }

        public void Crw5(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleWidth = 5;
        }

        public void Crw10(object sender, RoutedEventArgs e)
        {
            fd1.ColumnRuleWidth = 10;
        }

        // Begin ColumnWidth change methods
        public void Cw100(object sender, RoutedEventArgs e)
        {
            fd1.ColumnWidth = 100;
        }

        public void Cw150(object sender, RoutedEventArgs e)
        {
            fd1.ColumnWidth = 150;
        }

        public void Cw200(object sender, RoutedEventArgs e)
        {
            fd1.ColumnWidth = 200;
        }

        public void Cw250(object sender, RoutedEventArgs e)
        {
            fd1.ColumnWidth = 250;
        }

        // Begin ContentEnd method
        public void GetCeValue(object sender, RoutedEventArgs e)
        {
            var para1 = new Paragraph();
            fd1.Blocks.Add(para1);
            para1.Inlines.Add(new Run(" " + "Text added to the end of the FlowDocument."));
        }

        // Begin ContentStart method
        public void GetCsValue(object sender, RoutedEventArgs e)
        {
            var para2 = new Paragraph();
            fd1.Blocks.Add(para2);
            para2.Inlines.Add(new Run(" " + "Text added to the beginning of the FlowDocument."));
        }

        // Begin FlowDirection change methods
        public void Fdirection1(object sender, RoutedEventArgs e)
        {
            fd1.FlowDirection = FlowDirection.LeftToRight;
        }

        public void Fdirection2(object sender, RoutedEventArgs e)
        {
            fd1.FlowDirection = FlowDirection.RightToLeft;
        }

        // Begin FontFamily change methods
        public void FfTimes(object sender, RoutedEventArgs e)
        {
            fd1.FontFamily = new FontFamily("Times New Roman");
        }

        public void FfVerdana(object sender, RoutedEventArgs e)
        {
            fd1.FontFamily = new FontFamily("Verdana");
        }

        public void FfPalatino(object sender, RoutedEventArgs e)
        {
            fd1.FontFamily = new FontFamily("Palatino Linotype");
        }

        public void FfCourier(object sender, RoutedEventArgs e)
        {
            fd1.FontFamily = new FontFamily("Courier New");
        }

        // Begin FontSize change methods
        public void Fs10(object sender, RoutedEventArgs e)
        {
            fd1.FontSize = 10;
        }

        public void Fs15(object sender, RoutedEventArgs e)
        {
            fd1.FontSize = 15;
        }

        public void Fs20(object sender, RoutedEventArgs e)
        {
            fd1.FontSize = 20;
        }

        public void Fs25(object sender, RoutedEventArgs e)
        {
            fd1.FontSize = 25;
        }

        // Begin FontStretch change methods
        public void FstNormal(object sender, RoutedEventArgs e)
        {
            fd1.FontStretch = FontStretches.Normal;
        }

        public void FstCondensed(object sender, RoutedEventArgs e)
        {
            fd1.FontStretch = FontStretches.Condensed;
        }

        public void FstExpanded(object sender, RoutedEventArgs e)
        {
            fd1.FontStretch = FontStretches.Expanded;
        }

        public void FstMedium(object sender, RoutedEventArgs e)
        {
            fd1.FontStretch = FontStretches.Medium;
        }

        // Begin FontWeight change methods
        public void FwNormal(object sender, RoutedEventArgs e)
        {
            fd1.FontWeight = FontWeights.Normal;
        }

        public void FwLight(object sender, RoutedEventArgs e)
        {
            fd1.FontWeight = FontWeights.Light;
        }

        public void FwMedium(object sender, RoutedEventArgs e)
        {
            fd1.FontWeight = FontWeights.Medium;
        }

        public void FwBold(object sender, RoutedEventArgs e)
        {
            fd1.FontWeight = FontWeights.Bold;
        }

        public void FwUltraBold(object sender, RoutedEventArgs e)
        {
            fd1.FontWeight = FontWeights.UltraBold;
        }

        // Begin FontStyle change methods
        public void FstyleNormal(object sender, RoutedEventArgs e)
        {
            fd1.FontStyle = FontStyles.Normal;
        }

        public void FstyleItalic(object sender, RoutedEventArgs e)
        {
            fd1.FontStyle = FontStyles.Italic;
        }

        public void FstyleOblique(object sender, RoutedEventArgs e)
        {
            fd1.FontStyle = FontStyles.Oblique;
        }

        // Begin Foreground change methods
        public void ForegroundWhite(object sender, RoutedEventArgs e)
        {
            fd1.Foreground = Brushes.White;
        }

        public void ForegroundBlack(object sender, RoutedEventArgs e)
        {
            fd1.Foreground = Brushes.Black;
        }

        public void ForegroundBlue(object sender, RoutedEventArgs e)
        {
            fd1.Foreground = Brushes.Blue;
        }

        public void ForegroundRed(object sender, RoutedEventArgs e)
        {
            fd1.Foreground = Brushes.Red;
        }

        // Begin IsBackgroundPaginationEnabled change methods
        public void BpeTrue(object sender, RoutedEventArgs e)
        {
            _dynPaginator.IsBackgroundPaginationEnabled = true;
            tb2.Text = "IsBackgroundPaginationEnabled property is set to " +
                       _dynPaginator.IsBackgroundPaginationEnabled;
        }

        public void BpeFalse(object sender, RoutedEventArgs e)
        {
            _dynPaginator.IsBackgroundPaginationEnabled = false;
            tb2.Text = "IsBackgroundPaginationEnabled property is set to " +
                       _dynPaginator.IsBackgroundPaginationEnabled;
        }

        // Begin IsColumnWidthFlexible change methods
        public void CwfTrue(object sender, RoutedEventArgs e)
        {
            fd1.IsColumnWidthFlexible = true;
            tb2.Text = "IsColumnWidthFlexible property is set to " + fd1.IsColumnWidthFlexible;
        }

        public void CwfFalse(object sender, RoutedEventArgs e)
        {
            fd1.IsColumnWidthFlexible = false;
            tb2.Text = "IsColumnWidthFlexible property is set to " + fd1.IsColumnWidthFlexible;
        }

        // Begin IsPageCountFinal get method
        public void GetpcfValue(object sender, RoutedEventArgs e)
        {
            tb2.Text = "IsPageCountFinal value is " + _dynPaginator.IsPageCountValid;
        }

        // Begin Lineheight change methods
        public void Lineheight10(object sender, RoutedEventArgs e)
        {
            fd1.LineHeight = 10;
        }

        public void Lineheight20(object sender, RoutedEventArgs e)
        {
            fd1.LineHeight = 20;
        }

        public void Lineheight30(object sender, RoutedEventArgs e)
        {
            fd1.LineHeight = 30;
        }

        public void Lineheight40(object sender, RoutedEventArgs e)
        {
            fd1.LineHeight = 40;
        }

        // Begin MaxPageHeight change methods
        public void Maxpageheight500(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageHeight = 500;
            tb2.Text = "MaxPageHeight property is set to " + fd1.MaxPageHeight;
        }

        public void Maxpageheight600(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageHeight = 600;
            tb2.Text = "MaxPageHeight property is set to " + fd1.MaxPageHeight;
        }

        public void Maxpageheight700(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageHeight = 700;
            tb2.Text = "MaxPageHeight property is set to " + fd1.MaxPageHeight;
        }

        public void Maxpageheight800(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageHeight = 800;
            tb2.Text = "MaxPageHeight property is set to " + fd1.MaxPageHeight;
        }

        // Begin MaxPageWidth change methods
        public void Maxpagewidth300(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageWidth = 300;
            tb2.Text = "MaxPageWidth property is set to " + fd1.MaxPageWidth;
        }

        public void Maxpagewidth400(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageWidth = 400;
            tb2.Text = "MaxPageWidth property is set to " + fd1.MaxPageWidth;
        }

        public void Maxpagewidth500(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageWidth = 500;
            tb2.Text = "MaxPageWidth property is set to " + fd1.MaxPageWidth;
        }

        public void Maxpagewidth600(object sender, RoutedEventArgs e)
        {
            fd1.MaxPageWidth = 600;
            tb2.Text = "MaxPageWidth property is set to " + fd1.MaxPageWidth;
        }

        // Begin MinPageHeight change methods
        public void Minpageheight200(object sender, RoutedEventArgs e)
        {
            fd1.MinPageHeight = 200;
            tb2.Text = "MinPageHeight property is set to " + fd1.MinPageHeight;
        }

        public void Minpageheight300(object sender, RoutedEventArgs e)
        {
            fd1.MinPageHeight = 300;
            tb2.Text = "MaxPageHeight property is set to " + fd1.MinPageHeight;
        }

        public void Minpageheight400(object sender, RoutedEventArgs e)
        {
            fd1.MinPageHeight = 400;
            tb2.Text = "MinPageHeight property is set to " + fd1.MinPageHeight;
        }

        public void Minpageheight500(object sender, RoutedEventArgs e)
        {
            fd1.MinPageHeight = 500;
            tb2.Text = "MinPageHeight property is set to " + fd1.MinPageHeight;
        }

        // Begin MinPageWidth change methods
        public void Minpagewidth200(object sender, RoutedEventArgs e)
        {
            fd1.MinPageWidth = 200;
            tb2.Text = "MinPageWidth property is set to " + fd1.MinPageWidth;
        }

        public void Minpagewidth300(object sender, RoutedEventArgs e)
        {
            fd1.MinPageWidth = 300;
            tb2.Text = "MaxPageWidth property is set to " + fd1.MinPageWidth;
        }

        public void Minpagewidth400(object sender, RoutedEventArgs e)
        {
            fd1.MinPageWidth = 400;
            tb2.Text = "MinPageWidth property is set to " + fd1.MinPageWidth;
        }

        public void Minpagewidth500(object sender, RoutedEventArgs e)
        {
            fd1.MinPageWidth = 500;
            tb2.Text = "MinPageWidth property is set to " + fd1.MinPageWidth;
        }

        // Begin PageCount get method
        public void GetpcValue(object sender, RoutedEventArgs e)
        {
            tb2.Text = "PageCount value is " + _dynPaginator.PageCount;
        }

        // Begin PageHeight change methods
        public void Pageheight200(object sender, RoutedEventArgs e)
        {
            fd1.PageHeight = 200;
            tb2.Text = "PageHeight property is set to " + fd1.PageHeight;
        }

        public void Pageheight400(object sender, RoutedEventArgs e)
        {
            fd1.PageHeight = 400;
            tb2.Text = "PageHeight property is set to " + fd1.PageHeight;
        }

        public void Pageheight600(object sender, RoutedEventArgs e)
        {
            fd1.PageHeight = 600;
            tb2.Text = "PageHeight property is set to " + fd1.PageHeight;
        }

        public void Pageheight800(object sender, RoutedEventArgs e)
        {
            fd1.PageHeight = 800;
            tb2.Text = "PageHeight property is set to " + fd1.PageHeight;
        }

        // Begin PagePadding change methods
        public void Pagepadding5(object sender, RoutedEventArgs e)
        {
            fd1.PagePadding = new Thickness(5);
            tb2.Text = "PagePadding property is set to " + fd1.PagePadding;
        }

        public void Pagepadding10(object sender, RoutedEventArgs e)
        {
            fd1.PagePadding = new Thickness(10);
            tb2.Text = "PagePadding property is set to " + fd1.PagePadding;
        }

        public void Pagepadding15(object sender, RoutedEventArgs e)
        {
            fd1.PagePadding = new Thickness(15);
            tb2.Text = "PagePadding property is set to " + fd1.PagePadding;
        }

        public void Pagepadding20(object sender, RoutedEventArgs e)
        {
            fd1.PagePadding = new Thickness(20);
            tb2.Text = "PagePadding property is set to " + fd1.PagePadding;
        }

        // Begin PageSize change methods
        public void Pagesize200(object sender, RoutedEventArgs e)
        {
            // fd1.PageSize = new Size(200, 200);
            // tb2.Text = "PageSize property is set to " + fd1.PageSize.ToString();
            _dynPaginator.PageSize = new Size(200, 200);
            tb2.Text = "PageSize is set to " + _dynPaginator.PageSize;
        }

        public void Pagesize400(object sender, RoutedEventArgs e)
        {
            _dynPaginator.PageSize = new Size(400, 400);
            tb2.Text = "PageSize property is set to " + _dynPaginator.PageSize;
        }

        public void Pagesize600(object sender, RoutedEventArgs e)
        {
            _dynPaginator.PageSize = new Size(600, 600);
            tb2.Text = "PageSize property is set to " + _dynPaginator.PageSize;
        }

        public void Pagesize800(object sender, RoutedEventArgs e)
        {
            _dynPaginator.PageSize = new Size(800, 800);
            tb2.Text = "PageSize property is set to " + _dynPaginator.PageSize;
        }

        // Begin PageWidth change methods
        public void Pagewidth200(object sender, RoutedEventArgs e)
        {
            fd1.PageWidth = 200;
            tb2.Text = "PageWidth property is set to " + fd1.PageWidth;
        }

        public void Pagewidth400(object sender, RoutedEventArgs e)
        {
            fd1.PageWidth = 400;
            tb2.Text = "PageWidth property is set to " + fd1.PageWidth;
        }

        public void Pagewidth600(object sender, RoutedEventArgs e)
        {
            fd1.PageWidth = 600;
            tb2.Text = "PageWidth property is set to " + fd1.PageWidth;
        }

        // Begin TextAlignment change methods
        public void TalignLeft(object sender, RoutedEventArgs e)
        {
            fd1.TextAlignment = TextAlignment.Left;
        }

        public void TalignRight(object sender, RoutedEventArgs e)
        {
            fd1.TextAlignment = TextAlignment.Right;
        }

        public void TalignCenter(object sender, RoutedEventArgs e)
        {
            fd1.TextAlignment = TextAlignment.Center;
        }

        public void TalignJustify(object sender, RoutedEventArgs e)
        {
            fd1.TextAlignment = TextAlignment.Justify;
        }

        // Begin Typography change methods
        public void Typo1(object sender, RoutedEventArgs e)
        {
            Typography.SetHistoricalForms(fd1, false);
            Typography.SetVariants(fd1, FontVariants.Normal);
            Typography.SetCapitals(fd1, FontCapitals.SmallCaps);
        }

        public void Typo2(object sender, RoutedEventArgs e)
        {
            Typography.SetHistoricalForms(fd1, false);
            Typography.SetCapitals(fd1, FontCapitals.Normal);
            Typography.SetVariants(fd1, FontVariants.Subscript);
        }

        public void Typo3(object sender, RoutedEventArgs e)
        {
            Typography.SetVariants(fd1, FontVariants.Normal);
            Typography.SetCapitals(fd1, FontCapitals.Normal);
            Typography.SetHistoricalForms(fd1, true);
        }

        // Begin TextEffect methods
        public void TeTranslate(object sender, RoutedEventArgs e)
        {
            DisableTextEffects();

            var myEffect = new TextEffect
            {
                PositionStart = 0,
                PositionCount = 999
            };
            var myTranslateTransform = new TranslateTransform(50, 50);
            myEffect.Transform = myTranslateTransform;

            EnableTextEffects(fd1, myEffect);
            tb2.Text = "The TranslateTransform applied moved the TextBlock to an offset position of 50,50.";
        }

        public void TeScale(object sender, RoutedEventArgs e)
        {
            DisableTextEffects();

            var myEffect = new TextEffect
            {
                PositionStart = 0,
                PositionCount = 999
            };
            var myScaleTransform = new ScaleTransform(5, 5);
            myEffect.Transform = myScaleTransform;

            EnableTextEffects(fd1, myEffect);
            tb2.Text = "The ScaleTransform applied scaled the text by a factor of 5.";
        }

        public void TeRotate(object sender, RoutedEventArgs e)
        {
            DisableTextEffects();

            var myEffect = new TextEffect
            {
                PositionStart = 0,
                PositionCount = 999
            };
            var myRotateTransform = new RotateTransform(45);
            myEffect.Transform = myRotateTransform;

            EnableTextEffects(fd1, myEffect);
            tb2.Text = "The RotateTransform applied rotated the text by 45 degrees.";
        }

        private void DisableTextEffects()
        {
            if (_textEffectTargets != null)
            {
                foreach (var target in _textEffectTargets)
                    target.Disable();
            }
        }

        private void EnableTextEffects(FlowDocument fd, TextEffect effect)
        {
            _textEffectTargets = TextEffectResolver.Resolve(fd.ContentStart, fd.ContentEnd, effect);
            foreach (var target in _textEffectTargets)
                target.Enable();
        }
    }
}