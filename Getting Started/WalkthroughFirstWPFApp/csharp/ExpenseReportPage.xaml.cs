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

    }
  
}
