// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PropertyAnimation
{
    // Uses a storyboard to animate the properties
    // of two buttons.
    public class StoryboardExample : Page
    {
        public StoryboardExample()
        {
            // Create a name scope for the page.
            NameScope.SetNameScope(this, new NameScope());

            WindowTitle = "Animate Properties using Storyboards";
            var myStackPanel = new StackPanel
            {
                MinWidth = 500,
                Margin = new Thickness(30),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var myTextBlock = new TextBlock {Text = "Storyboard Animation Example"};
            myStackPanel.Children.Add(myTextBlock);

            //
            // Create and animate the first button.
            //

            // Create a button.
            var myWidthAnimatedButton = new Button
            {
                Height = 30,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = "A Button",
                Name = "myWidthAnimatedButton"
            };

            // Set the Name of the button so that it can be referred
            // to in the storyboard that's created later.
            // The ID doesn't have to match the variable name;
            // it can be any unique identifier.

            // Register the name with the page to which the button belongs.
            RegisterName(myWidthAnimatedButton.Name, myWidthAnimatedButton);

            // Create a DoubleAnimation to animate the width of the button.
            var myDoubleAnimation = new DoubleAnimation
            {
                From = 200,
                To = 300,
                Duration = new Duration(TimeSpan.FromMilliseconds(3000))
            };

            // Configure the animation to target the button's Width property.
            Storyboard.SetTargetName(myDoubleAnimation, myWidthAnimatedButton.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(WidthProperty));

            // Create a storyboard to contain the animation.
            var myWidthAnimatedButtonStoryboard = new Storyboard();
            myWidthAnimatedButtonStoryboard.Children.Add(myDoubleAnimation);

            // Animate the button width when it's clicked.
            myWidthAnimatedButton.Click +=
                delegate { myWidthAnimatedButtonStoryboard.Begin(myWidthAnimatedButton); };


            myStackPanel.Children.Add(myWidthAnimatedButton);

            //
            // Create and animate the second button.
            //

            // Create a second button.
            var myColorAnimatedButton = new Button
            {
                Height = 30,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = "Another Button"
            };

            // Create a SolidColorBrush to paint the button's background.
            var myBackgroundBrush = new SolidColorBrush {Color = Colors.Blue};

            // Because a Brush isn't a FrameworkElement, it doesn't
            // have a Name property to set. Instead, you just
            // register a name for the SolidColorBrush with
            // the page where it's used.
            RegisterName("myAnimatedBrush", myBackgroundBrush);

            // Use the brush to paint the background of the button.
            myColorAnimatedButton.Background = myBackgroundBrush;

            // Create a ColorAnimation to animate the button's background.
            var myColorAnimation = new ColorAnimation
            {
                From = Colors.Red,
                To = Colors.Blue,
                Duration = new Duration(TimeSpan.FromMilliseconds(7000))
            };

            // Configure the animation to target the brush's Color property.
            Storyboard.SetTargetName(myColorAnimation, "myAnimatedBrush");
            Storyboard.SetTargetProperty(myColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            // Create a storyboard to contain the animation.
            var myColorAnimatedButtonStoryboard = new Storyboard();
            myColorAnimatedButtonStoryboard.Children.Add(myColorAnimation);

            // Animate the button background color when it's clicked.
            myColorAnimatedButton.Click +=
                delegate { myColorAnimatedButtonStoryboard.Begin(myColorAnimatedButton); };


            myStackPanel.Children.Add(myColorAnimatedButton);
            Content = myStackPanel;
        }
    }
}

//</Snippet11>