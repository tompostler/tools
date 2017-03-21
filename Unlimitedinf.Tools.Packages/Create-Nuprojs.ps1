# Tom Postler, 2017-03-20
# Generate nuproj files for all the projects/things

# Set CWD to script location
Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

# Get common data
$packageIds = Get-Content ".\PackageIds.txt";

# Loop for each package
foreach ($packageId in $packageIds) {    
    # Create the text of the nuproj
    $nuproj = @"
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  
  <Import Project="Common.props" />
  <Import Project="$packageId.xml" />
  <PropertyGroup Label="Configuration">
    <Id>$packageId</Id>
    <Title>$packageId</Title>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\$packageId\$packageId.Latest\$packageId.csproj" />
    <ProjectReference Include="..\$packageId\$packageId.Net40\$packageId.Net40.csproj" />
    <ProjectReference Include="..\$packageId\$packageId.Net45\$packageId.Net45.csproj" />
    <ProjectReference Include="..\$packageId\$packageId.Net46\$packageId.Net46.csproj" />
  </ItemGroup>
  <Import Project="`$(NuProjPath)\NuProj.targets" Condition="Exists('`$(NuProjPath)\NuProj.targets')" />
</Project>
"@;

    ([xml]$nuproj).Save(".\$packageId.nuproj");
    Write-Host "Saved $packageId.nuproj";
}

# Restore CWD
Pop-Location
[Environment]::CurrentDirectory = $PWD
