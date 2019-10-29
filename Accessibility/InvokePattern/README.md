
# InvokePattern, ExpandCollapsePattern, and TogglePattern Sample
This sample demonstrates the Microsoft UI Automation ExpandCollapsePattern, InvokePattern, and TogglePattern control pattern classes.

The sample illustrates how identical controls may provide different control pattern support depending on their location within the control view of the Microsoft UI Automation tree and their function within an application. Specifically, it demonstrates how a menu or treeview element may support the InvokePattern control pattern if the element is a leaf-node and initiates a single, unambiguous action.

Two applications—a Windows Presentation Foundation (WPF) target application containing a variety of TreeView controls used as UI Automation providers and a WPF UI Automation client that operates against this target application—are created by the sample. The client uses the ExpandCollapsePattern, InvokePattern, and TogglePattern control patterns to interact with the controls in the target.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 

