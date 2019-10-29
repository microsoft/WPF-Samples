---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "PropertyChanged and CoerceValue Callbacks Sample"
---

# PropertyChanged and CoerceValue Callbacks Sample
This example illustrates how to implement callbacks for dependency properties. The dependencies illustrated here are deliberately complex, and illustrate some of the challenges that you will face if you create complex dependencies and also attempt to update constrained values as part of a user interface. It is deliberately not intended as a best practice for all aspects of how to implement callbacks, and is more intended to show the possible complexities. It features two different templates that present the same control information in two different representations.

You should experiment with the sample and make your own choices about how you could represent this same Minimum/Current/Maximum relationship by using fewer constraints in favor of hard-coded values, or not exposing as many aspects of the properties to user control.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


