---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Support for Touch Keyboard Notifications and Occlusion Handling in WPF"
---
# Support for Touch Keyboard Notifications and Occlusion Handling in WPF

## Overview
In .NET 4.6.2 or above on Windows 10 Anniversary Update or above, WPF supports automatic display of the Windows touch keyboard (tiptsf).  This functionality occurs automatically when a 
control that the touch keyboard could be needed for receives keyboard focus.  In order for this behavior to be enabled, the application must be running in Windows 10 tablet mode or the 
touch keyboard configured to be displayed outside of tablet mode.  WPF respects the system settings for this and leaves the decision to display entirely up to the touch keyboard itself. 
An issue that may occur due to this feature is that the touch keyboard may occlude a portion of a WPF application's window(s) when it is shown.  To help alleviate this, the WPF team is
providing a set of utility classes that can be used to both access the event notifications that the keyboard is showing or hiding and some examples of how an application can handle these
events in a way that improves usability.

## Included Classes:

### TouchKeyboardEventManager:
This class provides a way (AddHandlers/RemoveHandlers) to add and remove event handlers for the touch keyboard showing and hiding events.  These events will provide the window being 
occluded by the touch keyboard as well as a rectangle that describes the occlusion in screen coordinates.  Please see the actual code for details on how this is accomplished.

### TouchKeyboardAwareDecorator:
This class provides a decorator that can be used on a WPF window to alter the rendered content to account for touch keyboard occlusion.  To use this, merely wrap the content you wish
to react to the touch keyboard in this decorator.  The decorator will then attempt to shift its content if it is occluded by the touch keyboard.  Please see comments on the class for
exceptional situations.

## BUILD NOTES:
> You will need the Windows 10 SDK installed.  If you have not installed it to its default location in "C:\Program Files (x86)\Windows Kits\10", you will have to update the reference
> to Windows.winmd in the project file to point to your installed SDK directory.
