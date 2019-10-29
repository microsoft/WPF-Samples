---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Validate an Item in an ItemsControl Sample"
---

# Validate an Item in an ItemsControl Sample
This sample validates items in an ItemsControl by using the ItemsControl.ItemBindingGroup property. A BindingGroup contains multiple Binding objects and ValidationRule objects. When a ValidationRule that belongs to a BindingGroup runs, the Validate method can get the value of each binding in the BindingGroup. When the BindingExpressions is set to the ItemsControl.ItemBindingGroup property, each item container gets a BindingGroup that has the same ValidationRule objects as the ItemsControl.ItemBindingGroup, but the properties that describe the data in the bindings, such as Items and BindingExpressions, are specific to the data for each item in the ItemsControl.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


