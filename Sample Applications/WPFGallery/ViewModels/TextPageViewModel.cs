using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class TextPageViewModel : BaseSectionPageViewModel
    {
        public TextPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Text";
            PageDescription = "Controls for displaying and editing text";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Text");
        }
    }
}
