using Microsoft.Win32;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class DataGridPage : Page
    {
        public DataGridPageViewModel ViewModel { get; }
        public DataGridPage(DataGridPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();

            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            this.Loaded += (s, e) => UpdatePageVisuals();

        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                UpdatePageVisuals();
            });
        }

        private void UpdatePageVisuals()
        {
            if(SystemParameters.HighContrast == true)
            {
                SampleDataGrid.SetResourceReference(BackgroundProperty, SystemColors.ControlBrushKey);
                SampleDataGrid.SetResourceReference(ForegroundProperty, SystemColors.ControlTextBrushKey);
            }
            else
            {
                SampleDataGrid.SetResourceReference(BackgroundProperty, DependencyProperty.UnsetValue);
                SampleDataGrid.SetResourceReference(ForegroundProperty, "TextFillColorPrimaryBrush");
            }
        }
    }
}
