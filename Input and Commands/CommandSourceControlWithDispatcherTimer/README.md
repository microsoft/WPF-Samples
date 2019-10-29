---
languages:
- csharp
products:
- windows-wpf
page_type: sample
name: "Disable Command Source via Dispatcher Timer Sample"
---
# Disable Command Source via Dispatcher Timer Sample
This sample shows how to enable and disable a command source via a DispatcherTimer.

Command sources, such as the MenuItem class and the Button class, listen to the CanExecuteChanged event on the RoutedCommand they are attached to in order to determine when they need to query the command to see if the command can execute on the current command target. Command sources will typically disable themselves if the command cannot execute and enable themselves if the command can execute, such as when a MenuItem gray's itself out when the command cannot execute.

The CommandManager notifies the RoutedCommand via the RequerySuggested event that conditions have changed with the command target. The RoutedCommand raises the CanExecuteChanged event which the command source listens to. Normally, this notification mechanism is adequate, but there are some situations where the CommandManager is unaware that the conditions have changed on the command target and thus the RequerySuggested event is never raised and the command source never queries the RoutedCommand. In these situations, the CommandManager can be forced to raise the RequerySuggested event by calling InvalidateRequerySuggested.

This sample creates a RoutedCommand that can be executed only when the seconds in the current time are greater than a target value. A DispatcherTimer is created that calls InvalidateRequerySuggested every second. This insures that the command source will receive the CanExecuteChanged event so that it can call the CanExecute method on the command.

## Build the sample
The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in [Visual Studio 2019](https://www.visualstudio.com/wpf-vs).

### Deploying the sample
- Select Build > Deploy Solution. 

### Deploying and running the sample
- To debug the sample and then run it, press F5 or select Debug >  Start Debugging. To run the sample without debugging, press Ctrl+F5 or selectDebug > Start Without Debugging. 


