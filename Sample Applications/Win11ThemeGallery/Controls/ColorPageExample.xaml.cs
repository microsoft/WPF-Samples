using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Win11ThemeGallery.Controls
{
    /// <summary>
    /// Interaction logic for ColorPageExample.xaml
    /// </summary>
    [ContentProperty(nameof(ExampleContent))]
    public partial class ColorPageExample : UserControl
    {
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ColorPageExample), new PropertyMetadata(""));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ColorPageExample), new PropertyMetadata(""));

        public UIElement ExampleContent
        {
            get { return (UIElement)GetValue(ExampleContentProperty); }
            set { SetValue(ExampleContentProperty, value); }
        }
        public static readonly DependencyProperty ExampleContentProperty =
            DependencyProperty.Register("ExampleContent", typeof(UIElement), typeof(ColorPageExample), new PropertyMetadata(null));

    }
}
