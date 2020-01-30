<# Configuration Generator for azure-pipelines.xml #> 
Function IIf($If, $Then, $Else) {
    If ($If -IsNot "Boolean") {$_ = $If}
    If ($If) {If ($Then -is "ScriptBlock") {&$Then} Else {$Then}}
    Else {If ($Else -is "ScriptBlock") {&$Else} Else {$Else}}
}

$config = @('Debug', 'Release')
$platform = @('Any CPU', 'x86', 'x64')
$tfm = @('netcoreapp3.1', 'netcoreapp5.0')

$entries = @{}

foreach ($configItem in $config) {
    foreach ($platformItem in $platform) {
        foreach($tfmItem in $tfm){
            $entries."$configItem-$platformItem-$tfmItem" = @{config=$configItem;platform=$platformItem;tfm=$tfmItem}
        }
    }
}

clear-host 

$entries.keys | sort | foreach {
    [string]$item = $PSItem
    $item = $item.Replace(' ', '')
    "  $item`:"
    "    _Configuration: $($entries[$PSItem].config)"
    "    _Platform: `'$($entries[$PSItem].platform)`'"
    "    _TargetFramework: `'$($entries[$PSItem].tfm)`'"
    "    _ToolPlatform: " + (IIf ($($entries[$PSItem].platform) -ieq 'Any CPU') "x86" $($entries[$PSItem].platform))
}