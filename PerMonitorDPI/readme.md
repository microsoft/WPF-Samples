# Per Monitor DPI
Developer Guide

## Introduction
This developer guide provides developer info on how to test your WPF application with our Per Monitor DPI feature released in .NET 4.6.2 along with the Windows 10 Anniversary Update.

## Operating System Prerequisites
In order to enable Per Monitor DPI Awareness in your app, you should be running Windows 10 Anniversary Update or higher. 

## Enabling Per Monitor DPI for your app

### 1)	Turn on Windows level Per monitor DPI awareness in app.manifest
WPF apps are System DPI aware by default, and need to declare themselves to be Per Monitor DPI aware via an app.manifest file. To add an app.manifest:
-	Right click on your project folder, click Add -> New Item
-	Click on General in the left pane, and chose Application Manifest File.
-	Uncomment the following snippet of xml:

```xml
<application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <!-- The combination of below two tags have the following effect : 
      1) Per-Monitor for >= Windows 10 Anniversary Update
      2) System < Windows 10 Anniversary Update -->
      <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings"> PerMonitor</dpiAwareness>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    </windowsSettings>
  </application>
```

### 2)	Turn on WPF Feature in App.Config
If your app is targeting .NET Framework 4.6.2, you can ignore this step. Otherwise,  you will also need to add the following snippet to app.config (inside <configuration> element):
```xml
<runtime>
    <AppContextSwitchOverrides value = "Switch.System.Windows.DoNotScaleForDpiChanges=false"/>
  </runtime>
```

- Yes, set it to false. Double negative FTW!
-	Note: the AppContextSwitchOverrides cannot be set twice. If your application already has one set, you must semicolon delimit this switch inside the value attribute.

## Test Environment
Run on a PC with 2 monitors, Windows 10 Anniversary Update or higher.
Set the 2 monitors DPI so that they are different (DPI of 100 on one and 150 on the other).

## Once Enabled
Doing the above steps will enable any WPF app, with pure WPF content, to work seamlessly as a Per Monitor DPI Aware app.
There are several scenarios that will take some additional coding in your application with newly available APIs:

### WPF Apps
 - **Images** - often it is best to have different scales of images for different DPIs – see PerMonitorDpi\ImageScaling sample on github.com/WpfSamples.
We’re considering doing more work in the image scaling space, before release, to improve this scenario.
 - **Hosted Hwnds or WindowsForms controls** - if your application uses HwndHost or WindowsFormsHost, you’ll want to listen to DpiChanged on that control and adapt your scaling as appropriate - see PerMonitorDPI\WinFormsHost sample on github.com/WpfSamples 
 - **RenderTargetBitmap** – If you are rendering a set of visuals, which are not part of the main visual tree, via RenderTargetBitmap – WPF should be told the target DPI for that set of visuals. Call VisualTreeHelper.SetRootDpi(rootVisual, DpiScale) before calling Measure and/or RenderTargetBitmap.
 - **TextFormatting API calls** – if your application uses any System.Windows.Media.-TextFormatting APIs, you’ll need to pass in your target DPI into new constructor overrides or properties. When DPI changes, you’ll need to force a re-render of your text – see PerMonitorDpi\TextFormatting and PerMonitorDpi\FormattedTextExample samples on github.com/WpfSamples
 
### HWND Apps (NOT SUPPORTED)
The scenario where a Win32 hosts WPF via HwndSource does not currently support Per Monitor DPI. DPI changed messages (WM_DPICHANGED) are only sent to the top level window, so WPF is not informed of a change. We’ll consider doing work in the future here, but it likely will require additional features from the Windows team.

### WindowsForms Apps (NOT SUPPORTED)
The scenario where a WindowsForms app hosts WPF via ElementHost does not currently support Per Monitor DPI. We’ll consider doing work in the future here, but it likely will require additional features from the Windows team.

## Using new APIs requires Reference Assembly Install Step
If you need to do more advanced coding, involving this feature, you’ll need to make sure that your app is targeting .NET 4.6.2.

