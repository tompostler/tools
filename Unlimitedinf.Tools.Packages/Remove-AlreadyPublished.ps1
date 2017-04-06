# Tom Postler, 2017-04-06
# Remove nupkg files that are already published

# Set CWD to script location
Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

if (Test-Path ".\alreadypublished") {

# What packages do we need to look at?
$packageIds = Get-Content ".\alreadypublished";

# Loop for each package already published, and remove it
foreach ($packageId in $packageIds) {
    $packageId, $version = $packageId.Split(' ');

    $paths = @(
        ".\bin\Release\$packageId.$version.nupkg",
        ".\bin\Release\$packageId.$version.symbols.nupkg"
    );
    foreach ($path in $paths) {
        if (Test-Path "$path") {
            Remove-Item $path;
            Write-Host "Removed $path";
        } else {
            Write-Error "Could not find $path to remove.";
        }
    }
}

}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
