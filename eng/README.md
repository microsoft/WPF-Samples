# Build Script

```none
NAME
    build.ps1

SYNOPSIS
    Builds and publishes the projects


SYNTAX
    build.ps1 [-Architecture <String>] [-Configuration <String>] [-TargetFramework <String>] [-DryRun] [-UseMsBuild] [<CommonParameters>]


DESCRIPTION
    Builds and publishes the projects under artifacts\. Provides control over what TargetFramework to build against,
    which build engine to use (dotnet vs. MSBuild), and what architecture (x86, x64) to build.


PARAMETERS
    -Architecture <String>
        The Platform architecture to build.
        Valid values are AnyCPU, 'Any CPU', x86, Win32, x64, and amd64.

        The default is $env:PROCESSOR_ARCHITECTURE, i.e., the architecture of the current
        OS.

        amd64 is equivalent to x64
        AnyCPU and 'Any CPU' and Win32 are equivalent to x86

    -Configuration <String>
        The build configuration

        Valid values are Debug, Release
        The default is Debug

    -TargetFramework <String>
        The TargetFramework to build for.

        This will always default to the most recent released version of .NET Core that supports WPF.
        You can identify this by loking at global.json sdk.version property, or <TargetFramework> property
        in project files im this repo.

        Alternative TargetFramework can be supplied to build. Currently, netcoreapp3.1 (default),
        and net5.0 are supported.

    -DryRun [<SwitchParameter>]
        When this switch is specified, the build is simulated, but the actual build is not run.

    -UseMsBuild [<SwitchParameter>]
        When this switch is specified, MSBuild is used as the build engine instead of dotnet.exe.
        This requires that VS2019 be installed and avaialble on the local machine.

        Some projects in this repo can be built only using MSBuild.

    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216).

    -------------------------- EXAMPLE 1 --------------------------
    PS C:\>build.ps1

    Builds the repo
    -------------------------- EXAMPLE 2 --------------------------
    PS C:\>build.ps1 -TargetFramework net5.0 -UseMsBuild

    Builds the repo using MSBuild for net5.0 TFM
    -------------------------- EXAMPLE 3 --------------------------
    PS C:\>build.ps1 -UseMsBuild -Platform x86 -Configuration Release

    Builds the repo using MSBuild for x86 platform & Release configuration
```
