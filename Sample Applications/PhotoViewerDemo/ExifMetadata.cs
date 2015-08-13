// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Media.Imaging;

namespace PhotoViewerDemo
{
    public class ExifMetadata
    {
        private readonly BitmapMetadata _metadata;

        public ExifMetadata(Uri imageUri)
        {
            var frame = BitmapFrame.Create(imageUri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            _metadata = (BitmapMetadata) frame.Metadata;
        }

        public uint? Width
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=40962}");
                if (val == null)
                {
                    return null;
                }
                if (val.GetType() == typeof (uint))
                {
                    return (uint?) val;
                }
                return Convert.ToUInt32(val);
            }
        }

        public uint? Height
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=40963}");
                if (val == null)
                {
                    return null;
                }
                if (val.GetType() == typeof (uint))
                {
                    return (uint?) val;
                }
                return Convert.ToUInt32(val);
            }
        }

        public decimal? HorizontalResolution
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=282}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong) val);
                }
                return null;
            }
        }

        public decimal? VerticalResolution
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=283}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong) val);
                }
                return null;
            }
        }

        public string EquipmentManufacturer
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=271}");
                return (val != null ? (string) val : string.Empty);
            }
        }

        public string CameraModel
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=272}");
                return (val != null ? (string) val : string.Empty);
            }
        }

        public string CreationSoftware
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif:{uint=305}");
                return (val != null ? (string) val : string.Empty);
            }
        }

        public ColorRepresentation ColorRepresentation
        {
            get
            {
                if ((ushort) QueryMetadata("/app1/ifd/exif/subifd:{uint=40961}") == 1)
                    return ColorRepresentation.SRgb;
                return ColorRepresentation.Uncalibrated;
            }
        }

        public decimal? ExposureTime
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=33434}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong) val);
                }
                return null;
            }
        }

        public decimal? ExposureCompensation
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=37380}");
                if (val != null)
                {
                    return ParseSignedRational((long) val);
                }
                return null;
            }
        }

        public decimal? LensAperture
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=33437}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong) val);
                }
                return null;
            }
        }

        public decimal? FocalLength
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=37386}");
                if (val != null)
                {
                    return ParseUnsignedRational((ulong) val);
                }
                return null;
            }
        }

        public ushort? IsoSpeed => (ushort?) QueryMetadata("/app1/ifd/exif/subifd:{uint=34855}");

        public FlashMode FlashMode
        {
            get
            {
                if ((ushort) QueryMetadata("/app1/ifd/exif/subifd:{uint=37385}")%2 == 1)
                    return FlashMode.FlashFired;
                return FlashMode.FlashDidNotFire;
            }
        }

        public ExposureMode ExposureMode
        {
            get
            {
                var mode = (ushort?) QueryMetadata("/app1/ifd/exif/subifd:{uint=34850}");

                if (mode == null)
                {
                    return ExposureMode.Unknown;
                }
                switch ((int) mode)
                {
                    case 1:
                        return ExposureMode.Manual;
                    case 2:
                        return ExposureMode.NormalProgram;
                    case 3:
                        return ExposureMode.AperturePriority;
                    case 4:
                        return ExposureMode.ShutterPriority;
                    case 5:
                        return ExposureMode.LowSpeedMode;
                    case 6:
                        return ExposureMode.HighSpeedMode;
                    case 7:
                        return ExposureMode.PortraitMode;
                    case 8:
                        return ExposureMode.LandscapeMode;
                    default:
                        return ExposureMode.Unknown;
                }
            }
        }

        public WhiteBalanceMode WhiteBalanceMode
        {
            get
            {
                var mode = (ushort?) QueryMetadata("/app1/ifd/exif/subifd:{uint=37384}");

                if (mode == null)
                {
                    return WhiteBalanceMode.Unknown;
                }
                switch ((int) mode)
                {
                    case 1:
                        return WhiteBalanceMode.Daylight;
                    case 2:
                        return WhiteBalanceMode.Fluorescent;
                    case 3:
                        return WhiteBalanceMode.Tungsten;
                    case 10:
                        return WhiteBalanceMode.Flash;
                    case 17:
                        return WhiteBalanceMode.StandardLightA;
                    case 18:
                        return WhiteBalanceMode.StandardLightB;
                    case 19:
                        return WhiteBalanceMode.StandardLightC;
                    case 20:
                        return WhiteBalanceMode.D55;
                    case 21:
                        return WhiteBalanceMode.D65;
                    case 22:
                        return WhiteBalanceMode.D75;
                    case 255:
                        return WhiteBalanceMode.Other;
                    default:
                        return WhiteBalanceMode.Unknown;
                }
            }
        }

        public DateTime? DateImageTaken
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=36867}");
                if (val == null)
                {
                    return null;
                }
                var date = (string) val;
                try
                {
                    return new DateTime(
                        int.Parse(date.Substring(0, 4)), // year
                        int.Parse(date.Substring(5, 2)), // month
                        int.Parse(date.Substring(8, 2)), // day
                        int.Parse(date.Substring(11, 2)), // hour
                        int.Parse(date.Substring(14, 2)), // minute
                        int.Parse(date.Substring(17, 2)) // second
                        );
                }
                catch (FormatException)
                {
                    return null;
                }
                catch (OverflowException)
                {
                    return null;
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
        }

        private decimal ParseUnsignedRational(ulong exifValue) => (exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0xFFFFFFFF00000000L) >> 32);

        private decimal ParseSignedRational(long exifValue) => (exifValue & 0xFFFFFFFFL) / (decimal)((exifValue & 0x7FFFFFFF00000000L) >> 32);

        private object QueryMetadata(string query)
        {
            if (_metadata.ContainsQuery(query))
                return _metadata.GetQuery(query);
            return null;
        }
    }
}