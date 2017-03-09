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
	if (Test-Path "$packagePath") {
		Remove-Item $packagePath;
		Write-Host "Removed $packagePath";
	} else {
		# Some VSTS builds seems to but it in the root for some reason
		$packagePath = "..\$packageId.$version.nupkg"
		if (Test-Path "$packagePath") {
			Remove-Item $packagePath;
			Write-Host "Removed $packagePath";
		} else {
			Write-Error "Could not find $packageId.$version.nupkg to remove.";
		}
	}
}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
