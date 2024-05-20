# WPF Gallery

This application showcases the new Fluent ( Win11 ) theme styles being introduced in WPF in .NET 9 Preview 4.

![WPF Gallery Landing Page](Assets/README_Images/WPFGalleryLanding.png)

The WPF Gallery shows how to specify XAML controls in markup as each control page shows the markup used to create each example. It will also show all of the possible layout options for your app. 

It will also demonstrate the appearance of each control  across various themes. For example this is how the WPF Gallery Landing Page will look like in dark theme: 
![WPF Gallery Landing Page Dark](Assets/README_Images/WPFGalleryLandingDark.png)


## Further Information

Developers need to do the following changes in order to use this application:
1. Download the latest .NET 9 preview version available [here](https://github.com/dotnet/installer?tab=readme-ov-file#table).
2. Update `sdk`'s `version` to latest 9 preview, or the one available in local system, in `global.json` present in the repository root.

Additionally, if developer wants to use the locally built WPF binaries, the following steps in addition to the above 2 steps needs to be done:
- Clone the WPF repository, available [here](https://github.com/dotnet/wpf/)
- Build the `main` branch in the repository by running the command: `.\build.cmd -plat x64`
- If the WPF build fails, install the components mentioned in [wpf.vsconfig](https://github.com/dotnet/wpf/blob/main/Documentation/wpf.vsconfig).
- Update the `WpfRepoRoot` with it's path in `WPFGallery.csproj` file.

If the developer does not want to have the `Mica` backdrop effect in their applciation, they can do so by disabling it via the `RuntimeHostConfigurationOption`. This can be done by adding the following code to the `WPFGallery.csproj`.
```xml
<ItemGroup>
    <RuntimeHostConfigurationOption Include="Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop" Value="true" />
</ItemGroup>
```