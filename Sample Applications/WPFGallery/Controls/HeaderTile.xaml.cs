
namespace WPFGallery.Controls
{
    /// <summary>
    /// Interaction logic for HeaderTile.xaml
    /// </summary>
    public partial class HeaderTile : UserControl
    {
        public HeaderTile()
        {
            InitializeComponent();
            UpdateButtonResources();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        private void SystemParameters_StaticPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateButtonResources();
        }

        private void UpdateButtonResources()
        {
            if (!SystemParameters.HighContrast)
            {
                Color? color = (Color)Application.Current!.FindResource("AcrylicBackgroundFillColorDefault");

                RootButton.Resources["ButtonBackground"] = new SolidColorBrush { Color = color ?? Colors.Gray, Opacity = 0.8 };
                RootButton.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush { Color = color ?? Colors.Gray, Opacity = 0.9 };
                RootButton.Resources["ButtonBackgroundPressed"] = new SolidColorBrush { Color = color ?? Colors.Gray, Opacity = 1.0 };
            }
            else
            {
                RootButton.Resources["ButtonBackground"] = SystemColors.ControlBrush;
                RootButton.Resources["ButtonBackgroundPointerOver"] = SystemColors.ControlBrush;
                RootButton.Resources["ButtonBackgroundPressed"] = SystemColors.ControlBrush;
            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(HeaderTile), new PropertyMetadata(""));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("ColorExplanation", typeof(string), typeof(HeaderTile), new PropertyMetadata(""));

        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }

        public static readonly DependencyProperty LinkProperty = 
            DependencyProperty.Register("Link", typeof(string), typeof(HeaderTile), new PropertyMetadata(null));

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(HeaderTile), new PropertyMetadata(null));

        private void RootButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(Link) { UseShellExecute = true });
        }
    }
}
