# WPF-Samples
This repo contains the samples that demonstrate the API usage patterns and popular features for the Windows Presenation Foundation in the .NET Framework for Desktop. These samples were initially hosted on [MSDN](https://msdn.microsoft.com/en-us/library/vstudio/ms771633.aspx), and we are gradually 
moving all the interesting WPF samples over to GitHub.All the samples have been retargeted to [.NET 4.5.2](http://www.microsoft.com/en-us/download/details.aspx?id=42642).

For additional WPF samples, see [WPF Samples](https://msdn.microsoft.com/en-us/library/vstudio/ms771633.aspx).

## License
Unless otherwise mentioned, the samples are released under the [MIT license](https://github.com/Microsoft/WPF-Samples/blob/master/LICENSE)

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/WPF-Samples/issues)

Questions: mail wpfteam@microsoft.com

## WPF development
These samples require Visual Studio 2015 to build, test, and deploy. 

   [Get a free copy of Visual Studio 2015 Community Edition with support for building WPF apps](https://www.visualstudio.com/wpf-vs)

## Using the samples

The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in Visual Studio 2015.

   [Download the samples ZIP](../../archive/master.zip)

   **Notes:** 
   * Before you unzip the archive, right-click it, select Properties, and then select Unblock.
   * Most samples should work independently
   * By default, all the sample target .NET 4.5.2, you can change this to .NET 4.6 in the Project->Properties page in Visual Studio.

For more info about the programming models, platforms, languages, and APIs demonstrated in these samples, please refer to the guidance  available in  [MSDN](https://msdn.microsoft.com/en-us/library/ms754130.aspx). These samples are provided as-is in order to indicate or demonstrate the functionality of the programming models and feature APIs for WPF.

##Samples by category

<table>
 <tr>
  <th colspan="5" align="left">Getting Started</th>
 </tr>
<tr>
  <td><a href="Accessibility">Hello World Sample</a></td>
  <td><a href="Accessibility">Simple Layout</a></td>
  <td><a href="Accessibility">Complex Layout</a></td>
  <td><a href="Accessibility">Dynamic Application</a></td>
   <td><a href="Accessibility">Multipage Application</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">WPF Controls Gallery</a></td>
  <td><a href="Accessibility">Sample Gallery</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Base Elements</th>
 </tr>
<tr>
  <td><a href="Accessibility">ContextMenuOpening Handlers</a></td>
  <td><a href="Accessibility">Creating a FocusVisualStyle</a></td>
  <td><a href="Accessibility">Finding the Index of an Element in a Panel</a></td>
  <td><a href="Accessibility">Height Properties</a></td>
   <td><a href="Accessibility">Loaded Even</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Setting Margins</a></td>
  <td><a href="Accessibility">ThicknessConverter</a></td>
  <td><a href="Accessibility">Using Elements</a></td>
  <td><a href="Accessibility">Visibility Changes to a UIElement</a></td>
  <td><a href="Accessibility">Width Properties Comparison</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Element Tree and Serialization</th>
 </tr>
 <tr>
  <td><a href="Accessibility">Overriding the Logical Tree</a></td>
  <td><a href="Accessibility">Searching for an Element</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Properties</th>
 </tr>
 <tr>
  <td><a href="Accessibility">Custom classes with Dependency Properties</a></td>
  <td><a href="Accessibility">Restoring Default Values</a></td>
  <td><a href="Accessibility">PropertyChanged and CoerceValue Callbacks</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Events</th>
 </tr>
<tr>
  <td><a href="Accessibility">Adding an Event Handler</a></td>
  <td><a href="Accessibility">Custom Routed Events</a></td>
  <td><a href="Accessibility">Finding a Source Element in an Event Handler</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Routed Event Handling Sample</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Input and Commands</th>
 </tr>
<tr>
  <td><a href="Accessibility">Handling an Event When a Command</a></td>
  <td><a href="Accessibility">Handling an Event When a Command Occurs Using Code</a></td>
  <td><a href="Accessibility">Changing the Cursor Type</a></td>
  <td><a href="Accessibility">Detecting Mouse Button State</a></td>
   <td><a href="Accessibility">Firing Events When an Element Gains and Loses Focus</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Keyboard Key</a></td>
  <td><a href="Accessibility">Mouse Pointer</a></td>
  <td><a href="Accessibility">Moving an Object with the Mouse Pointer</a></td>
  <td><a href="Accessibility">Create Command Bindings Using Code</a></td>
  <td><a href="Accessibility">Create a Custom RoutedCommand</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">EditingCommands</a></td>
  <td><a href="Accessibility">Disable Command Source via Dispatcher Timer</a></td>
  <td><a href="Accessibility">Disable Command Source via System Timer</a></td>
  <td><a href="Accessibility">Capture and Uncapture</a></td>
  <td><a href="Accessibility">Implement ICommandSource</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Manipulate Focus Programmatically</a></td>
  <td><a href="Accessibility">Count the Key Strokes of a Specific Key</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Resources</th>
 </tr>
<tr>
  <td><a href="Accessibility">Application Resources</a></td>
  <td><a href="Accessibility">Defining a Resource</a></td>
  <td><a href="Accessibility">Merged ResourceDictionary</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Styles and Templates</th>
 </tr>
<tr>
  <td><a href="Accessibility">Introduction to Styling and Templating</a></td>
  <td><a href="Accessibility">Event Triggers</a></td>
  <td><a href="Accessibility">Content Control Style</a></td>
  <td><a href="Accessibility">Finding Template-Generated Elements</a></td>
   <td><a href="Accessibility">Alternating the Appearance of Items</a></td>
 </tr>
 <tr>
  <td><a href="https://msdn.microsoft.com/en-us/library/vstudio/aa358533.aspx">Themes</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Threading</th>
 </tr>
<tr>
  <td><a href="Accessibility">Multithreading Web Browser</a></td>
  <td><a href="Accessibility">Single Threaded Application with Long Running Calculation</a></td>
  <td><a href="Accessibility">Weather Service Simulation with Dispatcher</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Digital Ink</th>
 </tr>
<tr>
  <td><a href="https://msdn.microsoft.com/en-us/library/vstudio/aa972145.aspx">MSDN</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Accessibility</th>
 </tr>
<tr>
  <td><a href="https://msdn.microsoft.com/en-us/library/vstudio/ms771275.aspx">Test Script Generator(MSDN)</a></td>
  <td><a href="Accessibility">TrackFocus</a></td>
  <td><a href="Accessibility">FetchTimer</a></td>
  <td><a href="Accessibility">Highlighter</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">InvokePattern, ExpandCollapsePattern, and TogglePattern</a></td>
  <td><a href="Accessibility">SelectionPattern and SelectionItemPattern</a></td>
  <td><a href="Accessibility">TextPattern Search and Selection</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Application Management</th>
 </tr>
<tr>
  <td><a href="Accessibility">Application Activation and Deactivation</a></td>
  <td><a href="Accessibility">Application Cookies</a></td>
  <td><a href="Accessibility">Application Shutdown</a></td>
  <td><a href="Accessibility">Managing Unhandled Exceptions on Secondary UI Threads</a></td>
  <td><a href="Accessibility">Managing Unhandled Exceptions on Secondary Worker Threads</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Processing Command Line Arguments</a></td>
  <td><a href="Accessibility">Reusable Custom Application Class</a></td>
  <td><a href="Accessibility">Single Instance Detection</a></td>
  <td><a href="Accessibility">Skinned Application</a></td>
  <td><a href="Accessibility">Unhandled Application Exception</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Navigation</th>
 </tr>
<tr>
  <td><a href="https://msdn.microsoft.com/en-us/library/vstudio/aa972175.aspx">MSDN</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Application Extensibility</th>
 </tr>
<tr>
  <td><a href="https://msdn.microsoft.com/en-us/library/vstudio/bb913904.aspx">MSDN</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Clipboard</th>
 </tr>
<tr>
  <td><a href="Accessibility">Clipboard Spy</a></td>
  <td><a href="Accessibility">Clipboard Viewer</a></td>
 </tr>
</table>

<table>
<tr>
  <th colspan="5" align="left">DataBinding</th>
 </tr>
<tr>
  <td><a href="Accessibility">Simple Binding</a></td>
  <td><a href="Accessibility">Creating a Binding in Code</a></td>
  <td><a href="Accessibility">Controlling the Direction and Timing of the Data Flow</a></td>
  <td><a href="Accessibility">Binding with a Custom Value Converter</a></td>
  <td><a href="Accessibility">Sorting and Filtering Items in a View</a></td>
 </tr>
<tr>
  <td><a href="Accessibility">Sorting and Grouping Data in XAML</a></td>
  <td><a href="Accessibility">Adding and Removing Groups</a></td>
  <td><a href="Accessibility">Binding Validation</a></td>
  <td><a href="Accessibility">Business Layer Validation</a></td>
  <td><a href="Accessibility">Binding to XML Data</a></td>
 </tr>
<tr>
  <td><a href="Accessibility">XMLDataProvider with Embedded Data File</a></td>
  <td><a href="Accessibility">Binding Using XML Namespaces</a></td>
  <td><a href="Accessibility">Introduction to Data Templating</a></td>
  <td><a href="Accessibility">Binding Using Data Triggers</a></td>
  <td><a href="Accessibility">Displaying Hierarchical Data</a></td>
</tr>
<tr>
  <td><a href="Accessibility">Binding the Properties of UI Elements</a></td>
  <td><a href="Accessibility">Binding to a Collection</a></td>
  <td><a href="Accessibility">Binding Using Composite Collections</a></td>
  <td><a href="Accessibility">Master-Detail Scenario Using ObjectDataProvider</a></td>
  <td><a href="Accessibility">Master-Detail Scenario Using XmlDataProvider</a></td>
</tr>
<tr>
  <td><a href="Accessibility">inding to the Results of a Method</a></td>
  <td><a href="Accessibility">Implementing Property Change Notification</a></td>
  <td><a href="Accessibility">Implementing Parameterized MultiBinding</a></td>
  <td><a href="Accessibility">Binding Using PriorityBinding</a></td>
   <td><a href="Accessibility">Showing System Colors Using Data Services</a></td>
 </tr>
<tr>
  <td><a href="Accessibility">Explicitly Updating the Binding Source</a></td>
  <td><a href="Accessibility">Binding to an ADO.NET DataSet</a></td>
  <td><a href="Accessibility">Binding to a Web Service</a></td>
  <td><a href="Accessibility">LINQ Query</a></td>
   <td><a href="Accessibility">Formatting a String on a Binding</a></td>
 </tr>
<tr>
  <td><a href="Accessibility">Changing a Collection by Using IEditableCollectionView</a></td>
  <td><a href="Accessibility">Validate the Data of an Object</a></td>
   <td><a href="Accessibility">Validate an Item in an ItemsControl</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Drag & Drop</th>
 </tr>
<tr>
  <td><a href="Accessibility">Drag and Drop an Object on a Canvas</a></td>
  <td><a href="Accessibility">Create a Data Object from a Text Selection</a></td>
  <td><a href="Accessibility">Drag-and-Drop Events Explorer</a></td>
  <td><a href="Accessibility">Data Formats Spy</a></td>
   <td><a href="Accessibility">Load a Dropped File</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Thumb Drag Functionality</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Globalization and Localization</th>
 </tr>
<tr>
  <td><a href="Accessibility">Automatic Layout for Localizable Applications</a></td>
  <td><a href="Accessibility">FlowDirection</a></td>
  <td><a href="Accessibility">Gradient</a></td>
  <td><a href="Accessibility">Grid for Localizable Applications</a></td>
  <td><a href="Accessibility">Globalization Homepage</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Globalization Run Dialog</a></td>
  <td><a href="Accessibility">Image</a></td>
  <td><a href="Accessibility">Implementing Localizable String Resources in XAML</a></td>
  <td><a href="Accessibility">LanguageAttribute</a></td>
  <td><a href="Accessibility">LeftToRight and RightToLeft</a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">LocBaml Tool</a></td>
  <td><a href="Accessibility">Numbers</a></td>
  <td><a href="Accessibility">Numbers2</a></td>
  <td><a href="Accessibility">Numbers3</a></td>
  <td><a href="Accessibility">NumbersCSharp</a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Paths</a></td>
  <td><a href="Accessibility">Resources for Localizable Applications</a></td>
  <td><a href="Accessibility">RunSpan</a></td>
  <td><a href="Accessibility">Span</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Graphics</th>
 </tr>
<tr>
  <td><a href="Accessibility">Bitmap Effects</a></td>
  <td><a href="Accessibility">Brushes</a></td>
  <td><a href="Accessibility">Geometries</a></td>
  <td><a href="Accessibility">Imaging</a></td>
   <td><a href="Accessibility">Shapes</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Transformations</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Animation</th>
 </tr>
<tr>
  <td><a href="Accessibility">Animation Example</a></td>
  <td><a href="Accessibility">Property Animation</a></td>
  <td><a href="Accessibility">Animating the Opacity of an Element</a></td>
  <td><a href="Accessibility">Animation Timing Behavior</a></td>
   <td><a href="Accessibility">Custom Animation</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">From, To, and By Animation Target Values</a></td>
  <td><a href="Accessibility">KeyFrame Animation</a></td>
  <td><a href="Accessibility">Local Animations</a></td>
  <td><a href="Accessibility">Key Spline Animation</a></td>
  <td><a href="Accessibility">Path Animation/a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Per-Frame Animation</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Visual Layer</th>
 </tr>
<tr>
  <td><a href="Accessibility">Using the CompositionTarget</a></td>
  <td><a href="Accessibility">Hit Test Using DrawingVisuals</a></td>
  <td><a href="Accessibility">Hit Test with Win32 Interoperation</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Media</th>
 </tr>
<tr>
  <td><a href="Accessibility">Media Gallery</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Speech</th>
 </tr>
<tr>
  <td><a href="Accessibility">Using Speech</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Migration and Interoperability</th>
 </tr>
<tr>
  <td><a href="Accessibility">Arranging Windows Forms Controls in WPF</a></td>
  <td><a href="Accessibility">Data Binding in Hybrid Applications</a></td>
  <td><a href="Accessibility">Enabling Visual Styles in a Hybrid Application</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Hosting an ActiveX Control in WPF by Using XAML</a></td>
  <td><a href="Accessibility">Hosting a Simple WPF in Windows Forms</a></td>
  <td><a href="Accessibility">Hosting a Win32 ListBox Control in WPF</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Hosting a Windows Forms Composite Control in WPF</a></td>
  <td><a href="Accessibility">Hosting a Windows Forms Control in WPF</a></td>
  <td><a href="Accessibility">Hosting a Windows Forms Control in WPF</a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Hosting WPF Content in a Win32 Window</a></td>
  <td><a href="Accessibility">Hosting a WPF Composite Control in Windows Forms</a></td>
  <td><a href="Accessibility">Localizing a Hybrid Application/a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Mapping Properties Using the ElementHost Control</a></td>
  <td><a href="Accessibility">Mapping Properties Using the WindowsFormsHost Element</a></td>
  <td><a href="Accessibility">Win32 Clock Interoperation</a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Hosting a Win32 HWND in WPF</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Sample Applications</th>
 </tr>
<tr>
  <td><a href="Accessibility">Calculator</a></td>
  <td><a href="Accessibility">Concentric Rings</a></td>
  <td><a href="Accessibility">Cube Animation</a></td>
  <td><a href="Accessibility">Data Binding</a></td>
  <td><a href="Accessibility">Drop Shadow Ink</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Editing Examiner</a></td>
  <td><a href="Accessibility">ExpenseIt</a></td>
  <td><a href="Accessibility">ExpenseIt - Standalone</a></td>
  <td><a href="Accessibility">Font Dialog Box</a></td>
  <td><a href="Accessibility">Geometry Designer</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Graphing Calculator</a></td>
  <td><a href="Accessibility">HexSphere</a></td>
  <td><a href="Accessibility">Layout to Layout Transitions</a></td>
  <td><a href="Accessibility">Logon Screen</a></td>
  <td><a href="Accessibility">Notepad</a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Particle Effects</a></td>
  <td><a href="Accessibility">Photo Store</a></td>
  <td><a href="Accessibility">SDK Viewer</a></td>
  <td><a href="Accessibility">Logon Screen</a></td>
  <td><a href="Accessibility">Slide Puzzle</a></td>
 </tr>
  <tr>
  <td><a href="Accessibility">Sticky Notes</a></td>
  <td><a href="Accessibility">WPF Photo Viewer</a></td>
  <td><a href="Accessibility">XAML to HTML Conversion</a></td>
    <td><a href="Accessibility">Video Viewer</a></td>
 </tr>
</table>

<table>
 <tr>
  <th colspan="5" align="left">Window Management</th>
 </tr>
<tr>
  <td><a href="Accessibility">Customized Window UI</a></td>
  <td><a href="Accessibility">Dialog Box Sample</a></td>
  <td><a href="Accessibility">Hide a Window Without Closing</a></td>
  <td><a href="Accessibility">MessageBox</a></td>
   <td><a href="Accessibility">Non-Rectangular Windows</a></td>
 </tr>
 <tr>
  <td><a href="Accessibility">Notification Icon</a></td>
  <td><a href="Accessibility">Save Window Placement State</a></td>
  <td><a href="Accessibility">Show a Window Without Activating</a></td>
  <td><a href="Accessibility">Window Activation and Deactivation</a></td>
  <td><a href="Accessibility">Window Sizing Order of Precedence</a></td>
 </tr>
  <tr>
   <td><a href="Accessibility">Wizard</a></td>
 </tr>
</table>

