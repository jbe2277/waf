@echo off
setlocal
set SolutionDir=%~dp0..\src\System.Waf
set Configuration=Release
set KeyOriginatorFile=P:\IdentityKey.snk
set DefineConstants="CODE_ANALYSIS;TRACE;STRONG_NAME"

set BuildParams=/t:Rebuild /p:Configuration=%Configuration% /p:KeyOriginatorFile=%KeyOriginatorFile%

call "%VS140COMNTOOLS%\vsvars32.bat"

msbuild.exe %SolutionDir%\System.Waf\System.Waf.Core\System.Waf.Core.csproj %BuildParams% /p:DefineConstants=%DefineConstants%
msbuild.exe %SolutionDir%\System.Waf\System.Waf.UnitTesting.Core\System.Waf.UnitTesting.Core.csproj %BuildParams% /p:DefineConstants=%DefineConstants%

msbuild.exe %SolutionDir%\System.Waf\System.Waf.Wpf\System.Waf.Wpf.csproj %BuildParams% /p:DefineConstants=%DefineConstants%
msbuild.exe %SolutionDir%\System.Waf\System.Waf.UnitTesting.Wpf\System.Waf.UnitTesting.Wpf.csproj %BuildParams% /p:DefineConstants=%DefineConstants%

msbuild.exe %SolutionDir%\System.Waf\System.Waf.Uwp\System.Waf.Uwp.csproj %BuildParams% /p:DefineConstants="CODE_ANALYSIS;STRONG_NAME;NETFX_CORE;WINDOWS_UWP"