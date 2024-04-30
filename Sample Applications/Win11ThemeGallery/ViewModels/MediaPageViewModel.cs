using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class MediaPageViewModel : BaseNavigablePageViewModel
    {
        public MediaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Media Controls";
            PageDescription = "Controls for media presentation";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "Canvas",
                    PageType = typeof(CanvasPage),
                    Description = "Canvas presenter"
                },
                new NavigationCard
                {
                    Name = "Image",
                    PageType = typeof(ImagePage),
                    Description = "Image presenter"
                },
            };
        }
    }
}
