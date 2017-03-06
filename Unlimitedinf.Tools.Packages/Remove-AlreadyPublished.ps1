# Tom Postler, 2017-03-06
# Remove nupkg files that are already published

# Set CWD to script location
Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

# What packages do we need to look at?
$packageIds = Get-Content ".\alreadypublished";

# Loop for each package already published, and remove it
foreach ($packageId in $packageIds) {
    $packageId, $version = $packageId.Split(' ');
    $packagePath = "..\$packageId\$packageId.$version.nupkg"
    Remove-Item $packagePath;
    Write-Host "Removed $packagePath";
}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
