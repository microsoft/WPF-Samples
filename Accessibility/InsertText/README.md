
# ValuePattern Insert Text Sample
This sample demonstrates how to use both Microsoft UI Automation and unmanaged methods to input text into standard Win32 text controls. The target text controls are a single-line text box, a multi-line text box and a rich text box.

Since the TextPattern control pattern does not support the setting of text values in a control, the Microsoft UI Automation sample code shows how to use either the ValuePattern control pattern or standard keyboard input to simulate text input depending on the target control type.

The sample defines two applications, a target and a client that operates against the target application.

## Remarks
The target application, InsertTextTarget.exe, should be automatically copied to the InsertText client folder when you build the sample and is started manually from the client. You may have to manually copy this file to the bin/debug folder of the client application if you receive an error stating that the file cannot be found.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 

