setlocal
cd %~dp0..\src\
powershell -c "Get-ChildItem -inc bin,obj -rec | Remove-Item -rec -force"