# Tom Postler, 2017-12-29
# Remove already published nupkg files

# Set CWD to script location
Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

# What packages do we need to look at?
$packageIds = [IO.Directory]::EnumerateFiles($PWD, "*.nupkg") | ForEach-Object { Split-Path $_ -Leaf } | ForEach-Object { $_.Substring(0, $_.LastIndexOf('.')) }

# Loop for each package
foreach ($packageId in $packageIds) {
    # Get the version and the id (need to find the 3rd to last .)
    $pos = $packageId.LastIndexOf('.')
    $pos = $packageId.LastIndexOf('.', $pos - 1)
    $pos = $packageId.LastIndexOf('.', $pos - 1)
    $version = $packageId.Substring($pos + 1, $packageId.Length - $pos - 1)
    $packageId = $packageId.Substring(0, $pos)
    Write-Host "Found version '$version' for '$packageId'";

    # Get the nuget package repository
    $pkgNuGet_Core = "NuGet.Core." + ([xml](Get-Content ".\packages.config")).packages.package.version
    [Reflection.Assembly]::LoadFile((Resolve-Path ".\packages\$pkgNuGet_Core\lib\net40-Client\NuGet.Core.dll").Path) | Out-Null
    $repo = [NuGet.PackageRepositoryFactory]::Default.CreateRepository("https://packages.nuget.org/api/v2")

    # Check if current version is already published
    if (($repo.FindPackagesById($packageId) | Where-Object {$_.Version -eq $version} | Measure-Object).Count -eq 1) {
        Write-Warning "Nuget package $packageId.$version already published."
        Remove-Item "$packageId.$version.nupkg" -Verbose
    } else {
        Write-Host "Nuget package $packageId.$version not found on nuget.org"
    }
}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
