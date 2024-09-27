using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.ViewModels.Samples;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for UserDashboardPage.xaml
    /// </summary>
    public partial class UserDashboardPage : Page
    {
        public UserDashboardPageViewModel ViewModel { get; }

        public UserDashboardPage(UserDashboardPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (sender as Button)?.Command;
            var commandParameter = (sender as Button)?.CommandParameter;
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }

            save_button.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (sender as Button)?.Command;
            var commandParameter = (sender as Button)?.CommandParameter;
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }

            edit_button.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (sender as Button)?.Command;
            var commandParameter = (sender as Button)?.CommandParameter;
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }

            edit_button.Focus();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Width < 800)
            {
                Grid.SetColumnSpan(UserListGrid,2);
                Grid.SetRowSpan(UserListGrid, 1);
                Grid.SetRow(UserListGrid, 0);
                Grid.SetColumn(UserListGrid, 0);
                Grid.SetColumnSpan(UserDetailsGrid, 2);
                Grid.SetRowSpan(UserDetailsGrid, 1);
                Grid.SetRow(UserDetailsGrid, 1);
                Grid.SetColumn(UserDetailsGrid, 0);
                UserList.ClearValue(WidthProperty);
                NewUserButton.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                Grid.SetColumnSpan(UserListGrid, 1);
                Grid.SetRowSpan(UserListGrid, 2);
                Grid.SetRow(UserListGrid, 0);
                Grid.SetColumn(UserListGrid, 0);
                Grid.SetColumnSpan(UserDetailsGrid, 1);
                Grid.SetRowSpan(UserDetailsGrid, 2);
                Grid.SetRow(UserDetailsGrid, 0);
                Grid.SetColumn(UserDetailsGrid, 1);
                UserList.Width = 300;
                NewUserButton.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
    }
}
