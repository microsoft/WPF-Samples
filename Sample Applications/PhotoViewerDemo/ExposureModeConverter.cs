// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     Converts an exposure mode from an enum into a human-readable string (e.g. AperturePriority
    ///     becomes "Aperture Priority")
    /// </summary>
    public class ExposureModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var exposureMode = (ExposureMode) value;

            switch (exposureMode)
            {
                case ExposureMode.AperturePriority:
                    return "Aperture Priority";

                case ExposureMode.HighSpeedMode:
                    return "High Speed Mode";

                case ExposureMode.LandscapeMode:
                    return "Landscape Mode";

                case ExposureMode.LowSpeedMode:
                    return "Low Speed Mode";

                case ExposureMode.Manual:
                    return "Manual";

                case ExposureMode.NormalProgram:
                    return "Normal";

                case ExposureMode.PortraitMode:
                    return "Portrait";

                case ExposureMode.ShutterPriority:
                    return "Shutter Priority";

                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}