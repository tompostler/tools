﻿# Tom Postler, 2017-03-03
# Generate this specific assembly information

# Get the necesary information out of the XML
[xml]$packageInfo = Get-Content "$PSScriptRoot\..\..\Unlimitedinf.Tools.Packages\Unlimitedinf.Tools.xml";
$Version = $packageInfo.package.metadata.version;
$Prerelease = $packageInfo.package.LastUpdated;

# Remove anything that would disagree with AssemblyVersion
if ($Version.Contains("-")) {
    $Version = $Version.Split("-")[0];
}
if ($Version.Contains("+")) {
    $Version = $Version.Split("+")[0];
}

# Parse out the SemVer
$Major, $Minor, $Patch = $Version.Split('.');

# Set the prerelease to the tenths of days since the version string was last updated
$Prerelease = [Math]::Truncate(([DateTime]::Now - [DateTime]::Parse($Prerelease)).TotalDays * 10);

# The file contents
@"
//
// This code was generated by a tool. Any changes made manually will be lost the next time this code is regenerated.
//

using System.Reflection;

[assembly: AssemblyTitle("Unlimitedinf.Tools")]
[assembly: AssemblyProduct("Unlimitedinf.Tools")]

[assembly: AssemblyVersion("{0}.{1}.0.0")]
[assembly: AssemblyFileVersion("{0}.{1}.{2}.{3}")]
"@ -f $Major, $Minor, $Patch, $Prerelease > "$PSScriptRoot\LocalAssemblyInfo.cs";

Write-Host "Generated local assembly info.";