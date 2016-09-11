# Check for previously saved key and either setApiKey or just push the package
# Also, assume nuget.exe is here after a retail build that generated the package

param (
    [string]$apiKey = "",
    [Parameter(Mandatory=$true)][string]$package,
	[string]$packageSource = "https://www.nuget.org/api/v2/package"
)

# If api key provided, set it [again]
if (![string]::IsNullOrEmpty($apiKey)) {
	.\nuget.exe setApiKey $apiKey -Source $packageSource
}

# Key not found
if (-not (Get-Content $env:appdata\NuGet\NuGet.Config | Select-String "$packageSource" -quiet)) {
	Write-Host -ForegroundColor Red "apiKey not found.";
	exit;
}

# Put new package
.\nuget.exe push $package -Source "$packageSource"
