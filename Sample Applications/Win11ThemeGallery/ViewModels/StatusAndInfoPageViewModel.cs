using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class StatusAndInfoPageViewModel : BaseNavigablePageViewModel
    {
        public StatusAndInfoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Status & Info";
            PageDescription = "Controls to show progress and extra information";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "ProgressBar",
                    PageType = typeof(ProgressBarPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ProgressBar.png"))},
                    Description = "Shows the apps progress on a task, or that the app is performing ongoing work that doesn't block user interaction."
                },
                new NavigationCard
                {
                    Name = "ToolTip",
                    PageType = typeof(ToolTipPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ToolTip.png"))},
                    Description = "Displays information for an element in a pop-up window."
                },
            };
        }
    }
}
