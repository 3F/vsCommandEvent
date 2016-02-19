call "%~dp0_config"

REM # Configuration
set cfgname=CI_Debug

REM # Version of MSBuild tool
set _msbuild=12.0

call "%~dp0_build"