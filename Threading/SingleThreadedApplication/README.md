---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Single Threaded Application With Long Running Calculation Sample"
---
# Single Threaded Application With Long Running Calculation Sample
This sample demonstrates how to keep the UI from becoming non-responsive in single threaded application which performs a long operation.
This sample has a Button which when clicked starts calculating prime numbers. This calculation is theoretically unending, so if the event handler simple went into a loop and started calculating prime numbers, the call would never return and the UI thread would freeze.
To avoid this, the sample queues an asynchronous job onto the Dispatcher of the UI thread by calling BeginInvoke. The job that is pushed on the queue processes one number when the UI thread is in the SystemIdle state and pushes another call on the dispatcher to process the next number the next time the UI thread is idle.
This insures that the UI thread will always take precedence over calculating the next number and it keeps the UI from hanging.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


