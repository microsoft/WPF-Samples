---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Highlighter Sample"
---

# Highlighter Sample
This sample demonstrates how to keep track of focus changes so that focused elements can be highlighted on the screen. The highlight is a simple colored rectangle, but it could be a magnifier window or some other tool to make the focused element more accessible.
For convenience and simplicity, the sample runs in its own window. A real-world application might run in the background.
Sometimes focus-changed events occur in rapid succession: for example, when the user rapidly moves the cursor down a menu. Also, when a complex element such as a list box receives the focus, generally two events are raised: one for the container receiving the focus, and one for the focused item within the container. To avoid flicker (rapid drawing and erasing of the highlight), the sample uses a timer. The timer is started, or restarted, whenever an event is received. Only when the timer reaches its interval is the highlight redrawn. Thus the response to an event becomes "pending" when the event occurs and is discarded if another event occurs before the timer interval has elapsed.

You can experiment with different timer intervals by using the slider.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


