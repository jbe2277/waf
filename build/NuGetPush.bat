setlocal
set Params=-s https://api.nuget.org/v3/index.json --force-english-output
set Version=7.0.0-alpha.11

cd %~dp0..\src\System.Waf\System.Waf\

dotnet nuget push .\System.Waf.Core\bin\Release\System.Waf.Core.%Version%.nupkg %Params%
dotnet nuget push .\System.Waf.UnitTesting.Core\bin\Release\System.Waf.UnitTesting.Core.%Version%.nupkg %Params%
dotnet nuget push .\System.Waf.Wpf\bin\Release\System.Waf.Wpf.%Version%.nupkg %Params%
dotnet nuget push .\System.Waf.UnitTesting.Wpf\bin\Release\System.Waf.UnitTesting.Wpf.%Version%.nupkg %Params%
