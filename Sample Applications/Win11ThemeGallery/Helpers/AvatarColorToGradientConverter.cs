using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win11ThemeGallery.Models;

namespace Win11ThemeGallery.Helpers;

internal sealed class AvatarColorToGradientConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var avatarColor = (AvatarColor)value;
        switch (avatarColor)
        {
            case AvatarColor.Green:
                return new LinearGradientBrush(Colors.LightGreen, Colors.DarkGreen, 0);
            case AvatarColor.Blue:
                return new LinearGradientBrush(Colors.LightBlue, Colors.DarkBlue, 0);
            case AvatarColor.Red:
                return new LinearGradientBrush(Colors.OrangeRed, Colors.DarkRed, 0);
            case AvatarColor.Orange:
                return new LinearGradientBrush(Colors.Orange, Colors.DarkOrange, 0);
            case AvatarColor.Pink:
                return new LinearGradientBrush(Colors.LightPink, Colors.DeepPink, 0);
            case AvatarColor.Purple:
                return new LinearGradientBrush(Colors.Purple, Colors.DarkViolet, 0);
            case AvatarColor.Teal:
                return new LinearGradientBrush(Colors.Teal, Colors.DarkCyan, 0);
            case AvatarColor.Lilac:
                return new LinearGradientBrush(Colors.MediumPurple, Colors.DarkMagenta, 0);
            case AvatarColor.Caramel:
                return new LinearGradientBrush(Colors.Gold, Colors.DarkGoldenrod, 0);
            case AvatarColor.Yellow:
                return new LinearGradientBrush(Colors.Yellow, Colors.Goldenrod, 0);
            case AvatarColor.White:
                return new LinearGradientBrush(Colors.White, Colors.LightGray, 0);
            default:
                return Brushes.Transparent;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
