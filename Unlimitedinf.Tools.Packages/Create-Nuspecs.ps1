# Tom Postler, 2017-03-03
# Generate nuspec files for all the projects/things

# Set CWD to script location
Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

# Get common data
$packageIds = Get-Content ".\PackageIds.txt";
[xml]$commonXml = Get-Content ".\Common.xml";

# Loop for each package
foreach ($packageId in $packageIds) {
    # Get the corresponding specifics
    [xml]$packageXml = Get-Content ".\$packageId.xml";
    
    # Opening XML things and common package info
    $nuspec = '<?xml version="1.0" encoding="utf-8"?><package><metadata>';
    $nuspec += "<id>$packageId</id>";
    $nuspec += "<title>$packageId</title>";
    
    # Add XML common to all packages and this specific package
    $nuspec += $packageXml.package.metadata.InnerXml;
    $nuspec += $commonXml.package.metadata.InnerXml;
    
    # Close XML and save
    $nuspec += '</metadata></package>';
    ([xml]$nuspec).Save("..\$packageId\$packageId.nuspec");
    Write-Host "Saved $packageId.nuspec";
}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
