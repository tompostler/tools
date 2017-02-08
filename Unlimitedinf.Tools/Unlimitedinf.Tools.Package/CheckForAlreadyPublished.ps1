# Get the version from the nuproj
[xml]$proj = Get-Content .\Unlimitedinf.Tools.Package.nuproj;
$version = $proj.Project.PropertyGroup | ? {$_.Version} | Select-Object -ExpandProperty Version;

# Get the nuget package repository
$pkgNuGet_Core = "NuGet.Core." + ([xml](Get-Content .\packages.config)).packages.package.version
[Reflection.Assembly]::LoadFile((Resolve-Path "..\packages\$pkgNuGet_Core\lib\net40-Client\NuGet.Core.dll").Path) | Out-Null;
$repo = [NuGet.PackageRepositoryFactory]::Default.CreateRepository("https://packages.nuget.org/api/v2");

# Check if current version is already published
if (($repo.FindPackagesById("Unlimitedinf.Tools") | ? {$_.Version -eq $version} | Measure-Object).Count -eq 1) {
    Write-Error "Nuget package Unlimitedinf.Tools.$version already published.";
}
