using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ExpenseIt
{
    /// <summary>
    /// Interaction logic for ExpenseItHome.xaml
    /// </summary>
    public partial class ExpenseItHome : Page
    {
        public ExpenseItHome()
        {
            InitializeComponent();
        }

        private void ExpenseItHome_Loaded(object sender, RoutedEventArgs e)
        {
            // Trigger live region to be read when loaded.
            FrameworkElementAutomationPeer.FromElement(LiveRegion)?.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // View Expense Report
            ExpenseReportPage expenseReportPage = new ExpenseReportPage(this.peopleListBox.SelectedItem);
            this.NavigationService.Navigate(expenseReportPage);
        }

    }
}
