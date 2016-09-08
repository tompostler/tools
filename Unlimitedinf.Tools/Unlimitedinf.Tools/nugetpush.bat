:: Check for previously saved key and either setApiKey or just push the package

>nul find "https://www.nuget.org/api/v2/package" %appdata%\NuGet\NuGet.Config && (

    :: %1 == package name after build (*kg tab on cli)
    nuget push %2 -Source https://www.nuget.org/api/v2/package

) || (

    :: %1 == API key from https://www.nuget.org/account
    :: %2 == package name after build (*kg tab on cli)
    nuget setApiKey %1 -Source https://www.nuget.org/api/v2/package
    nuget push %2 -Source https://www.nuget.org/api/v2/package

)
