[CmdletBinding(PositionalBinding=$false)]
param(
  [string] [Alias('a')]
  [Parameter(HelpMessage='Architecture')]
  [ValidateSet('x86', 'x64', 'amd64', 'AnyCPU', 'Any CPU', 'Win32', IgnoreCase=$true)]
  $Architecture=$env:PROCESSOR_ARCHITECTURE, 

  [string] [Alias('c')]
  [Parameter(HelpMessage='Build Configuration')]
  [ValidateSet('Debug', 'Release', IgnoreCase=$true)]
  $Configuration='Debug',

  [string] [Alias('f')]
  [Parameter(HelpMessage='TargetFramework to match from global.json/altsdk section for an alternate SDK version')]
  [ValidateSet('', $null, 'netcoreapp3.0', 'netcoreapp3.1', 'netcoreapp5.0', IgnoreCase=$true)]
  $TargetFramework='', 

  [switch]
  [Parameter(HelpMessage='Do not actually build - DryRun mode')]
  $DryRun,

  [Parameter(HelpMessage='Use MSBuild instead of dotnet.exe')]
  [switch]
  $UseMsBuild
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
        'netcoreapp5.0'
    )

    $tfm = ('netcoreapp' + $SdkVersion.Substring(0,3)).Trim().ToLowerInvariant()

    return IIf ($WellKnownTFMs -icontains $tfm) $tfm ""
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
        Uninstall-Package -ProviderName Chocolatey -Name vswhere -Force
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
            set-content env:\"$name" $value
        }
    }
}

Function Get-LogFile {
    param(
        [bool]$UseMsBuild,
        [string]$SolutionFile,
        [string]$ArtifactsDir, 
        [string]$suffix=''
    )

    $prefix = if ($UseMsBuild) { 'msbuild' } else { 'dotnet' } 
    
    $sln = [System.IO.Path]::GetFileNameWithoutExtension($SolutionFile)
    $name = $sln + '.' + $prefix
    if (-not [string]::IsNullOrEmpty($suffix)) {
        $name += '.' + $suffix
    }

    return Join-Path $ArtifactsDir ($name + '.binlog')
}

Function Ensure-Directory {
    param(
        [string]$Directory
    )
    
    if (-not (Test-Path $Directory)) {
        New-Item -ItemType Directory -Path $Directory | Out-Null
    }
}

if ($psISE) {
    $Eng =  (Get-Item (Split-Path $psISE.CurrentFile.FullPath -Parent)).FullName
} else {
    $Eng =  (Get-Item $PSScriptRoot).FullName
}

if ($UseMsBuild) {
    Start-VsDevCmd
}
$Architecture = Fixup-Architecture $Architecture
$RepoRoot = (Get-Item $Eng).Parent.FullName
$GlobalJson = Join-Path $RepoRoot 'global.json' 
$DotNetToolsDirectory = Join-Path $RepoRoot '.dotnet' 
$DotNet = Join-Path $DotNetToolsDirectory 'dotnet.exe'
$Solution = Join-Path $RepoRoot 'WPFSamples.sln'
$MsBuildOnlySolution = Join-Path $RepoRoot 'WPFSamples.msbuild.sln'


if (-not $TargetFramework) {
    $json = (Get-Content $GlobalJson | ConvertFrom-Json)
    $TargetFramework = Get-Tfm $json.sdk.version
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

<# Run in local scope to inherit updates to $env:PATH #>
$EnsureGlobalJsonSdk = Join-Path $Eng 'EnsureGlobalJsonSdk.ps1'
. $EnsureGlobalJsonSdk -g $GlobalJson -i $DotNetToolsDirectory -a $Architecture -f $TargetFramework
if (-not (Test-Path $DotNet)) {
    Write-Error "$DotNet not found - exiting"
    exit
}



Set-Location $RepoRoot 

$escapeparser = '--%'
if (-not $UseMsBuild) {
    $LogFile = Get-LogFile -UseMsBuild $false -SolutionFile $Solution -ArtifactsDir $Artifacts
    $RestoreLogFile = Get-LogFile -UseMsBuild $false -SolutionFile $Solution -ArtifactsDir $Artifacts -suffix 'restore' 

    $BuildArgs = "$escapeparser /bl:$LogFile /p:Platform=$Architecture /p:LangVersion=preview /p:PublishDir=$PublishDir /p:RuntimeIdentifier=$RuntimeIdentifier /clp:Summary;Verbosity=minimal"
    $RestoreArgs = "$escapeparser /bl:$RestoreLogFile /p:RuntimeIdentifier=$RuntimeIdentifier /clp:Summary;Verbosity=minimal"

    $BuildCmd = "$DotNet restore $RestoreArgs $Solution"
    Write-Verbose $BuildCmd
    if (-not $DryRun) {
        Invoke-Expression $BuildCmd
    }

    $BuildCmd = "$DotNet publish $BuildArgs $Solution"
    Write-Verbose $BuildCmd
    if (-not $DryRun) {
        Invoke-Expression $BuildCmd
    }
}

if ($UseMsBuild) {
    $LogFile = Get-LogFile -UseMsBuild $true -SolutionFile $Solution -ArtifactsDir $Artifacts
    $RestoreLogFile = Get-LogFile -UseMsBuild $true -SolutionFile $Solution -ArtifactsDir $Artifacts -suffix 'restore' 

    $BuildArgs =   "$escapeparser /bl:$LogFile /p:Platform=$Architecture /p:LangVersion=preview /p:PublishDir=$PublishDir /p:RuntimeIdentifier=$RuntimeIdentifier /m /clp:Summary;Verbosity=minimal /t:publish"
    $RestoreArgs = "/t:restore /p:RuntimeIdentifier=$RuntimeIdentifier /bl:$RestoreLogFile /noconlog /m"

    $LogFile2 = Get-LogFile -UseMsBuild $true -SolutionFile $MsBuildOnlySolution -ArtifactsDir $Artifacts
    $RestoreLogFile2 = Get-LogFile -UseMsBuild $true -SolutionFile $MsBuildOnlySolution -ArtifactsDir $Artifacts -suffix 'restore' 
    $BuildArgs2 =  "$escapeparser /bl:$LogFile2 /p:Platform=$Architecture /p:LangVersion=preview /p:PublishDir=$PublishDir /p:RuntimeIdentifier=$RuntimeIdentifier /m /clp:Summary;Verbosity=minimal /t:publish"
    $RestoreArgs2 = "/t:restore /p:RuntimeIdentifier=$RuntimeIdentifier /bl:$RestoreLogFile2 /noconlog /m"

    $BuildCmd = "msbuild $RestoreArgs $Solution"
    Write-Verbose $BuildCmd
    if (-not $DryRun) {
        Invoke-Expression $BuildCmd
    }

    $BuildCmd = "msbuild $BuildArgs $Solution"
    Write-Verbose $BuildCmd
    if (-not $DryRun) {
        Invoke-Expression $BuildCmd
    }

    $BuildCmd = "msbuild $RestoreArgs2 $MsBuildOnlySolution"
    Write-Verbose $BuildCmd
    if (-not $DryRun) {
        Invoke-Expression $BuildCmd
    }

    $BuildCmd = "msbuild $BuildArgs2 $MsBuildOnlySolution"
    Write-Verbose $BuildCmd
    if (-not $DryRun) {
        Invoke-Expression $BuildCmd
    }
}

<# restore global.json #>
Copy-Item (Join-Path $ArtifactsTemp 'global.json') $GlobalJson -Force


