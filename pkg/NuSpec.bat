@echo off
setlocal
set PkgDir=%~dp0
set PATH=%PATH%;%PkgDir%

cd %PkgDir%..\src\System.Waf\System.Waf\System.Waf.Core\
nuget spec -f

cd %PkgDir%..\src\System.Waf\System.Waf\System.Waf.Wpf\
nuget spec -f

cd %PkgDir%..\src\System.Waf\System.Waf\System.Waf.UnitTesting.Core\
nuget spec -f

cd %PkgDir%..\src\System.Waf\System.Waf\System.Waf.UnitTesting.Wpf\
nuget spec -f

cd %PkgDir%