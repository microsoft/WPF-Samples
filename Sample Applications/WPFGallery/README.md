### WPF Gallery

This application showcases the new Fluent ( Win11 ) theme styles being introduced in WPF in .NET 9 Preview 4.

Developers need to do the following changes in order to use this application:
1. Download the latest .NET 9 preview version available [here](https://github.com/dotnet/installer?tab=readme-ov-file#table).
2. Update `sdk`'s `version` to latest 9 preview, or the one available in local system, in `global.json` present in the repository root.
3. 
    -  Clone the WPF repository, available [here](https://github.com/dotnet/wpf/)
    - Run the following commands: 
        ```
        git fetch origin pull/8870/head:wpf-win11theme
        git checkout wpf-win11theme
        ```
    - Build the WPF repository by running the command: `.\build.cmd -plat x64`
    - If the WPF build fails, install the components mentioned in [wpf.vsconfig](https://github.com/dotnet/wpf/blob/main/Documentation/wpf.vsconfig).
    - Update the `WpfRepoRoot` with it's path in `WPFGallery.csproj` file.
