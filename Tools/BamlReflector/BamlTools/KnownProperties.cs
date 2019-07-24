// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//using System.Windows.Controls;
//using System.Windows.Media.Animation;
//using System.Windows.Media.Effects;
//using System.Windows.Controls.Primitives;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Media3D;
//using System.Windows.Documents;
//using System.Windows.Shapes;
//using System.Windows.Markup;
//using System.Windows.Input;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Xaml;
//using System.Reflection;
//using Microsoft.Windows.Themes;

namespace BamlTools
{
    class KnownProperty
    {
        public short BamlNumber { get; private set; }
        public string PropertyName { get; private set; }
        public string DeclaringTypeName { get; private set; }

        public KnownProperty(short bamlNumber, string declaringTypeName, string propertyName)
        {
            BamlNumber = bamlNumber;
            DeclaringTypeName = declaringTypeName;
            PropertyName = propertyName;
        }

        public string CreateMethodName
        {
            get { return "Create_BamlProperty_" + DeclaringTypeName + "_" + PropertyName; }
        }
    }

    class KnownProperties
    {
        public const short MaxDependencyProperty = 137;

        static public KnownProperty[] _knownProperties;

        static public KnownProperty[] Properties
        {
            get
            {
                if (_knownProperties == null)
                {
                    InitializeKnownProperties();
                }
                return _knownProperties;
            }
        }


