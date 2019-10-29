---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Data Formats Spy Sample"
---

# Data Formats Spy Sample
This sample displays the data formats contained in any item dropped on the sample. Methods demonstrated in this sample include:
- GetData
- GetDataPresent

In this sample, a simple event handler monitors the PreviewDrop event on a TextBox. When a drop is detected, the data formats present in the dropped data object are displayed using the GetFormats method.
The TextBox control includes native drop handling for text-based data; this sample overrides native drop support by setting the Handled property on the PreviewDrop event to true. Note that with routed events, tunneling events (by convention, prefixed with "Preview") first propagate down through the element tree, and then bubbling events propagate back up through the element tree. Marking the tunneling PreviewDrop event as handled causes the native drop handler to skip handling the corresponding bubbling Drop event.

In addition to listing data formats in a dropped item, the sample includes the ability to filter-out non-native data formats (that is, data formats that are available through automatic data-conversion), and labels each data format as "native" or "autoconvert". The GetDataPresent method is used to differentiate between native and auto-convertable data formats.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


