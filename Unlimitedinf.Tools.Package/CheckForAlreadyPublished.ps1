# Get the version from the nuproj
[xml]$proj = Get-Content ..\Version.xml;
$version = $proj.Project.PropertyGroup | ? {$_.Version} | Select-Object -ExpandProperty Version;
Write-Host "Found version '$version' in Version.xml";

# Get the nuget package repository
$pkgNuGet_Core = "NuGet.Core." + ([xml](Get-Content .\packages.config)).packages.package.version
[Reflection.Assembly]::LoadFile((Resolve-Path "..\packages\$pkgNuGet_Core\lib\net40-Client\NuGet.Core.dll").Path) | Out-Null;
$repo = [NuGet.PackageRepositoryFactory]::Default.CreateRepository("https://packages.nuget.org/api/v2");

# Check if current version is already published
if (($repo.FindPackagesById("Unlimitedinf.Tools") | ? {$_.Version -eq $version} | Measure-Object).Count -eq 1) {
    $msg = "Nuget package Unlimitedinf.Tools.$version already published.";
    Write-Host $msg;
    Write-Error $msg;
} else {
    Write-Host "Nuget package Unlimitedinf.Tools.$version not found on nuget.org";
}
