using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ExpenseIt
{
    /// <summary>
    /// Interaction logic for ExpenseReportPage.xaml
    /// </summary>
    
    public partial class ExpenseReportPage : Page
    {
        public ExpenseReportPage()
        {
            InitializeComponent();
        }

        // Custom constructor to pass expense report data
        public ExpenseReportPage(object data):this()
        {
            // Bind to expense report data.
            this.DataContext = data;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Trigger live region to be read when loaded.
            // Note that we announce two live regions for this page that is so we can trigger screen readers to read
            // the text of "Expense Report For" but then also append the actual name of the person we are viewing the
            // the report for.
            FrameworkElementAutomationPeer.FromElement(LiveRegion)?.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            FrameworkElementAutomationPeer.FromElement(NameLiveRegion)?.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            FrameworkElementAutomationPeer.FromElement(DepartmentLiveRegion)?.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
        }
    }
  
}
