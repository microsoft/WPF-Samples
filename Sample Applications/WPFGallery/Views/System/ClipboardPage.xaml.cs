using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for ClipboardPage.xaml
    /// </summary>
    public partial class ClipboardPage : Page
    {
        public ClipboardPageViewModel ViewModel { get; }

        public ClipboardPage(ClipboardPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var text = CopyTextBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
                ViewModel.CopyStatus = $"Copied \"{text}\" to clipboard!";
            }
            else
            {
                ViewModel.CopyStatus = "Nothing to copy - text box is empty.";
            }
        }

        private void PasteFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                ViewModel.PastedText = Clipboard.GetText();
            }
            else
            {
                ViewModel.PastedText = "(No text in clipboard)";
            }
        }

        private void ClearClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            ViewModel.ClearStatus = "Clipboard cleared!";
            ViewModel.PastedText = "";
        }

        private void CheckFormats_Click(object sender, RoutedEventArgs e)
        {
            var formats = new System.Text.StringBuilder();
            formats.AppendLine("Clipboard contains:");
            formats.AppendLine($"  • Text: {Clipboard.ContainsText()}");
            formats.AppendLine($"  • Image: {Clipboard.ContainsImage()}");
            formats.AppendLine($"  • File Drop List: {Clipboard.ContainsFileDropList()}");
            formats.AppendLine($"  • Audio: {Clipboard.ContainsAudio()}");
            ViewModel.FormatsInfo = formats.ToString();
        }

        private void CopyImageToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (SourceImage.Source is BitmapSource bitmapSource)
            {
                Clipboard.SetImage(bitmapSource);
                ViewModel.CopyImageStatus = "Image copied to clipboard!";
            }
            else
            {
                ViewModel.CopyImageStatus = "Failed to copy image.";
            }
        }

        private void PasteImageFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                BitmapSource image = Clipboard.GetImage();
                PastedImage.Source = image;
                ViewModel.PasteImageStatus = $"Image pasted! Size: {image.PixelWidth}x{image.PixelHeight}";
            }
            else
            {
                PastedImage.Source = null;
                ViewModel.PasteImageStatus = "No image in clipboard.";
            }
        }
    }
}
