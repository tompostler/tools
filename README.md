# tools

_Organized by package/dll_

#### Badges!
|Wut|Badge|
|:--|:-:|
|Build (+tests) on VSTS|![Build on VSTS](https://tompostler.visualstudio.com/_apis/public/build/definitions/59d5d8a6-84be-41a0-b43a-6b99271c20fb/4/badge)|
|`Unlimitedinf.Tools`|[![Unlimitedinf.Tools NuGet](https://img.shields.io/nuget/v/Unlimitedinf.Tools.svg?style=flat-square)](https://www.nuget.org/packages/Unlimitedinf.Tools/)|
|`Unlimitedinf.Tools.Statics`|[![Unlimitedinf.Tools.Statics NuGet](https://img.shields.io/nuget/v/Unlimitedinf.Tools.Statics.svg?style=flat-square)](https://www.nuget.org/packages/Unlimitedinf.Tools.Statics/)|
|`Unlimitedinf.Tools.Numerics`|[![Unlimitedinf.Tools.Numerics NuGet](https://img.shields.io/nuget/v/Unlimitedinf.Tools.Numerics.svg?style=flat-square)](https://www.nuget.org/packages/Unlimitedinf.Tools.Numerics/)|

## Unlimitedinf.Tools

_The nuget package that collects everything that doesn't belong somewhere else_

Currently just contains `Unlimitedinf.Tools.dll`.

|Class|Description|
|:--|:--|
|`GenerateRandom`|A class containing various methods to generate random information using the `RNGCryptoServiceProvider`|
|`Id`|A lighter-weight id tracker to replace the heftiness of Guid. 4 billion unique Ids, with 2 billion before statistically likely collisions built on a backing datatype of 4 bytes.|
|`Log`|A custom wrapper for the standard `Console.WriteLine` method that prints the way I normally do for my projects.|
|`SemVer`|To help with representing and performing operations on semantic versions version 2 ([SemVer.org](http://semver.org/spec/v2.0.0.html))|
|`IO.FileSystemCollection`|Class for enumerating files and directories in a way that exposes through an `IEnumerable` and tries to gracefully ignore the most common exceptions it encounters.|
|`IO.ThrottledStream`|Class for streaming data with throttling support|
|`Hashing.Hasher`|A wrapper for common hashing functions to operate on streams or files|
|`Hashing.Crc32`|Implements the Crc32 checksum as a `HashAlgorithm`|
|`Hashing.Blockhash`|Perceptual image hash calculation as a `HashAlgorithm`. Warning: Memory intensive due to image verification/handling.|

|Function|Description|
|:--|:--|
|`double Median<T>(this IEnumerable<T> source)`|Extension method to calculate the median of an `IEnumerable` whose elements can be `Convert.ToDouble`|
|`bool TryParseRelativeDateTime(this string toParse, out DateTime result)`|Given a string, treat it as an engligh-formatted relative datetime and attempt to parse an actual datetime out|

## Unlimitedinf.Tools.Numerics

_The ruff tuff number-crunchin maniac nuget package_

Initially started as part of my solutions to [Project Euler](http://www.projecteuler.net), these useful functions were broken out into their own DLL, and then their own package, to maximize reusability.

For a list of all functions available, see the source.

## Unlimitedinf.Tools.Statics

_The nuget package of constant data_

Contains `Unlimitedinf.Tools.Statics.dll` with the following:

|Class|Description|
|:--|:--|
|`Numerics.Primes`|Contains two members, `int[] Ordered` and `HashSet<int> Unordered`, which contain the prime integers from 2 to 1000003, inclusive.|

## Other notes

- If strong-name signing is required, please educate me on how to do it with VSTS. Alternatively, you may take a copy of the source (as permitted by the MIT license) and build+sign it yourself (in case that part was unclear). If you wish to distribute that effort more broadly than your own application, please get in touch so that I may do that on your behalf.
