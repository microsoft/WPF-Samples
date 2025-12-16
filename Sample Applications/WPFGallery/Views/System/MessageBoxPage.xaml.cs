using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for MessageBoxPage.xaml
    /// </summary>
    public partial class MessageBoxPage : Page
    {
        public MessageBoxPageViewModel ViewModel { get; }

        public MessageBoxPage(MessageBoxPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void ShowDefaultMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This is a simple message box!");
            ViewModel.DefaultMessageResult = $"Result: {result}";
        }

        private void ShowCustomTitleButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This is a detailed description of what happened or what action is needed.", "Custom Title");
            ViewModel.CustomTitleResult = $"Result: {result}";
        }

        private void ShowButtonFromComboBox_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttonType = ViewModel.SelectedButtonIndex switch
            {
                0 => MessageBoxButton.OK,
                1 => MessageBoxButton.OKCancel,
                2 => MessageBoxButton.AbortRetryIgnore,
                3 => MessageBoxButton.YesNoCancel,
                4 => MessageBoxButton.YesNo,
                5 => MessageBoxButton.RetryCancel,
                6 => MessageBoxButton.CancelTryContinue,
                _ => MessageBoxButton.OK
            };

            string buttonName = ViewModel.SelectedButtonIndex switch
            {
                0 => "OK",
                1 => "OK/Cancel",
                2 => "Abort/Retry/Ignore",
                3 => "Yes/No/Cancel",
                4 => "Yes/No",
                5 => "Retry/Cancel",
                6 => "Cancel/Try/Continue",
                _ => "OK"
            };

            var result = MessageBox.Show($"This MessageBox has {buttonName} button(s).", $"{buttonName} Button(s)", buttonType);
            ViewModel.DifferentButtonsResult = $"Result: {result}";
        }

        private void ShowImageFromComboBox_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxImage imageType = ViewModel.SelectedImageIndex switch
            {
                0 => MessageBoxImage.None,
                1 => MessageBoxImage.Error,
                2 => MessageBoxImage.Question,
                3 => MessageBoxImage.Warning,
                4 => MessageBoxImage.Information,
                _ => MessageBoxImage.None
            };

            string imageName = ViewModel.SelectedImageIndex switch
            {
                0 => "None",
                1 => "Error",
                2 => "Question",
                3 => "Warning",
                4 => "Information",
                _ => "None"
            };

            var result = MessageBox.Show($"This MessageBox displays the {imageName} icon.", $"{imageName} Icon", MessageBoxButton.OK, imageType);
            ViewModel.DifferentImagesResult = $"Result: {result}";
        }

        // 6. Common Messages (Information, Error, Warning)
        private void ShowCommonInformation_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("The operation completed successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            ViewModel.CommonMessagesResult = $"Type: Information | Result: {result}";
        }

        private void ShowCommonError_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("An error occurred! The operation could not be completed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ViewModel.CommonMessagesResult = $"Type: Error | Result: {result}";
        }

        private void ShowCommonWarning_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This action cannot be undone! Do you want to continue?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            ViewModel.CommonMessagesResult = $"Type: Warning | Result: {result}";
        }

        // 7. Custom Default Button
        private void ShowCustomDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you want to save changes? Press Enter to select the default 'No' button.", "Save Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);
            ViewModel.CustomDefaultResult = $"User selected: {result}";
        }
    }
}
