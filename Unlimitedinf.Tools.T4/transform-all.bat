:: http://stackoverflow.com/a/3041089 accessed 2017-02-02
:: Modified by Tom Postler, 2017-02-11

:: Useful in situations where T4 templates are needed but it is not installed (e.g. VSTS builds)
:: Add the following in to a project in order to perform the transform on every build:
::    <PropertyGroup>
::      <PreBuildEvent>$(SolutionDir)\packages\Unlimitedinf.Tools.#.#.#\tools\t4\transform-all.bat $(ProjectDir)</PreBuildEvent>
::    </PropertyGroup>

:: I do not intend to break any licenses for T4. I just found that there is no NuGet package with it.

@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

:: set the working dir (default to current dir)
set wdir=%cd%
if not (%1)==() set wdir=%1

:: set the file extension (default to cs)
set extension=cs
if not (%2)==() set extension=%2

echo Performing T4 transforms in subdirectories of %wdir%
:: create a list of all the T4 templates in the working dir
dir /b /s %wdir%\*.tt > t4list.txt

echo The following T4 templates will be transformed in place:
type t4list.txt
echo Transforming templates...

:: transform all the templates
for /f %%d in (t4list.txt) do (
    set file_name=%%d
    set file_name=!file_name:~0,-3!.%extension%
    echo:  \--^> !file_name!    
    %~dp0\TextTransform.exe -out !file_name! %%d
)
del t4list.txt

echo Transformation complete.