using System.Windows;
using System.Windows.Media;

namespace WPFGallery.Helpers
{
    internal static class Utility
    {
        public static bool IsBackdropSupported()
        {
            var os = Environment.OSVersion;
            var version = os.Version;

            return version.Major >= 10 && version.Build >= 22621;
        }

        public static bool IsBackdropDisabled()
        {
            var appContextBackdropData = AppContext.GetData("Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop");
            bool disableFluentThemeWindowBackdrop = false;

            if (appContextBackdropData != null)
            {
                disableFluentThemeWindowBackdrop = bool.Parse(Convert.ToString(appContextBackdropData));
            }

            return disableFluentThemeWindowBackdrop;
        }

        public static bool IsLightTheme()
        {
            try
            {
                var themeMode = Application.Current.ThemeMode;

                if (themeMode == ThemeMode.Light)
                    return true;
                if (themeMode == ThemeMode.Dark)
                    return false;

                // For System theme, detect the actual effective theme
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    var backgroundResource = mainWindow.TryFindResource("SolidBackgroundFillColorBaseBrush");
                    if (backgroundResource is SolidColorBrush brush)
                    {
                        var color = brush.Color;
                        var luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
                        return luminance > 0.5;
                    }
                }

                return themeMode != ThemeMode.Dark;
            }
            catch
            {
                return true;
            }
        }
    }
}
