---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "TrackFocus Sample"
---

# TrackFocus Sample
This sample tracks the input focus on the desktop and displays information about focus changes, using Microsoft UI Automation. This is a simple console application that might be used as a starting-point for an application that uses UI Automation to track events on the desktop.
The program announces when the input focus changes. If the focus moves to a different application window, the caption of the window is announced. If the focus moves within an application window, the type and name of the control being read are announced.
To know when the focus switches from one application to another, the program keeps a list of the runtime identifiers of all open top-level windows. In response to each focus-changed event, a TreeWalker is used to find the parent window, and that window is compared with the last window that had focus.

The program subscribes to three event types:

- Structure changed. The only event of interest is the addition of a new top-level window.
- Focus changed. All events are captured.
- Window closed. When a top-level window is closed, its runtime ID is removed from the list.

For simplicity, no caching is done. A full-scale application would likely cache all immediate children of an application window as soon as that window received focus.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


