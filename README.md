# WPF-Samples

### **This branch is an archive is is no longer in active development.**


This repo contains the samples that demonstrate the API usage patterns and popular features for the Windows Presentation Foundation in the .NET Framework for Desktop. These samples were initially hosted on [MSDN](https://msdn.microsoft.com/en-us/library/vstudio/ms771633.aspx), and we are gradually 
moving all the interesting WPF samples over to GitHub. All the samples have been retargeted to [.NET 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework-runtime/net472). 

For additional WPF samples, see [WPF Samples](https://msdn.microsoft.com/en-us/library/vstudio/ms771633.aspx).

Please note that the documentation on the repo is still being updated, so all links might not point to the right location.

## License
Unless otherwise mentioned, the samples are released under the [MIT license](https://github.com/Microsoft/WPF-Samples/blob/master/LICENSE)

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/WPF-Samples/issues)

Questions: mail wpfteam@microsoft.com

## WPF development

These samples require Visual Studio 2019 to build, test, and deploy. 

   [Get a free copy of Visual Studio 2019 Community Edition with support for building WPF apps](https://www.visualstudio.com/wpf-vs)

## Using the samples

To use the samples with Git, clone the WPF-Samples repository with 'git clone https://github.com/microsoft/WPF-Samples'

After cloning the WPF-Samples respository, there will be two solution files in the root directory: WPF-Samples.sln (targeting .NET Framework 4.7.2) 

* To build the samples, open one of the solution files in Visual Studio 2019 and build the solution.
* Alternatively, navigate to the directory of a sample and build with 'msbuild' specifying the target project file. 

The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the repo page). You can then unzip the entire archive and use the samples in Visual Studio 2019.

   [Download the samples ZIP](../../archive/master.zip)

   **Notes:** 
   * Before you unzip the archive, right-click it, select Properties, and then select Unblock.
   * Most samples should work independently
   * By default, all the .NET Framework samples target .NET 4.7.2, you can change this in the Project->Properties page in Visual Studio.

For more info about the programming models, platforms, languages, and APIs demonstrated in these samples, please refer to the guidance  available in  [MSDN](https://msdn.microsoft.com/en-us/library/ms754130.aspx). These samples are provided as-is in order to indicate or demonstrate the functionality of the programming models and feature APIs for WPF.
