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

        private void ShowSimpleMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This is a simple message box!");
            ViewModel.SimpleMessageResult = $"Result: {result}";
        }

        private void ShowMessageWithTitleButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This message box has a custom title.", "Custom Title");
            ViewModel.MessageWithTitleResult = $"Result: {result}";
        }

        private void ShowYesNoCancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            ViewModel.YesNoCancelResult = $"User selected: {result}";
        }

        private void ShowErrorButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("An error has occurred! The operation could not be completed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ViewModel.ErrorResult = $"Result: {result}";
        }

        private void ShowWarningButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This action cannot be undone! Do you want to continue?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            ViewModel.WarningResult = $"User selected: {result}";
        }

        private void ShowInformationButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("The operation completed successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            ViewModel.InformationResult = $"Result: {result}";
        }

        private void ShowCustomDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Press ESC or Enter to see the default button behavior. 'No' is the default button.", "Default Button Demo", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);
            ViewModel.CustomDefaultResult = $"User selected: {result}";
        }
    }
}
