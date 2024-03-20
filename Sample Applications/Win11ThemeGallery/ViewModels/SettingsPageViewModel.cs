
using System.Windows.Controls;

namespace Win11ThemeGallery.ViewModels;

/// <summary>
/// Interaction logic for Settings.xaml
/// </summary>
public partial class SettingsPageViewModel : ObservableObject
{
    //[ObservableProperty]
    //private string _pageTitle = "Settings";

    //[ObservableProperty]
    //private ApplicationTheme _currentApplicationTheme = ApplicationTheme.Unknown;

    //private void OnThemeChanged(ApplicationTheme currentApplicationTheme, Color systemAccent)
    //{
    //    // Update the theme if it has been changed elsewhere than in the settings.
    //    if (CurrentApplicationTheme != currentApplicationTheme)
    //    {
    //        CurrentApplicationTheme = currentApplicationTheme;
    //    }
    //}

    //public SettingsPageViewModel()
    //{
    //    CurrentApplicationTheme = ApplicationThemeManager.GetAppTheme();
    //    ApplicationThemeManager.Changed += OnThemeChanged;
    //}

    //partial void OnCurrentApplicationThemeChanged(ApplicationTheme oldValue, ApplicationTheme newValue)
    //{
    //    ApplicationThemeManager.Apply(newValue);
    //}

}