        static private void InitializeKnownProperties()
        {
            _knownProperties = new KnownProperty[]
            {
                new KnownProperty(0, "ERROR 0", "ERROR 0"),
                new KnownProperty(1,  "AccessText",  "Text"),
                new KnownProperty(2,  "BeginStoryboard",  "Storyboard"),
                new KnownProperty(3,  "BitmapEffectGroup",  "Children"),
                new KnownProperty(4,  "Border",  "Background"),
                new KnownProperty(5,  "Border",  "BorderBrush"),
                new KnownProperty(6,  "Border",  "BorderThickness"),
                new KnownProperty(7,  "ButtonBase",  "Command"),
                new KnownProperty(8,  "ButtonBase",  "CommandParameter"),
                new KnownProperty(9,  "ButtonBase",  "CommandTarget"),
                new KnownProperty(10,  "ButtonBase",  "IsPressed"),
                new KnownProperty(11,  "ColumnDefinition",  "MaxWidth"),
                new KnownProperty(12,  "ColumnDefinition",  "MinWidth"),
                new KnownProperty(13,  "ColumnDefinition",  "Width"),
                new KnownProperty(14,  "ContentControl",  "Content"),
                new KnownProperty(15,  "ContentControl",  "ContentTemplate"),
                new KnownProperty(16,  "ContentControl",  "ContentTemplateSelector"),
                new KnownProperty(17,  "ContentControl",  "HasContent"),  // readonly
                new KnownProperty(18,  "ContentElement",  "Focusable"),
                new KnownProperty(19,  "ContentPresenter",  "Content"),
                new KnownProperty(20,  "ContentPresenter",  "ContentSource"),
                new KnownProperty(21,  "ContentPresenter",  "ContentTemplate"),
                new KnownProperty(22,  "ContentPresenter",  "ContentTemplateSelector"),
                new KnownProperty(23,  "ContentPresenter",  "RecognizesAccessKey"),
                new KnownProperty(24,  "Control",  "Background"),
                new KnownProperty(25,  "Control",  "BorderBrush"),
                new KnownProperty(26,  "Control",  "BorderThickness"),
                new KnownProperty(27,  "Control",  "FontFamily"),
                new KnownProperty(28,  "Control",  "FontSize"),
                new KnownProperty(29,  "Control",  "FontStretch"),
                new KnownProperty(30,  "Control",  "FontStyle"),
                new KnownProperty(31,  "Control",  "FontWeight"),
                new KnownProperty(32,  "Control",  "Foreground"),
                new KnownProperty(33,  "Control",  "HorizontalContentAlignment"),
                new KnownProperty(34,  "Control",  "IsTabStop"),
                new KnownProperty(35,  "Control",  "Padding"),
                new KnownProperty(36,  "Control",  "TabIndex"),
                new KnownProperty(37,  "Control",  "Template"),
                new KnownProperty(38,  "Control",  "VerticalContentAlignment"),
                new KnownProperty(39,  "DockPanel",  "Dock"),
                new KnownProperty(40,  "DockPanel",  "LastChildFill"),
                new KnownProperty(41,  "DocumentViewerBase",  "Document"),
                new KnownProperty(42,  "DrawingGroup",  "Children"),
                new KnownProperty(43,  "FlowDocumentReader",  "Document"),
                new KnownProperty(44,  "FlowDocumentScrollViewer",  "Document"),
                new KnownProperty(45,  "FrameworkContentElement",  "Style"),
                new KnownProperty(46,  "FrameworkElement",  "FlowDirection"),
                new KnownProperty(47,  "FrameworkElement",  "Height"),
                new KnownProperty(48,  "FrameworkElement",  "HorizontalAlignment"),
                new KnownProperty(49,  "FrameworkElement",  "Margin"),
                new KnownProperty(50,  "FrameworkElement",  "MaxHeight"),
                new KnownProperty(51,  "FrameworkElement",  "MaxWidth"),
                new KnownProperty(52,  "FrameworkElement",  "MinHeight"),
                new KnownProperty(53,  "FrameworkElement",  "MinWidth"),
                new KnownProperty(54,  "FrameworkElement",  "Name"),
                new KnownProperty(55,  "FrameworkElement",  "Style"),
                new KnownProperty(56,  "FrameworkElement",  "VerticalAlignment"),
                new KnownProperty(57,  "FrameworkElement",  "Width"),
                new KnownProperty(58,  "GeneralTransformGroup",  "Children"),
                new KnownProperty(59,  "GeometryGroup",  "Children"),
                new KnownProperty(60,  "GradientBrush",  "GradientStops"),
                new KnownProperty(61,  "Grid",  "Column"),
                new KnownProperty(62,  "Grid",  "ColumnSpan"),
                new KnownProperty(63,  "Grid",  "Row"),
                new KnownProperty(64,  "Grid",  "RowSpan"),
                new KnownProperty(65,  "GridViewColumn",  "Header"),
                new KnownProperty(66,  "HeaderedContentControl",  "HasHeader"),  // readonly 
                new KnownProperty(67,  "HeaderedContentControl",  "Header"),
                new KnownProperty(68,  "HeaderedContentControl",  "HeaderTemplate"),
                new KnownProperty(69,  "HeaderedContentControl",  "HeaderTemplateSelector"),
                new KnownProperty(70,  "HeaderedItemsControl",  "HasHeader"),  // readonly
                new KnownProperty(71,  "HeaderedItemsControl",  "Header"),
                new KnownProperty(72,  "HeaderedItemsControl",  "HeaderTemplate"),
                new KnownProperty(73,  "HeaderedItemsControl",  "HeaderTemplateSelector"),
                new KnownProperty(74,  "Hyperlink",  "NavigateUri"),
                new KnownProperty(75,  "Image",  "Source"),
                new KnownProperty(76,  "Image",  "Stretch"),
                new KnownProperty(77,  "ItemsControl",  "ItemContainerStyle"),
                new KnownProperty(78,  "ItemsControl",  "ItemContainerStyleSelector"),
                new KnownProperty(79,  "ItemsControl",  "ItemTemplate"),
                new KnownProperty(80,  "ItemsControl",  "ItemTemplateSelector"),
                new KnownProperty(81,  "ItemsControl",  "ItemsPanel"),
                new KnownProperty(82,  "ItemsControl",  "ItemsSource"),
                new KnownProperty(83,  "MaterialGroup",  "Children"),
                new KnownProperty(84,  "Model3DGroup",  "Children"),
                new KnownProperty(85,  "Page",  "Content"),
                new KnownProperty(86,  "Panel",  "Background"),
                new KnownProperty(87,  "Path",  "Data"),
                new KnownProperty(88,  "PathFigure",  "Segments"),
                new KnownProperty(89,  "PathGeometry",  "Figures"),
                new KnownProperty(90,  "Popup",  "Child"),
                new KnownProperty(91,  "Popup",  "IsOpen"),
                new KnownProperty(92,  "Popup",  "Placement"),
                new KnownProperty(93,  "Popup",  "PopupAnimation"),
                new KnownProperty(94,  "RowDefinition",  "Height"),
                new KnownProperty(95,  "RowDefinition",  "MaxHeight"),
                new KnownProperty(96,  "RowDefinition",  "MinHeight"),
                new KnownProperty(97,  "ScrollViewer",  "CanContentScroll"),
                new KnownProperty(98,  "ScrollViewer",  "HorizontalScrollBarVisibility"),
                new KnownProperty(99,  "ScrollViewer",  "VerticalScrollBarVisibility"),
                new KnownProperty(100,  "Shape",  "Fill"),
                new KnownProperty(101,  "Shape",  "Stroke"),
                new KnownProperty(102,  "Shape",  "StrokeThickness"),
                new KnownProperty(103,  "TextBlock",  "Background"),
                new KnownProperty(104,  "TextBlock",  "FontFamily"),
                new KnownProperty(105,  "TextBlock",  "FontSize"),
                new KnownProperty(106,  "TextBlock",  "FontStretch"),
                new KnownProperty(107,  "TextBlock",  "FontStyle"),
                new KnownProperty(108,  "TextBlock",  "FontWeight"),
                new KnownProperty(109,  "TextBlock",  "Foreground"),
                new KnownProperty(110,  "TextBlock",  "Text"),
                new KnownProperty(111,  "TextBlock",  "TextDecorations"),
                new KnownProperty(112,  "TextBlock",  "TextTrimming"),
                new KnownProperty(113,  "TextBlock",  "TextWrapping"),
                new KnownProperty(114,  "TextBox",  "Text"),
                new KnownProperty(115,  "TextElement",  "Background"),
                new KnownProperty(116,  "TextElement",  "FontFamily"),
                new KnownProperty(117,  "TextElement",  "FontSize"),
                new KnownProperty(118,  "TextElement",  "FontStretch"),
                new KnownProperty(119,  "TextElement",  "FontStyle"),
                new KnownProperty(120,  "TextElement",  "FontWeight"),
                new KnownProperty(121,  "TextElement",  "Foreground"),
                new KnownProperty(122,  "TimelineGroup",  "Children"),
                new KnownProperty(123,  "Track",  "IsDirectionReversed"),
                new KnownProperty(124,  "Track",  "Maximum"),
                new KnownProperty(125,  "Track",  "Minimum"),
                new KnownProperty(126,  "Track",  "Orientation"),
                new KnownProperty(127,  "Track",  "Value"),
                new KnownProperty(128,  "Track",  "ViewportSize"),
                new KnownProperty(129,  "Transform3DGroup",  "Children"),
                new KnownProperty(130,  "TransformGroup",  "Children"),
                new KnownProperty(131,  "UIElement",  "ClipToBounds"),
                new KnownProperty(132,  "UIElement",  "Focusable"),
                new KnownProperty(133,  "UIElement",  "IsEnabled"),
                new KnownProperty(134,  "UIElement",  "RenderTransform"),
                new KnownProperty(135,  "UIElement",  "Visibility"),
                new KnownProperty(136,  "Viewport3D",  "Children"),
                new KnownProperty(137,  "ERROR 137", "ERROR 137"),      // MaxDependencyProperty",
                new KnownProperty(138,  "AdornedElementPlaceholder",  "Child"),
                new KnownProperty(139,  "AdornerDecorator",  "Child"),
                new KnownProperty(140,  "AnchoredBlock",  "Blocks"),
                new KnownProperty(141,  "ArrayExtension",  "Items"),
                new KnownProperty(142,  "BlockUIContainer",  "Child"),
                new KnownProperty(143,  "Bold",  "Inlines"),
                new KnownProperty(144,  "BooleanAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(145,  "Border",  "Child"),
                new KnownProperty(146,  "BulletDecorator",  "Child"),
                new KnownProperty(147,  "Button",  "Content"),
                new KnownProperty(148,  "ButtonBase",  "Content"),
                new KnownProperty(149,  "ByteAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(150,  "Canvas",  "Children"),
                new KnownProperty(151,  "CharAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(152,  "CheckBox",  "Content"),
                new KnownProperty(153,  "ColorAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(154,  "ComboBox",  "Items"),
                new KnownProperty(155,  "ComboBoxItem",  "Content"),
                new KnownProperty(156,  "ContextMenu",  "Items"),
                new KnownProperty(157,  "ControlTemplate",  "VisualTree"),
                new KnownProperty(158,  "DataTemplate",  "VisualTree"),
                new KnownProperty(159,  "DataTrigger",  "Setters"),
                new KnownProperty(160,  "DecimalAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(161,  "Decorator",  "Child"),
                new KnownProperty(162,  "DockPanel",  "Children"),
                new KnownProperty(163,  "DocumentViewer",  "Document"),
                new KnownProperty(164,  "DoubleAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(165,  "EventTrigger",  "Actions"),
                new KnownProperty(166,  "Expander",  "Content"),
                new KnownProperty(167,  "Figure",  "Blocks"),
                new KnownProperty(168,  "FixedDocument",  "Pages"),
                new KnownProperty(169,  "FixedDocumentSequence",  "References"),
                new KnownProperty(170,  "FixedPage",  "Children"),
                new KnownProperty(171,  "Floater",  "Blocks"),
                new KnownProperty(172,  "FlowDocument",  "Blocks"),
                new KnownProperty(173,  "FlowDocumentPageViewer",  "Document"),
                new KnownProperty(174,  "FrameworkTemplate",  "VisualTree"),
                new KnownProperty(175,  "Grid",  "Children"),
                new KnownProperty(176,  "GridView",  "Columns"),
                new KnownProperty(177,  "GridViewColumnHeader",  "Content"),
                new KnownProperty(178,  "GroupBox",  "Content"),
                new KnownProperty(179,  "GroupItem",  "Content"),
                new KnownProperty(180,  "HeaderedContentControl",  "Content"),
                new KnownProperty(181,  "HeaderedItemsControl",  "Items"),
                new KnownProperty(182,  "HierarchicalDataTemplate",  "VisualTree"),
                new KnownProperty(183,  "Hyperlink",  "Inlines"),
                new KnownProperty(184,  "InkCanvas",  "Children"),
                new KnownProperty(185,  "InkPresenter",  "Child"),
                new KnownProperty(186,  "InlineUIContainer",  "Child"),
                new KnownProperty(187,  "InputScopeName",  "NameValue"),
                new KnownProperty(188,  "Int16AnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(189,  "Int32AnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(190,  "Int64AnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(191,  "Italic",  "Inlines"),
                new KnownProperty(192,  "ItemsControl",  "Items"),
                new KnownProperty(193,  "ItemsPanelTemplate",  "VisualTree"),
                new KnownProperty(194,  "Label",  "Content"),
                new KnownProperty(195,  "LinearGradientBrush",  "GradientStops"),
                new KnownProperty(196,  "List",  "ListItems"),
                new KnownProperty(197,  "ListBox",  "Items"),
                new KnownProperty(198,  "ListBoxItem",  "Content"),
                new KnownProperty(199,  "ListItem",  "Blocks"),
                new KnownProperty(200,  "ListView",  "Items"),
                new KnownProperty(201,  "ListViewItem",  "Content"),
                new KnownProperty(202,  "MatrixAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(203,  "Menu",  "Items"),
                new KnownProperty(204,  "MenuBase",  "Items"),
                new KnownProperty(205,  "MenuItem",  "Items"),
                new KnownProperty(206,  "ModelVisual3D",  "Children"),
                new KnownProperty(207,  "MultiBinding",  "Bindings"),
                new KnownProperty(208,  "MultiDataTrigger",  "Setters"),
                new KnownProperty(209,  "MultiTrigger",  "Setters"),
                new KnownProperty(210,  "ObjectAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(211,  "PageContent",  "Child"),
                new KnownProperty(212,  "PageFunctionBase",  "Content"),
                new KnownProperty(213,  "Panel",  "Children"),
                new KnownProperty(214,  "Paragraph",  "Inlines"),
                new KnownProperty(215,  "ParallelTimeline",  "Children"),
                new KnownProperty(216,  "Point3DAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(217,  "PointAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(218,  "PriorityBinding",  "Bindings"),
                new KnownProperty(219,  "QuaternionAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(220,  "RadialGradientBrush",  "GradientStops"),
                new KnownProperty(221,  "RadioButton",  "Content"),
                new KnownProperty(222,  "RectAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(223,  "RepeatButton",  "Content"),
                new KnownProperty(224,  "RichTextBox",  "Document"),
                new KnownProperty(225,  "Rotation3DAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(226,  "Run",  "Text"),
                new KnownProperty(227,  "ScrollViewer",  "Content"),
                new KnownProperty(228,  "Section",  "Blocks"),
                new KnownProperty(229,  "Selector",  "Items"),
                new KnownProperty(230,  "SingleAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(231,  "SizeAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(232,  "Span",  "Inlines"),
                new KnownProperty(233,  "StackPanel",  "Children"),
                new KnownProperty(234,  "StatusBar",  "Items"),
                new KnownProperty(235,  "StatusBarItem",  "Content"),
                new KnownProperty(236,  "Storyboard",  "Children"),
                new KnownProperty(237,  "StringAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(238,  "Style",  "Setters"),
                new KnownProperty(239,  "TabControl",  "Items"),
                new KnownProperty(240,  "TabItem",  "Content"),
                new KnownProperty(241,  "TabPanel",  "Children"),
                new KnownProperty(242,  "Table",  "RowGroups"),
                new KnownProperty(243,  "TableCell",  "Blocks"),
                new KnownProperty(244,  "TableRow",  "Cells"),
                new KnownProperty(245,  "TableRowGroup",  "Rows"),
                new KnownProperty(246,  "TextBlock",  "Inlines"),
                new KnownProperty(247,  "ThicknessAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(248,  "ToggleButton",  "Content"),
                new KnownProperty(249,  "ToolBar",  "Items"),
                new KnownProperty(250,  "ToolBarOverflowPanel",  "Children"),
                new KnownProperty(251,  "ToolBarPanel",  "Children"),
                new KnownProperty(252,  "ToolBarTray",  "ToolBars"),
                new KnownProperty(253,  "ToolTip",  "Content"),
                new KnownProperty(254,  "TreeView",  "Items"),
                new KnownProperty(255,  "TreeViewItem",  "Items"),
                new KnownProperty(256,  "Trigger",  "Setters"),
                new KnownProperty(257,  "Underline",  "Inlines"),
                new KnownProperty(258,  "UniformGrid",  "Children"),
                new KnownProperty(259,  "UserControl",  "Content"),
                new KnownProperty(260,  "Vector3DAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(261,  "VectorAnimationUsingKeyFrames",  "KeyFrames"),
                new KnownProperty(262,  "Viewbox",  "Child"),
                new KnownProperty(263,  "Viewport3DVisual",  "Children"),
                new KnownProperty(264,  "VirtualizingPanel",  "Children"),
                new KnownProperty(265,  "VirtualizingStackPanel",  "Children"),
                new KnownProperty(266,  "Window",  "Content"),
                new KnownProperty(267,  "WrapPanel",  "Children"),
                new KnownProperty(268,  "XmlDataProvider",  "XmlSerializer"),
            };
        }
    }
}
