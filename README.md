# WPF-Samples

This repo contains the samples that demonstrate the API usage patterns and popular features for the Windows Presentation Foundation in the .NET for Desktop. These samples were initially hosted on MSDN \( https://msdn.microsoft.com/ \), and we are gradually 
moving all the interesting WPF samples over to GitHub. All the samples have been retargeted to  [.NET 10.0](https://dotnet.microsoft.com/en-us/download).

You can also find an archive of samples targeting .NET 4.7.2 in the [netframework](https://github.com/microsoft/WPF-Samples/tree/netframework) branch.

The samples in this repo are generally about illustrating specific concepts and may go against accessibility best practices. However, the team has spent some time illustrating accessibility best practices in a subset of these samples.

* [ExpenseItIntro](https://github.com/microsoft/WPF-Samples/tree/main/Getting%20Started/WalkthroughFirstWPFApp/csharp)
* [ExpenseItDemo](https://github.com/microsoft/WPF-Samples/tree/main/Sample%20Applications/ExpenseIt/ExpenseItDemo)
* [DataBindingDemo](https://github.com/microsoft/WPF-Samples/tree/main/Sample%20Applications/DataBindingDemo)
* [CustomComboBox](https://github.com/microsoft/WPF-Samples/tree/main/Sample%20Applications/CustomComboBox)
* [EditingExaminerDemo](https://github.com/microsoft/WPF-Samples/tree/main/Sample%20Applications/EditingExaminerDemo)

## License
Unless otherwise mentioned, the samples are released under the [MIT license](https://github.com/Microsoft/WPF-Samples/blob/main/LICENSE)

## Help us improve our samples
Help us improve out samples by sending us a pull-request or opening a [GitHub Issue](https://github.com/Microsoft/WPF-Samples/issues)

## WPF development

### For current .NET version (.NET 10) - main branch

These samples require Visual Studio 2026(v18.1) to build, test, and deploy, and also require the most recent .NET 10 SDK.

   [Get a free copy of Visual Studio 2026 Community Edition](https://visualstudio.microsoft.com/downloads/)

   [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

### For older .NET versions - release/x.x branches

These samples require Visual Studio 2022(v17.7) to build, test, and deploy, and also require the .NET 9 SDK.

   [Get a free copy of Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/older-downloads/)

   [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) <br>
   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

WPF on .NET has been open-sourced, and is now available on [Github](https://github.com/dotnet/wpf)


## Using the samples

To use the samples with Git, clone the WPF-Samples repository with 'git clone https://github.com/microsoft/WPF-Samples'

After cloning the WPF-Samples respository, there will be two solution files in the root directory: WPF-Samples.sln and WPF-Samples.msbuild.sln 

* To build the samples, open one of the solution files in Visual Studio 2022 and build the solution.
* Alternatively, navigate to the directory of a sample and build with 'dotnet build' or 'msbuild' specifying the target project file. 
* WPF-Samples.msbuild.sln contains projects that can be built only with `msbuild` or Visual Studio, and will not compile with `dotnet build`. These projects contain C++ code, for which there is no support in `dotnet build`

The easiest way to use these samples without using Git is to download the zip file containing the current version (using the link below or by clicking the "Download ZIP" button on the [repo](https://github.com/microsoft/WPF-Samples?tab=readme-ov-file) page). You can then unzip the entire archive and use the samples in Visual Studio 2022.

   [Download the samples ZIP](../../archive/main.zip)

   **Notes:** 
   * Before you unzip the archive, right-click it, select Properties, and then select Unblock.
   * Most samples should work independently
   * By default, all the samples target .NET 10.0. (Installers for the .NET 10 SDK can be found at <https://dotnet.microsoft.com/en-us/download>)

