.nuget\nuget restore vsCommandEvent.sln 
"C:\Program Files (x86)\MSBuild\12.0\bin\msbuild.exe" "vsCommandEvent.sln" /verbosity:normal  /l:"packages\vsSBE.CI.MSBuild.1.5.1\bin\CI.MSBuild.dll" /m:12 /t:Rebuild /p:Configuration=CI_Release /p:Platform="Any CPU"