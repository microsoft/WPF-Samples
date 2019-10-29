---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "FetchTimer Sample"
---

# FetchTimer Sample

This sample demonstrates some aspects of caching (prefetching) in Microsoft UI Automation and supplies comparative performance values.
After launching the sample, leave it in the foreground while moving the cursor over various elements on the desktop. Press Alt+G to retrieve the AutomationElement at the cursor. The sample performs three tests: fetching the element without caching, fetching it with caching, and updating the cache.
You can set the scope and caching mode of element retrieval by using the checkboxes.

## Remarks
The sample is designed to demonstrate the use of CacheRequest and related classes and methods, but does not necessarily reflect a real-world implementation of UI Automation caching as a whole.

## Build the sample

The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample

- Select Build > Deploy Solution. 

### Deploying and running the sample

- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


