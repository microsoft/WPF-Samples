<#
.SYNOPSIS
    Builds and publishes the projects
.DESCRIPTION
    Builds and publishes the projects under artifacts\. Provides control over what TargetFramework to build against,
    which build engine to use (dotnet vs. MSBuild), and what architecture (x86, x64) to build. 
.PARAMETER Architecture
    The Platform architecture to build. 
    Valid values are AnyCPU, 'Any CPU', x86, Win32, x64, and amd64. 
    
    The default is $env:PROCESSOR_ARCHITECTURE, i.e., the architecture of the current
    OS. 
    
    amd64 is equivalent to x64 
    AnyCPU and 'Any CPU' and Win32 are equivalent to x86 
.PARAMETER Configuration 
    The build configuration
    
    Valid values are Debug, Release
    The default is Debug   
.PARAMETER TargetFramework
    The TargetFramework to build for. 
    
    This will always default to the most recent released version of .NET Core that supports WPF. 
    You can identify this by loking at global.json sdk.version property, or <TargetFramework> property 
    in project files im this repo. 
    
    Alternative TargetFramework can be supplied to build. Currently, netcoreapp3.1 (default),
    and net5.0 are supported.
.PARAMETER DryRun 
    When this switch is specified, the build is simulated, but the actual build is not run. 
.PARAMETER UseMsBuild 
    When this switch is specified, MSBuild is used as the build engine instead of dotnet.exe. 
    This requires that VS2019 be installed and avaialble on the local machine. 
    
    Some projects in this repo can be built only using MSBuild. 
.PARAMETER SdkVersionOverride
    Supply an SDK version to override global.json to use for building the samples
.PARAMETER AdditionalNuGetFeeds
    Supply Additonal NuGet feeds that may be required to build the samples. This is only
    used when $SdkVersionOverride is specified, and the SDK is as-yet unreleased and requires
    private NuGet feeds. 
.EXAMPLE
    build.ps1
    Builds the repo 
.EXAMPLE 
    build.ps1 -TargetFramework net5.0 -UseMsBuild
    
    Builds the repo using MSBuild for net5.0 TFM 
.EXAMPLE 
    build.ps1 -UseMsBuild -Platform x86 -Configuration Release
    
    Builds the repo using MSBuild for x86 platform & Release configuration
#>
[CmdletBinding(PositionalBinding=$false)]
param(
  [string] [Alias('a')][Alias('Platform')]
  [Parameter(HelpMessage='Architecture')]
  [ValidateSet('x86', 'x64', 'amd64', 'AnyCPU', 'Any CPU', 'Win32', IgnoreCase=$true)]
  $Architecture=$env:PROCESSOR_ARCHITECTURE, 

  [string] [Alias('c')]
  [Parameter(HelpMessage='Build Configuration')]
  [ValidateSet('Debug', 'Release', IgnoreCase=$true)]
  $Configuration='Debug',

  [string] [Alias('f')]
  [Parameter(HelpMessage='TargetFramework to match from global.json/altsdk section for an alternate SDK version')]
  [ValidateSet('', $null, 'netcoreapp3.1', 'net5.0', IgnoreCase=$true)]
  $TargetFramework='', 

  [switch]
  [Parameter(HelpMessage='Do not actually build - DryRun mode')]
  $DryRun,

  [Parameter(HelpMessage='Use MSBuild instead of dotnet.exe')]
  [switch]
  $UseMsBuild,

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

Function Fixup-Architecture {
  param(
      [string] [Alias('a')]
      [Parameter(HelpMessage='Architecture')]
      [ValidateSet('x86', 'x64', 'amd64', 'AnyCPU', 'Any CPU', 'Win32', IgnoreCase=$true)]
      $Architecture
  )

  if ($Architecture -ieq 'amd64') {
    return 'x64'
  }

  if ($Architecture -iin @('AnyCPU', 'Any CPU', 'Win32')) {
    return 'x86'
  }

  return $Architecture
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
        'net5.0'
    )

    $tfm1 = ('netcoreapp' + $SdkVersion.Substring(0,3)).Trim().ToLowerInvariant()
	$tfm2 = ('net' + $SdkVersion.Substring(0,3)).Trim().ToLowerInvariant()

    return IIf (($WellKnownTFMs -icontains $tfm1) -or ($WellKnownTFMs -icontains $tfm2)) $tfm ""
}

