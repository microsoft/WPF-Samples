using System.Windows.Markup;
using System.Windows.Controls;

namespace WPFGallery.Controls
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
