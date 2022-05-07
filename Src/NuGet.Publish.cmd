@echo off

REM ***
REM *** Set up the enviroment.
REM ***
CALL "%ProgramFiles%\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsamd64_arm64.bat"

REM ***
REM *** Get the NuGet publish key.
REM ***
SET /P PUBLISH_KEY=Enter the publish key:

REM ***
REM *** Publish all *.nupkg files.
REM ***
forfiles /P "%CD%" /S /M *.nupkg /C "cmd /c dotnet nuget push @path -k %PUBLISH_KEY% -s https://api.nuget.org/v3/index.json"

REM ***
REM *** Pause to view errors.
REM ***
pause