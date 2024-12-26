setlocal
set DOTNET_CLI_UI_LANGUAGE=en
cd %~dp0../src

rem  -p:ContinuousIntegrationBuild=true  ... Workaround: does not work for coverage
dotnet build System.Waf/System.Waf.sln -c Release

pwsh -c "Get-ChildItem -Recurse | Where-Object {$_.Name -eq 'TestResults'} | Remove-Item -Force -Recurse"
dotnet test System.Waf/System.Waf.sln -c Release --no-build

dotnet-coverage merge --output ../out/CodeCoverage/System.Waf.cobertura.xml --output-format cobertura "./**/TestResults/**/*.coverage"

reportgenerator -reports:../out/CodeCoverage/System.Waf.cobertura.xml -targetdir:../out/CodeCoverage -reporttypes:"MarkdownSummaryGithub"