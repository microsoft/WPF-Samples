---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Load a Dropped File Sample"
---

# Load a Dropped File Sample
This sample will open and display the contents of a file dropped on the sample. Methods demonstrated in this sample include:
- GetData
- GetDataPresent

In this sample, event handlers monitor the PreviewDragOver and PreviewDrop events on a TextBox. When an object is dragged over the TextBox, a PreviewDragOver event handler checks to see if the object is a single file, and adjusts the DragDropEffects to indicate that a single file can be dropped, and anything else cannot. When a single file is dropped on the TextBox, a PreviewDrop event handler displays the file contents in the TextBox.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