Function Identify-RID {
   param(
    [ValidateSet('x86', 'x64', 'amd64', 'AnyCPU', 'Any CPU', 'Win32', IgnoreCase=$true)]
    [string]$Architecture
   )

   switch($Architecture) 
   {
    {$_ -iin @('x86', 'AnyCPU', 'Any CPU', 'Win32')} {'win-x86'}
    {$_ -iin @('x64', 'amd64')} {'win-x64'}
   }
}

Function Get-VsWhere {
    if (-not (Get-PackageProvider -Name 'Chocolatey' -ErrorAction SilentlyContinue)) {
        Install-PackageProvider Chocolatey -Force -Scope CurrentUser | Out-Null
    }

    if (-not (Get-Package -ProviderName Chocolatey -Name vswhere -ErrorAction SilentlyContinue)) {
        Install-Package -ProviderName Chocolatey -Name vswhere -Force 
    }

    $vsWherePath = join-path (join-path "$env:SystemDrive" 'Chocolatey') 'bin'
    $VsWhere =  (Join-Path $vsWherePath 'vswhere.bat')

    if (-not (Test-Path $VsWhere)) {
        # Try again
        Uninstall-Package -ProviderName Chocolatey -Name vswhere -Force -ErrorAction SilentlyContinue
        Install-Package -ProviderName Chocolatey -Name vswhere -Force
        if (-not (Test-Path $VsWhere)) {
            # Let's try something else 
            $ChocolateyLib = Join-Path (Join-Path "$env:SystemDrive" 'Chocolatey') 'lib'
            $vsw = Get-ChildItem -Recurse -File -Path $ChocolateyLib -Include 'vswhere.exe' 
            if ($vsw) {
                $vsWherePath = $vsw[0].FullName
            }
        }
    }

    if (Test-Path $VsWhere) {
        $vsWherePath = (Get-Item $VsWhere).Directory.FullName
        Add-EnvPath -path $vsWherePath -prepend -emitAzPipelineLogCommand

        return $VsWhere
    }

    Write-Warning "Cannot find vswhere"
    return $null
}

