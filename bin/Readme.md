All final binaries will be here...

Use ['build_x.bat'](../build) file-helpers for start build without Visual Studio like:

```
msbuild "vsCommandEvent.sln" /verbosity:normal /l:"packages\vsSBE.CI.MSBuild\bin\CI.MSBuild.dll" /m:4 /p:Configuration=Debug /p:Platform="Any CPU"
```

*Or click on `Build` - `Build Solution` if you have [installed plugin](https://visualstudiogallery.msdn.microsoft.com/0d1dbfd7-ed8a-40af-ae39-281bfeca2334/)*

/**[How to build](http://vsce.r-eg.net/doc/Dev/How%20to%20build/)** (the list of requirements etc.)