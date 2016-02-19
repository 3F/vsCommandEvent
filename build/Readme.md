A very simple files-helpers to start build, for example:

```
"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild.exe" "vsCommandEvent_2015.sln" /verbosity:normal /l:"packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll" /m:12 /p:Configuration=Debug /p:Platform="Any CPU"
```

/**[How to build](http://vsce.r-eg.net/doc/Dev/How%20to%20build/)**