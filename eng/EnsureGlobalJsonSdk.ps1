﻿#
# Script.ps1
#

[CmdletBinding(PositionalBinding=$false)]
param(
  [string][Alias('g')]
  [Parameter(HelpMessage='Path to global.json')]
  $globalJson, 

  [string] [Alias('t')]
  [Parameter(HelpMessage='Installation Path')]
  $installPath, 

  [string] [Alias('a')]
  [Parameter(HelpMessage='Architecture')]
  $architecture=$env:PROCESSOR_ARCHITECTURE, 

  [string] [Alias('f')]
  [Parameter(HelpMessage='TargetFramework to match from global.json/altsdk section for an alternate SDK version')]
  [ValidateSet('', $null, 'netcoreapp3.1', 'net6.0-windows', 'net7.0-windows', 'net8.0-windows', IgnoreCase=$true)]
  $TargetFramework='',

  [string]
  [Parameter(HelpMessage='SDK Override Version')]
  $SdkVersionOverride = $null,

  [string[]]
  [Parameter(HelpMessage='Additional NuGet Feeds for Overridden SDK Version')]
  $AdditionalNuGetFeeds = $null
)

Function IIf($If, $Then, $Else) {
    If ($If -IsNot "Boolean") {$_ = $If}
    If ($If) {If ($Then -is "ScriptBlock") {&$Then} Else {$Then}}
    Else {If ($Else -is "ScriptBlock") {&$Else} Else {$Else}}
}

Function Get-Tfm {
    param(
        [string]$SdkVersion
    )

    $WellKnownTFMs = @(
        'netcoreapp1.0',
        'netcoreapp1.1',
        'netcoreapp2.0',
        'netcoreapp2.1',
        'netcoreapp2.2',
        'netcoreapp3.0',
        'netcoreapp3.1',
        'net6.0-windows',
        'net7.0-windows',
        'net8.0-windows'
    )

    $tfm1 = ('netcoreapp' + $SdkVersion.Substring(0,3)).Trim().ToLowerInvariant()
	$tfm2 = ('net' + $SdkVersion.Substring(0,3)).Trim().ToLowerInvariant()

    return IIf (($WellKnownTFMs -icontains $tfm1) -or ($WellKnownTFMs -icontains $tfm2)) $tfm ""
}

