
using WPFGallery.ViewModels;

namespace WPFGallery.Views;
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
    public CalendarPageViewModel ViewModel { get; }

    public CalendarPage(CalendarPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
    }
