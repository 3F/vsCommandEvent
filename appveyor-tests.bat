@echo off
echo Usage: %~nx0 [configuration name or nothing to test all]

set "cfg=%~1"
set ciLogger=Appveyor.TestLogger -Version 2.0.0
set tcmd=dotnet test --no-build --no-restore --test-adapter-path:. --logger:Appveyor

::::::::::::::::::::

setlocal
    cd vsCommandEventTest
    nuget install %ciLogger%
endlocal

if not defined cfg (

    call %tcmd% -c REL_SDK10 vsCommandEventTest
    call %tcmd% -c REL_SDK15 vsCommandEventTest
    call %tcmd% -c REL_SDK17 vsCommandEventTest

) else (
    call %tcmd% -c %cfg% vsCommandEventTest
)

exit /B 0

:err
echo. Build failed. 1>&2
exit /B 1