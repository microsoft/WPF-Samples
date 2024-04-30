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
    public partial class LayoutPageViewModel : BaseNavigablePageViewModel
    {
        public LayoutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Layout Controls";
            PageDescription = "Controls for layouting";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "Expander",
                    PageType = typeof(ExpanderPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Expander.png"))},
                    Description = "A container with a header that can be expanded to show a body with more content."
                },
            };
        }
    }
}
