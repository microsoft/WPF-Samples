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
            if (e.NewSize.Width < 800) // Collapse ListView when screen is narrow
            {
                ListViewGrid.Visibility = Visibility.Collapsed;
                ToggleListButton.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(ContentGrid, 2);
                Grid.SetRowSpan(ContentGrid, 2);
                Grid.SetRow(ContentGrid, 0);
                Grid.SetColumn(ContentGrid, 0);
            }
            else // Show ListView when screen is wide
            {
                ListViewGrid.Visibility = Visibility.Visible;
                ToggleListButton.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(ContentGrid, 1);
                Grid.SetRowSpan(ContentGrid, 1);
                Grid.SetRow(ContentGrid, 1);
                Grid.SetColumn(ContentGrid, 1);
            }
        }

        private void ToggleListButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewGrid.Visibility == Visibility.Collapsed)
            {
                ListViewGrid.Visibility = Visibility.Visible;
                Panel.SetZIndex(ListViewGrid, 1);
            }
            else
            {
                ListViewGrid.Visibility = Visibility.Collapsed;
            }

            ToggleListButton.Visibility = Visibility.Collapsed;
            CollapseUserList.Focus();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (UsersList.Items.Count > 0)
            {
                ListViewItem firstItem = (ListViewItem)UsersList.ItemContainerGenerator.ContainerFromItem(UsersList.Items[0]);
                if (firstItem != null)
                {
                    firstItem.IsSelected = true;
                }
            }
        }

        private void CollapseUserList_Click(object sender, RoutedEventArgs e)
        {
            ListViewGrid.Visibility = Visibility.Collapsed;
            ToggleListButton.Visibility = Visibility.Visible;
            ToggleListButton.Focus();
        }
    }
}
