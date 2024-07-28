using System.Windows.Controls;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for DateAndTimePage.xaml
    /// </summary>
    public partial class DateAndTimePage : Page
    {
        public DateAndTimePageViewModel ViewModel { get; } 
		public DateAndTimePage(DateAndTimePageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }
    }
}
