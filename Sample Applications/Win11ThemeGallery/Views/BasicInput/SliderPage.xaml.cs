using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Win11ThemeGallery.ViewModels;

namespace Win11ThemeGallery.Views
{
    /// <summary>
    /// Interaction logic for SliderPage.xaml
    /// </summary>
    public partial class SliderPage : Page
    {
        public SliderPageViewModel ViewModel { get; }

        public SliderPage(SliderPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
