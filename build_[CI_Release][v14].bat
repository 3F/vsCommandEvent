.nuget\nuget restore vsCommandEvent_2015.sln 
"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild.exe" "vsCommandEvent_2015.sln" /verbosity:normal  /l:"packages\vsSBE.CI.MSBuild.1.5.1\bin\CI.MSBuild.dll" /m:12 /t:Rebuild /p:Configuration=CI_Release /p:Platform="Any CPU"