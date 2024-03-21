## WPF Fluent Windows Gallery Application
The application is a control catalog, containing various controls, their variations and appearance in FluentWindows style. The application is still a work in progress, with occasional irregularities in sample codes of some controls.

Developers need to do the following changes in order to use this application:
1. Download the latest .NET 9 preview version available [here](https://github.com/dotnet/installer?tab=readme-ov-file#table).
2. Update `sdk`'s `version` to latest 9 preview, or the one available in local system, in `global.json` present in the repository root.
3. Clone and build the WPF repository, available [here](https://github.com/dotnet/wpf/), and update the `WpfRepoRoot` with it's path in `Win11ThemeGallery.csproj` file.

For detailed steps on debugging locally built WPF assemblies with WPF application refer [this](https://github.com/dotnet/wpf/blob/main/Documentation/developer-guide.md#debugging-locally-built-wpf-assemblies-with-wpf-application).