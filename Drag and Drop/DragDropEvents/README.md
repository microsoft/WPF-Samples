---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Drag-and-Drop Events Explorer Sample"        
description: "This sample explores how and when common drag and drop events fire. Events demonstrated in this sample include:"
---

# Drag-and-Drop Events Explorer Sample
This sample explores how and when common drag and drop events fire. Events demonstrated in this sample include:

###Bubbling
- DragEnter
- DragLeave
- DragOver
- Drop

###Tunneling
- PreviewDragEnter
- PreviewDragLeave
- PreviewDragOver
- PreviewDrop

The sample provides a UIElement (which happens to be a TextBlock) that is configured to be the target of a drag-and-drop operation; this drop-target support is enabled by setting the AllowDrop attribute to true on the target element, which can be a UIElement or a ContentElement.
Simple event handlers are attached to the UIElement for each of the drag-and-drop events listed in the table above. Whenever any of these events fires, a log entry is written to a log window included in the sample. The sample supports both brief and verbose event logging.
To observe when and in what order the events fire, drag any object into, over, or out of the bounds of the drop-target area, or drop any object on the drop-target area.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


