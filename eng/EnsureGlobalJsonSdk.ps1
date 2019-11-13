#
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
  [ValidateSet('', $null, 'netcoreapp3.0', 'netcoreapp3.1', 'netcoreapp5.0', IgnoreCase=$true)]
  $TargetFramework=''
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
        'netcoreapp5.0'
    )

    $tfm = ('netcoreapp' + $SdkVersion.Substring(0,3)).Trim().ToLowerInvariant()

    return IIf ($WellKnownTFMs -icontains $tfm) $tfm ""
}



# Use-RunAs function from TechNet Script Gallery
# https://gallery.technet.microsoft.com/scriptcenter/63fd1c0d-da57-4fb4-9645-ea52fc4f1dfb
function Use-RunAs {    
    # Check if script is running as Adminstrator and if not use RunAs 
    # Use Check Switch to check if admin 
    param([Switch]$Check) 
     
    $IsAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()` 
        ).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator") 
         
    if ($Check) { 
		return $IsAdmin 
	}     
    if ($MyInvocation.ScriptName -ne "") {  
        if (-not $IsAdmin) {  
            try {  
				$arg = "-file `"$($MyInvocation.ScriptName)`"" 
				
				Write-Verbose "Starting elevated process..."
				Write-Verbose '\t' "$psHome\powershell.exe" -Verb Runas -ArgumentList $arg -ErrorAction 'stop'  
				
                Start-Process "$psHome\powershell.exe" -Verb Runas -ArgumentList $arg -ErrorAction 'stop'  
            } 
            catch { 
                Write-Warning "Error - Failed to restart script with runas"  
                break               
            } 
            Exit # Quit this session of powershell 
        }  
    }  
    else {  
        Write-Warning "Error - Script must be saved as a .ps1 file first"  
        break  
    }  
} 

Use-RunAs
if (Test-Path $globalJson) {
    $json = Get-Content $globalJson | ConvertFrom-Json

    <# 
        If the $TargetFramework is no different from
        that of the version supplied in global.json/sdk.version, don't
        use it for further decisions.
    #>
    if ($TargetFramework -ieq (Get-Tfm -SdkVersion $json.sdk.version)) {
        $TargetFramework = ''
    }

    if (-not $TargetFramework) {
        $sdk_version = $json.sdk.version
    } else {
        Write-Verbose "Alternate TargetFramework requested - reading from altsdk section in global.json"
        $sdk_version = $json.altsdk.$TargetFramework
        if ($sdk_version) {
            Write-Verbose "Alternate SDK Version: $sdk_version"
        } else {
            Write-Verbose "Alternate SDK Version not found"
        }
    }

    if ($sdk_version) {
        $dotnet_install = "$env:TEMP\dotnet-install.ps1"

        $AllProtocols = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'
        [System.Net.ServicePointManager]::SecurityProtocol = $AllProtocols

        Invoke-WebRequest https://dot.net/v1/dotnet-install.ps1 -OutFile $dotnet_install
        Write-Verbose "Downloaded dotnet-install.ps1 to $dotnet_install"

        .$dotnet_install -Channel $channel -Version $sdk_version -Architecture $architecture -InstallDir $installPath
        Write-Verbose "Installed SDK Version=$sdk_version Channel=$channel Architecture=$architecture to $installPath"
        
        Write-Host "##vso[task.prependpath]$installPath"
        Write-Verbose "Added $installPath to Azure Pipelines PATH variable"

        <# 
            If $TargetFramework is specified, then an alternate SDK was requested.
            This means that the build requires an updated global.json as well. 

            This is a destructive change. The global.json should be restored by doing this after a build:
                    git checkout global.json
        #>
        if ($TargetFramework) {
            $dotnet = Join-Path $installPath 'dotnet.exe'
            & $dotnet new globaljson --sdk-version $sdk_version --force
        }
    }
}