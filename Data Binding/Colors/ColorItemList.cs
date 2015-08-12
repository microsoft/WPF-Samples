// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media;

namespace Colors
{
    public class ColorItemList : ObservableCollection<ColorItem>
    {
        public ColorItemList()
        {
            var type = typeof (Brushes);
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (propertyInfo.PropertyType == typeof (SolidColorBrush))
                    Add(new ColorItem(propertyInfo.Name, (SolidColorBrush) propertyInfo.GetValue(null, null)));
            }
        }
    }
}