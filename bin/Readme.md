All final binaries will be here...

Use 'build_x.bat' files-helpers to start build without Visual Studio like:

```
"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild.exe" "vsCommandEvent_2015.sln" /verbosity:normal /l:"packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll" /m:12 /p:Configuration=Debug /p:Platform="Any CPU"
```

*Or just click on `Build` - `Build Solution`*

/**[How to build](http://vsce.r-eg.net/doc/Dev/How%20to%20build/)** (the list of requirements etc.)