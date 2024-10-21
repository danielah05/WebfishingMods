# SprintToggle

To build the project, you need to set the `GDWeavePath` variable to your game install's GDWeave directory. Create `SprintToggle.csproj.user` next to the original `.csproj`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <GDWeavePath>G:\games\steam\steamapps\common\WEBFISHING\GDWeave</GDWeavePath>
    <AssemblySearchPaths>$(AssemblySearchPaths);$(GDWeavePath)/core</AssemblySearchPaths>
  </PropertyGroup>
</Project>
```
