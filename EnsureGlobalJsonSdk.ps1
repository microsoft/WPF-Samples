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
  $architecture=$env:PROCESSOR_ARCHITECTURE
)

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
    $sdk_version = (Get-Content $globalJson | ConvertFrom-Json).sdk.version
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
    }
}