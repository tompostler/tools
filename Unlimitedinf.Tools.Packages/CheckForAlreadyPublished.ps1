# Tom Postler, 2017-03-06
# Generate nuspec files for all the projects/things

# Set CWD to script location
Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

# What packages do we need to look at?
$packageIds = Get-Content ".\PackageIds.txt";

# Loop for each package
$notFound = 0;
foreach ($packageId in $packageIds) {
    # Get the version from the xml
    [xml]$packageXml = Get-Content ".\$packageId.xml";
    $version = $packageXml.package.metadata.version
    Write-Host "Found version '$version' in $packageId.xml";

    # Get the nuget package repository
    $pkgNuGet_Core = "NuGet.Core." + ([xml](Get-Content ".\packages.config")).packages.package.version
    [Reflection.Assembly]::LoadFile((Resolve-Path "..\packages\$pkgNuGet_Core\lib\net40-Client\NuGet.Core.dll").Path) | Out-Null;
    $repo = [NuGet.PackageRepositoryFactory]::Default.CreateRepository("https://packages.nuget.org/api/v2");

    # Check if current version is already published
    if (($repo.FindPackagesById($packageId) | ? {$_.Version -eq $version} | Measure-Object).Count -eq 1) {
        $msg = "Nuget package $packageId.$version already published.";
        "$packageId $version" >> "alreadypublished";
        Write-Warning $msg;
    } else {
        Write-Host "Nuget package $packageId.$version not found on nuget.org";
        $notFound++;
    }
}

# No new versions to publish
if ($notFound -eq 0) {
    Write-Error "All packages current.";
}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
