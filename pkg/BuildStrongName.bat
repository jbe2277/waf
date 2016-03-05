@echo off
setlocal
set SolutionDir=%~dp0..\src\System.Waf
set OutputPath=..\..\..\..\out\System.Waf\StrongName\
set Configuration=Release
set DefineConstants="TRACE;CODE_ANALYSIS;STRONG_NAME"
set KeyOriginatorFile=P:\IdentityKey.snk

set BuildParams=/t:Rebuild /p:Configuration=%Configuration% /p:OutputPath=%OutputPath% /p:DefineConstants=%DefineConstants% /p:KeyOriginatorFile=%KeyOriginatorFile%

call "%VS140COMNTOOLS%\vsvars32.bat"

msbuild.exe %SolutionDir%\System.Waf\System.Waf.Core\System.Waf.Core.csproj %BuildParams%
msbuild.exe %SolutionDir%\System.Waf\System.Waf.UnitTesting.Core\System.Waf.UnitTesting.Core.csproj %BuildParams%
msbuild.exe %SolutionDir%\System.Waf\System.Waf.Wpf\System.Waf.Wpf.csproj %BuildParams%
msbuild.exe %SolutionDir%\System.Waf\System.Waf.UnitTesting.Wpf\System.Waf.UnitTesting.Wpf.csproj %BuildParams%