Function Start-VsDevCmd {
    $VsWhere = Get-VsWhere
    $installationPath = iex "$VsWhere -prerelease -latest -property installationPath"
    if ($installationPath -and (test-path "$installationPath\Common7\Tools\vsdevcmd.bat")) {
        & "${env:COMSPEC}" /s /c "`"$installationPath\Common7\Tools\vsdevcmd.bat`" -no_logo && set" | foreach-object {
            $name, $value = $_ -split '=', 2
            Write-Verbose "Setting env:$name=$value"
            if ($name -and $value) {
                set-content env:\"$name" "$value"
            }
        }
    }
}

Function Get-LogFile {
    param(
        [bool]$UseMsBuild,
        [string]$SolutionFile,
        [string]$ArtifactsDir, 
        [string]$action='build'
    )

    $prefix = if ($UseMsBuild) { 'msbuild' } else { 'dotnet' } 
    
    $sln = [System.IO.Path]::GetFileNameWithoutExtension($SolutionFile)
    $name = $sln + '.' + $prefix
    if (-not [string]::IsNullOrEmpty($action)) {
        $name += '_' + $action
    }

    return Join-Path $ArtifactsDir ($name + '.binlog')
}

Function Get-BuildArgs {
    [CmdletBinding(PositionalBinding=$false)]
    param(
        [Parameter(Mandatory=$true)]
        [string]$LogFile,
        [Parameter(Mandatory=$true)]
        [string]$Platform,
        [Parameter(Mandatory=$true)]
        [string]$PublishDir,
        [Parameter(Mandatory=$true)]
        [string]$RuntimeIdentifier,
        [Parameter(Mandatory=$true)]
        [string]$TargetFramework,
        [switch]$UseMsBuild,
        [switch]$Restore
    )
    
    [string]$escapeparser = '--%'
    
    $Verbosity = 'quiet'
    $NodeReuse = 'false'
    $LangVersion = 'latest'
    
    if (-not $Restore) {
        $BuildArgs = "$escapeparser /bl:$LogFile /p:Platform=$Platform /p:LangVersion=$LangVersion /p:PublishDir=$PublishDir /p:UseCommonOutputDirectory=true /p:TargetFramework=$TargetFramework /p:RuntimeIdentifier=$RuntimeIdentifier /clp:Summary;Verbosity=$Verbosity /nr:$NodeReuse"
    } else {
        $BuildArgs = "$escapeparser /bl:$RestoreLogFile /p:TargetFramework=$TargetFramework /p:RuntimeIdentifier=$RuntimeIdentifier /clp:Summary;Verbosity=$Verbosity /nr:$NodeReuse"
    }
    
    if ($UseMsBuild) {
        $BuildArgs += " /m"
        if ($Restore) {
            $BuildArgs += " /t:restore"
        } else {
            $BuildArgs += " /t:publish"
        }
    }
    
    return $BuildArgs
}

Function Ensure-Directory {
    param(
        [string]$Directory
    )
    
    if (-not (Test-Path $Directory)) {
        New-Item -ItemType Directory -Path $Directory | Out-Null
    }
}

###################### Start of main script #########################

# Chocolatey requires min TLS1.2
# https://chocolatey.org/blog/remove-support-for-old-tls-versions
[Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12

if ($psISE) {
    $Eng =  (Get-Item (Split-Path $psISE.CurrentFile.FullPath -Parent)).FullName
} else {
    $Eng =  (Get-Item $PSScriptRoot).FullName
}

if ($UseMsBuild) {
    Start-VsDevCmd
}

[string]$escapeparser = '--%'

$Architecture = Fixup-Architecture $Architecture
$RepoRoot = (Get-Item $Eng).Parent.FullName
$GlobalJson = Join-Path $RepoRoot 'global.json' 
$NuGetConfig = Join-Path $RepoRoot 'NuGet.config'
$DotNetToolsDirectory = Join-Path $RepoRoot '.dotnet' 
$DotNet = Join-Path $DotNetToolsDirectory 'dotnet.exe'
$Solution = Join-Path $RepoRoot 'WPFSamples.sln'
$MsBuildOnlySolution = Join-Path $RepoRoot 'WPFSamples.msbuild.sln'

$EnsureGlobalJsonSdk = Join-Path $Eng 'EnsureGlobalJsonSdk.ps1'

if ([string]::IsNullOrEmpty($TargetFramework)) {
    if (-not $SdkVersionOverride) {
        $json = (Get-Content $GlobalJson | ConvertFrom-Json)
        $TargetFramework = Get-Tfm $json.sdk.version
    } else {
        $TargetFramework = Get-Tfm $SdkVersionOverride
    }

    if (-not $TargetFramework) {
        Write-Error "TargetFramework could not be identified" -ErrorAction Stop
    }

    Write-Verbose "TargetFramework identified: $TargetFramework"
}

$Artifacts = Join-path (Join-path (Join-Path (Join-Path $RepoRoot 'artifacts') $Configuration) $TargetFramework) $Architecture
$PublishDir = Join-Path $Artifacts 'publish'
$ArtifactsTemp = Join-Path $Artifacts 'Temp' 
$RuntimeIdentifier = Identify-RID $Architecture

Ensure-Directory $Artifacts
Ensure-Directory $ArtifactsTemp
Ensure-Directory $PublishDir

<# Save Global.json file #>
Copy-Item $GlobalJson -Destination (join-path $ArtifactsTemp 'global.json') -Force

if ($AdditionalNuGetFeeds) {
    # Save NuGet.config also 
    Copy-Item $NuGetConfig -Destination (join-path $ArtifactsTemp 'Nuget.config') -Force
}

Try {
    <# Run in local scope to inherit updates to $env:PATH #>
    . $EnsureGlobalJsonSdk -g $GlobalJson -i $DotNetToolsDirectory -a $Architecture -f $TargetFramework -AdditionalNuGetFeeds $AdditionalNuGetFeeds -SdkVersionOverride $SdkVersionOverride
    if (-not (Test-Path $DotNet)) {
        Write-Error "$DotNet not found - exiting"
        exit
    }

    Set-Location $RepoRoot 

    $LogFiles = @()
    $BuildCommands = @()
    
    if (-not $UseMsBuild) {
        $LogFile = Get-LogFile -UseMsBuild $false -SolutionFile $Solution -ArtifactsDir $Artifacts
        $RestoreLogFile = Get-LogFile -UseMsBuild $false -SolutionFile $Solution -ArtifactsDir $Artifacts -action 'restore' 

        $LogFiles += $RestoreLogFile
        $LogFiles += $LogFile

        $BuildArgs = Get-BuildArgs -LogFile $LogFile -Platform $Architecture -PublishDir $PublishDir -RuntimeIdentifier $RuntimeIdentifier -TargetFramework $TargetFramework
        $RestoreArgs = Get-BuildArgs -LogFile $RestoreLogFile -Platform $Architecture -PublishDir $PublishDir -RuntimeIdentifier $RuntimeIdentifier -Restore -TargetFramework $TargetFramework

        $BuildCmd = "$DotNet restore $RestoreArgs $Solution"
        Write-Verbose $BuildCmd
        if (-not $DryRun) {
            $BuildCommands += $BuildCmd
            Invoke-Expression $BuildCmd
        }

        $BuildCmd = "$DotNet publish $BuildArgs $Solution"
        Write-Verbose $BuildCmd
        if (-not $DryRun) {
            $BuildCommands += $BuildCmd        
            Invoke-Expression $BuildCmd
        }
    }

    
    if ($UseMsBuild) {       
        $LogFile = Get-LogFile -UseMsBuild $true -SolutionFile $Solution -ArtifactsDir $Artifacts
        $RestoreLogFile = Get-LogFile -UseMsBuild $true -SolutionFile $Solution -ArtifactsDir $Artifacts -action 'restore' 
               
        $BuildArgs = Get-BuildArgs -LogFile $LogFile -Platform $Architecture -PublishDir $PublishDir -RuntimeIdentifier $RuntimeIdentifier -UseMsBuild -TargetFramework $TargetFramework
        $RestoreArgs = Get-BuildArgs -LogFile $RestoreLogFile -Platform $Architecture -PublishDir $PublishDir -RuntimeIdentifier $RuntimeIdentifier -UseMsBuild -Restore -TargetFramework $TargetFramework

        $LogFile2 = Get-LogFile -UseMsBuild $true -SolutionFile $MsBuildOnlySolution -ArtifactsDir $Artifacts
        $RestoreLogFile2 = Get-LogFile -UseMsBuild $true -SolutionFile $MsBuildOnlySolution -ArtifactsDir $Artifacts -action 'restore' 
        
        $BuildArgs2 = Get-BuildArgs -LogFile $LogFile2 -Platform $Architecture -PublishDir $PublishDir -RuntimeIdentifier $RuntimeIdentifier -UseMsBuild -TargetFramework $TargetFramework
        $RestoreArgs2 = Get-BuildArgs -LogFile $RestoreLogFile2 -Platform $Architecture -PublishDir $PublishDir -RuntimeIdentifier $RuntimeIdentifier -UseMsBuild -Restore -TargetFramework $TargetFramework

        $LogFiles += $RestoreLogFile
        $LogFiles += $LogFile
        $LogFiles += $RestoreLogFile2
        $LogFiles += $LogFile2
        
        $BuildCmd = "msbuild $RestoreArgs $Solution"
        Write-Verbose $BuildCmd
        if (-not $DryRun) {
            $BuildCommands += $BuildCmd        
            Invoke-Expression $BuildCmd
        }

        $BuildCmd = "msbuild $BuildArgs $Solution"
        Write-Verbose $BuildCmd
        if (-not $DryRun) {
            $BuildCommands += $BuildCmd        
            Invoke-Expression $BuildCmd
        }

        $BuildCmd = "msbuild $RestoreArgs2 $MsBuildOnlySolution"
        Write-Verbose $BuildCmd
        if (-not $DryRun) {
            $BuildCommands += $BuildCmd        
            Invoke-Expression $BuildCmd
        }

        $BuildCmd = "msbuild $BuildArgs2 $MsBuildOnlySolution"
        Write-Verbose $BuildCmd
        if (-not $DryRun) {
            $BuildCommands += $BuildCmd        
            Invoke-Expression $BuildCmd
        }
    }
   


    Write-Host "Build Commands:"
    $BuildCommands.ForEach({
        Write-Host ("`t" + $_)
    })
    Write-Host "\n"
    
    Write-Host "Build Logs:"
    $LogFiles.ForEach({
        Write-Host ("`t" + $_)
    })
}
Finally {
    <# restore global.json #>
    Copy-Item (Join-Path $ArtifactsTemp 'global.json') $GlobalJson -Force

    if ($AdditionalNuGetFeeds) {
        # Also restore NuGet.config
        Copy-Item (Join-Path $ArtifactsTemp 'NuGet.config') $NuGetConfig -Force
    }
}
