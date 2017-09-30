@echo off
setlocal
set PkgDir=%~dp0
set PATH=%PATH%;%PkgDir%
set Version=4.1.0-rc2

cd %PkgDir%\System.Waf\Release

nuget Push System.Waf.Core.%Version%.nupkg -Source https://www.nuget.org/api/v2/package
nuget Push System.Waf.UnitTesting.Core.%Version%.nupkg -Source https://www.nuget.org/api/v2/package
nuget Push System.Waf.Wpf.%Version%.nupkg -Source https://www.nuget.org/api/v2/package
nuget Push System.Waf.UnitTesting.Wpf.%Version%.nupkg -Source https://www.nuget.org/api/v2/package

rem nuget Push System.Waf.Uwp.4.1.0-alpha8.nupkg -Source https://www.nuget.org/api/v2/package