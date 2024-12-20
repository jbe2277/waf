setlocal
set DOTNET_CLI_UI_LANGUAGE=en
cd %~dp0../src

dotnet build System.Waf/System.Waf.sln -c Release -p:ContinuousIntegrationBuild=true

pwsh -c "Get-ChildItem -Recurse | Where-Object {$_.Name -eq 'TestResults'} | Remove-Item -Force -Recurse"
dotnet test System.Waf/System.Waf.sln -c Release --no-build --collect "Code Coverage"

mkdir ..\out\CodeCoverageReport
dotnet-coverage merge --output ../out/CodeCoverageReport/System.Waf.cobertura.xml --output-format cobertura "./**/TestResults/**/*.coverage"

reportgenerator -reports:../out/CodeCoverageReport/System.Waf.cobertura.xml -targetdir:../out/CodeCoverageReport -reporttypes:"MarkdownSummaryGithub"