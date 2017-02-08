# Download nuget.exe if not found.
# If apiKey provided, set it. Then check for it.
# Nuget pack (assumes a retail build has been done)
# Nuget push

param (
    [string]$apiKey = "",
    [Parameter(Mandatory=$true)][string]$nuspec,
    [string]$packageSource = "https://www.nuget.org/api/v2/package"
)

# Make sure we have a nuget.exe
if (![System.IO.File]::Exists("nuget.exe")) {
    Write-Host -ForegroundColor Red "nuget.exe not found.";
    Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -Outfile "nuget.exe"
}

# If api key provided, set it [again]
if (![string]::IsNullOrEmpty($apiKey)) {
    .\nuget.exe setApiKey $apiKey -Source $packageSource
}
# Key not found
if (-not (Get-Content $env:appdata\NuGet\NuGet.Config | Select-String "$packageSource" -quiet)) {
    Write-Host -ForegroundColor Red "apiKey not found.";
    exit;
}

# Build new package
.\nuget.exe pack $nuspec -Build -Properties "Configuration=Release;Platform=AnyCPU" -IncludeReferencedProjects -Exclude "*.ico"

# Put new package
# .\nuget.exe push $package -Source "$packageSource"