function Add-EnvPath {
    param(
        [string]$path, 
        [switch]$prepend = $false,
        [switch]$emitAzPipelineLogCommand = $false 
    )

    
    [string]$envPath = $env:Path.ToLowerInvariant()
    if (-not $path.EndsWith('\')) {
        $path += '\'
    }

    <# 
        Remove any previous instance of $path from 
        $env:Path 

        Try from longest to shortest possible combination
    #>
    if ($envPath.Contains("$path;")) {                                <# path\to\dir\; #>
        $envPath = $envPath.Replace("$path;", '')
    } elseif ($envPath.Contains($path)) {                             <# path\to\dir\  #>
        $envPath = $envPath.Replace($path, '')
    } elseif ($path.Contains($path.TrimEnd('\') + ";")) {             <# path\to\dir;  #>
        $envPath = $envPath.Replace($path.TrimEnd('\') + ";", '')
    } elseif ($path.Contains($path.TrimEnd('\'))) {                   <# path\to\dir   #>
        $envPath = $envPath.Replace($path.TrimEnd('\'), '')
    }

    if ($prepend) {
        $envPath = "$path;" + $envPath
    } else {
        $envPath += ";$path"
    }

    $env:Path = $envPath

    if ($emitAzPipelineLogCommand) {
        if ($prepend) {
            Write-Host "##vso[task.prependpath]$path"
        } else {
            Write-Host "##vso[task.setvariable variable=PATH]$envPath"
        }
    }

    Write-Verbose "Added $path to PATH variable"
}

################### Main Script #####################

if (Test-Path $globalJson) {
    $json = Get-Content $globalJson | ConvertFrom-Json

    <# 
        If the $TargetFramework is no different from
        that of the version supplied in global.json/sdk.version, don't
        use it for further decisions.
    #>
    $script:EffectiveTargetFramework = $TargetFramework
    if ($script:EffectiveTargetFramework -ieq (Get-Tfm -SdkVersion $json.sdk.version)) {
        $script:EffectiveTargetFramework = ''
    }

    if (-not $script:EffectiveTargetFramework) {
        $sdk_version = $json.sdk.version
    } else {
        Write-Verbose "Alternate TargetFramework requested - reading from altsdk section in global.json"
        $sdk_version = $json.altsdk.$script:EffectiveTargetFramework
        if ($sdk_version) {
            Write-Verbose "Alternate SDK Version: $sdk_version"
        } else {
            Write-Verbose "Alternate SDK Version not found"
        }
    }

    if ($SdkVersionOverride) {
        Write-Verbose "SdkVersionOverride=$SdkVersionOverride - overriding"
        $sdk_version = $SdkVersionOverride
    }


    if ($sdk_version) {
        $dotnet_install = "$env:TEMP\dotnet-install.ps1"

        $AllProtocols = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'
        [System.Net.ServicePointManager]::SecurityProtocol = $AllProtocols

        Invoke-WebRequest https://dot.net/v1/dotnet-install.ps1 -OutFile $dotnet_install
        Write-Verbose "Downloaded dotnet-install.ps1 to $dotnet_install"

        .$dotnet_install -Channel $channel -Version $sdk_version -Architecture $architecture -InstallDir $installPath
        Write-Verbose "Installed SDK Version=$sdk_version Channel=$channel Architecture=$architecture to $installPath"
        
        Add-EnvPath -path $installPath -prepend -emitAzPipelineLogCommand

        <#
           Emit the right signals to Azure Pipelines about 
           updating env vars
        #>
        Write-Host "##vso[task.setvariable variable=DOTNET_MULTILEVEL_LOOKUP]0"
        Write-Host "##vso[task.setvariable variable=DOTNET_SKIP_FIRST_TIME_EXPERIENCE]1"

        $env:DOTNET_MULTILEVEL_LOOKUP = 0
        $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1

        <# 
            If $script:EffectiveTargetFramework is present, then an alternate SDK was requested.
            This means that the build requires an updated global.json as well. 

            This is a destructive change. The global.json should be restored by doing this after a build:
                    git checkout global.json
        #>
        if ($script:EffectiveTargetFramework -or $SdkVersionOverride) {
            <# 
                We would like to use '$dotnet new globaljson --sdk-version $sdk_version --force', but,...
                ..when global.json has a sdk.version that's different from the sdk installed, dotnet.exe complains:

                "A compatible installed .NET Core SDK for global.json version [3.0.100] from [path/to/global.json] was not found"

                So let's update global.json using JSON API's. 
            #>
            $json.sdk.version = $sdk_version
            $json | ConvertTo-Json | Set-Content $globalJson -Force
            Write-Verbose "global.json updated"
            Write-Verbose (Get-Content $globalJson -Raw)
        }

        if ($SdkVersionOverride -and $AdditionalNuGetFeeds) {
            $nugetConfig = Join-Path (Get-Item $globalJson).Directory.FullName 'NuGet.config'
            if (Test-Path $nugetConfig) {
                Write-Verbose "Additional Feeds requested - Updating NuGet.config"
                Write-Verbose "Feeds are..."
                $AdditionalNuGetFeeds | %{
                    $feed = $_
                    Write-Verbose "`t$feed"
                }

                # Download NuGet.exe 
                $NuGetExe = join-path $env:TEMP 'nuget.exe'
                if (-not (Test-Path $NuGetExe -PathType Leaf)) {
                    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile $NuGetExe
                    Write-Verbose "Downloaded nuget.exe to $NuGetExe"
                }

                for ($i = 0; $i -lt $AdditionalNuGetFeeds.Count; $i++) {
                    & $NuGetExe Sources Add -Name "transient-delete-$i" -Source $AdditionalNuGetFeeds[$i]
                }
            }
        }
    }
}
