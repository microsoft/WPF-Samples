// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Controls;

namespace FontDialog
{
    internal class FontSizeListItem : TextBlock, IComparable
    {
        public FontSizeListItem(double sizeInPoints)
        {
            SizeInPoints = sizeInPoints;
            Text = sizeInPoints.ToString(CultureInfo.InvariantCulture);
        }

        public double SizeInPoints { get; }
        public double SizeInPixels => PointsToPixels(SizeInPoints);

        int IComparable.CompareTo(object obj)
        {
            double value;

            if (obj is double)
            {
                value = (double) obj;
            }
            else
            {
                if (!double.TryParse(obj.ToString(), out value))
                {
                    return 1;
                }
            }

            return
                FuzzyEqual(SizeInPoints, value)
                    ? 0
                    : (SizeInPoints < value) ? -1 : 1;
        }

        public override string ToString() => SizeInPoints.ToString(CultureInfo.InvariantCulture);

        public static bool FuzzyEqual(double a, double b) => Math.Abs(a - b) < 0.01;

        public static double PointsToPixels(double value) => value * (96.0 / 72.0);

        public static double PixelsToPoints(double value) => value * (72.0 / 96.0);
    }
}