## API Details
### Events and Virtuals
 - **`Window.DpiChanged` event**: 
    as a window is moved to a different DPI monitor, this event will fire.
 - **`Image.DpiChanged` event**: 
    as a window is moved to a different DPI monitor, this event will fire on all Images inside of that window. It is a routed event, so you could listen to it centrally, on the root element of each xaml file.
 - **`WindowsFormHost.DpiChanged` event**: 
    as a window is moved to a different DPI monitor, this event will fire on all WindowsFormHosts inside of that window. It is a routed event, so you could listen to it centrally, on the root element of each xaml file.
 - **`HwndHost.DpiChanged` event**: 
    as a window is moved to a different DPI monitor, this event will fire on all HwndHosts inside of that window. It is a routed event, so you could listen to it centrally, on the root element of each xaml file.
 - **`HwndSource.DpiChange`d event**: 
    Most developers will be using the HwndSource that Window provides for them. If your application uses a HwndSource directly, you may find the following API useful:
    DpiChanged event is fired on the HwndSource. 
    Note: During your event handler, if you mark this event as handled, WPF will not scale the UI for you, or notify the visuals of any DPI change.
 - **`Visual.OnDpiChanged` virtual method**:
    Each visual has a virtual method which can be overridden in order to listen to DPI change notifications on each visual. If your control needs to understand DPI for your rendering, you should override this method and behave appropriately.
```cs
protected virtual void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
```
 
## VisualTreeHelper APIs

### 1)	VisualTreeHelper.GetDpi
```cs
/// <summary>
/// Gets the current DPI at which this visual is rendered.
/// </summary>
public static DpiScale GetDpi(Visual visual)
```
 
### 2)	VisualTreeHelper.SetRootDpi
```cs
/// <summary>
/// This method updates the DPI information of a visual.
/// It can only be called on a Visual with no parent.
/// </summary>
public static void SetRootDpi(Visual visual, DpiScale dpiInfo)
```

### TextFormatting Scenarios
If you use low level Text Formatting APIs and use custom TextSource, TextRun, GlyphRun, FormattedText to draw text in your app, then you need to use the following guidelines:
 - All text should be re-rendered when DPI is changed on the owner window.
 - TextSource, TextRunProperties, GlyphRun and FormattedText have a new PixelsPerDip property, which should be updated to reflect the DPI at which the text should be rendered. For guidelines on how to do this effectively, see PerMonitorDPI\FormattedTextExample and PerMonitorDPI\TextFormatting samples on github.com/WpfSamples.

### New DPI Awareness Manifest Setting
It has always been recommended that applications specify their DPI awareness using the application manifest rather than using the equivalent programmatic APIs.  Use of the manifest forces the awareness to be fixed before any application code runs and avoids any ordering issues with respect to what code runs first at process startup (e.g. the DllMain in any statically referenced DLL will run before the application’s main). 
A new manifest element, <dpiAwareness>, has been added to the Windows 10 Anniversary Update.  This element will supersede the older <dpiAware> element.  The old element will still be supported for backwards compatibility.  The new element allows you to indicate that you want different DPI awareness behavior for the current version of Windows than should be used on downlevel systems. 
For example, here is a manifest containing both the old and new DPI elements.  This manifest will result in per-monitor DPI awareness on Win 10 Anniversary Update, and system DPI awareness for all earlier versions of windows.  


```xml
<assembly xmlns="urn:schemas-microsoft-com:asm.v1" manifestVersion="1.0">
...
    <application xmlns="urn:schemas-microsoft-com:asm.v3">
        <windowsSettings>
            <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
     <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">
         PerMonitor   
     </dpiAwareness>
        </windowsSettings>
    </application>
...
</assembly>
```

Characteristics of the new element:
 - The set of valid names are PerMonitor, System, and Unaware.  Names are case insensitive, and whitespace is ignored.  
 - The system will now completely ignore the older element (<dpiAware>) if both are present.  The older element is used alongside the newer one only for down-level support.  
 - As with the older element, the new element is only meaningful within the manifest of an exe.  It has no significance within a dll.
 - Note that the new element uses a different XML namespace than the old one.
Note that nothing about the older <dpiAware> element changes in Windows 10 Anniversary Update, with the exception that the system now ignores it if the newer element is present.  If the newer element is not present, then the older element is used and it has the same semantics and behavior as in the previous versions of Windows.

## Communicating Feedback
Please let us know how this is working for you, or if you find any problems with the feature.

Email: *wpfteam@microsoft.com*

We will update samples and this document as needed, look for updates.
