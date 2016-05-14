@echo off
setlocal
set PkgDir=%~dp0
set PATH=%PATH%;%PkgDir%
set Version=4.0.0-alpha5

cd %PkgDir%\System.Waf\Release

nuget Push System.Waf.Core.%Version%.nupkg
nuget Push System.Waf.UnitTesting.Core.%Version%.nupkg
nuget Push System.Waf.Wpf.%Version%.nupkg
nuget Push System.Waf.UnitTesting.Wpf.%Version%.nupkg
nuget Push System.Waf.Uwp.%Version%.nupkg