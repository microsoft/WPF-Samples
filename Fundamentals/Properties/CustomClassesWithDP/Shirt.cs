// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace CustomClassesWithDP
{
    public class Shirt : Canvas
    {
        public static readonly DependencyProperty ShirtColorProperty = DependencyProperty.Register("ShirtColor",
            typeof (ShirtColors), typeof (Shirt), new FrameworkPropertyMetadata(ShirtColors.None));

        public static readonly DependencyProperty ShirtTypeProperty = DependencyProperty.Register(
            "ShirtType",
            typeof (ShirtTypes),
            typeof (Shirt),
            new FrameworkPropertyMetadata(
                ShirtTypes.None,
                OnShirtTypeChangedCallback
                ), ShirtValidateCallback);

        public static readonly DependencyProperty ButtonColorProperty = DependencyProperty.Register(
            "ButtonColor",
            typeof (ButtonColors),
            typeof (Shirt),
            new FrameworkPropertyMetadata(
                ButtonColors.Black,
                OnButtonColorChangedCallback,
                CoerceButtonColor)
            );

        public static readonly RoutedEvent ButtonColorChangedEvent =
            EventManager.RegisterRoutedEvent("ButtonColorChanged", RoutingStrategy.Bubble,
                typeof (DependencyPropertyChangedEventHandler), typeof (Shirt));

        public ShirtColors ShirtColor
        {
            get { return (ShirtColors) GetValue(ShirtColorProperty); }
            set { SetValue(ShirtColorProperty, value); }
        }

        public ShirtTypes ShirtType
        {
            get { return (ShirtTypes) GetValue(ShirtTypeProperty); }
            set { SetValue(ShirtTypeProperty, value); }
        }

        public ButtonColors ButtonColor
        {
            get { return (ButtonColors) GetValue(ButtonColorProperty); }
            set { SetValue(ButtonColorProperty, value); }
        }

        private static bool ShirtValidateCallback(object value)
        {
            var sh = (ShirtTypes) value;
            return (sh == ShirtTypes.None || sh == ShirtTypes.Bowling || sh == ShirtTypes.Dress ||
                    sh == ShirtTypes.Rugby || sh == ShirtTypes.Tee);
        }

        private static void OnShirtTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(ButtonColorProperty);
        }

        private static void OnButtonColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newargs = new RoutedEventArgs(ButtonColorChangedEvent);
            (d as FrameworkElement).RaiseEvent(newargs);
        }

        private static object CoerceButtonColor(DependencyObject d, object value)
        {
            var newShirtType = (d as Shirt).ShirtType;
            if (newShirtType == ShirtTypes.Dress || newShirtType == ShirtTypes.Bowling)
            {
                return ButtonColors.Black;
            }
            return ButtonColors.None;
        }

        public event RoutedEventHandler ButtonColorChanged
        {
            add { AddHandler(ButtonColorChangedEvent, value); }
            remove { RemoveHandler(ButtonColorChangedEvent, value); }
        }
    }
}