using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win11ThemeGallery.Models;

namespace Win11ThemeGallery.Helpers;

internal sealed class ImageIdToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string imageKey = (string)value;
        return Application.Current.Resources[imageKey];

        // switch (imageId)
        // {
        //     case "64":
        //         return Application.Current.Resources["p64"];

        //     default:
        //         return Brushes.Transparent;
        // }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
