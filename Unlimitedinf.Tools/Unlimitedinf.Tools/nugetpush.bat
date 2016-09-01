:: %1 == API key from https://www.nuget.org/account
:: %2 == package name after build (*kg tab on cli)

nuget setApiKey %1 -Source https://www.nuget.org/api/v2/package
nuget push %2 -Source https://www.nuget.org/api/v2/package
