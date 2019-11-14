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
  $DryRun

)

Function IIf($If, $Then, $Else) {
    If ($If -IsNot "Boolean") {$_ = $If}
    If ($If) {If ($Then -is "ScriptBlock") {&$Then} Else {$Then}}
    Else {If ($Else -is "ScriptBlock") {&$Else} Else {$Else}}
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

if ($psISE) {
    $Eng =  (Get-Item (Split-Path $psISE.CurrentFile.FullPath -Parent)).FullName
} else {
    $Eng =  (Get-Item $PSScriptRoot).FullName
}


$Architecture = Fixup-Architecture $Architecture
$RepoRoot = (Get-Item $Eng).Parent.FullName
$GlobalJson = Join-Path $RepoRoot 'global.json' 
$DotNetToolsDirectory = Join-Path $RepoRoot '.dotnet' 
$DotNet = Join-Path $DotNetToolsDirectory 'dotnet.exe'
$Solution = Join-Path $RepoRoot 'WPFSamples.sln'


if (-not $TargetFramework) {
    $json = (Get-Content $GlobalJson | ConvertFrom-Json)
    $TargetFramework = Get-Tfm $json.sdk.version
}

$Artifacts = Join-path (Join-path (Join-Path (Join-Path $RepoRoot 'artifacts') $Configuration) $TargetFramework) $Architecture
$PublishDir = Join-Path $Artifacts 'publish'
$LogFile = Join-path $Artifacts "dotnet.binlog"
$ArtifactsTemp = Join-Path $Artifacts 'Temp' 
$RuntimeIdentifier = Identify-RID $Architecture

if (-not (Test-Path $Artifacts)) {
    New-Item -ItemType Directory -Path $Artifacts
}

if (-not (Test-Path $ArtifactsTemp)) {
    New-Item -ItemType Directory -Path $ArtifactsTemp
}

<# Save Global.json file #>
Copy-Item $GlobalJson -Destination (join-path $ArtifactsTemp 'global.json') -Force

<# Run in local scope to inherit updates to $env:PATH #>
$EnsureGlobalJsonSdk = Join-Path $Eng 'EnsureGlobalJsonSdk.ps1'
. $EnsureGlobalJsonSdk -g $GlobalJson -i $DotNetToolsDirectory -a $Architecture -f $TargetFramework
if (-not (Test-Path $DotNet)) {
    Write-Error "$DotNet not found - exiting"
    exit
}

$RestoreArgs = "restore $Solution"
$BuildArgs = "publish /bl:$LogFile /p:Platform=`"$Architecture`" /p:LangVersion=preview /p:PublishDir=$PublishDir /p:RuntimeIdentifier=$RuntimeIdentifier $Solution"

Set-Location $RepoRoot 

$BuildCmd = "$DotNet $RestoreArgs"
Write-Verbose $BuildCmd
if (-not $DryRun) {
    Invoke-Expression $BuildCmd
}

$BuildCmd = "$DotNet $BuildArgs"
Write-Verbose $BuildCmd
if (-not $DryRun) {
    Invoke-Expression $BuildCmd
}


<# restore global.json #>
Copy-Item (Join-Path $ArtifactsTemp 'global.json') $GlobalJson -Force


