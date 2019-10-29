---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Weather Service Simulation Via Dispatcher Sample"
---
# Weather Service Simulation Via Dispatcher Sample
This sample demonstrates how to keep a blocking operation from making an application unresponsive.

This sample simulates a weather service application which queries a remote resource. While the application is querying the remote resource, the UI should not be unresponsive. To solve this, a background thread retrieves the information. When the thread has completed its task, it pushes a job onto dispatcher of the UI thread.

A background thread is created by calling BeginInvoke on the delegate object. This is an asynchronous call that uses a threadpool thread, so there is no other work we need to do with regards to thread creation.

The delay of connecting to the remote service is simulated by putting the worker thread to sleep. When the job is finished it pushes another job onto the Dispatcher of the UI thread to update the UI with the weather forecast information.

This sample also demonstrates animation and storyboards. Most of the animation is defined in the XAML file, while the starting and stopping of the animation is handled in the code behind.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


