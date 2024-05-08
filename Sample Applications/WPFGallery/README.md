### WPF Gallery

This application showcases the new Fluent ( Win11 ) theme styles being introduced in WPF in .NET 9 Preview 4.

Developers need to do the following changes in order to use this application:
1. Download the latest .NET 9 preview version available [here](https://github.com/dotnet/installer?tab=readme-ov-file#table).
2. Update `sdk`'s `version` to latest 9 preview, or the one available in local system, in `global.json` present in the repository root.

If the developer does not want to have the `Mica` backdrop effect in their applciation, they can do so by disabling it via the `RuntimeHostConfigurationOption`. This can be done by adding the following code to the `WPFGallery.csproj`.
```xml
<ItemGroup>
    <RuntimeHostConfigurationOption Include="Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop" Value="true" />
</ItemGroup>
```