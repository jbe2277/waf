setlocal
dotnet build %~dp0..\src\System.Waf\System.Waf.sln -c Release -p:ContinuousIntegrationBuild=true