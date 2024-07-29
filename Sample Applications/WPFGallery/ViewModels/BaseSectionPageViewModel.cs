using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels;

public partial class BaseSectionPageViewModel : BasePageViewModel
{
    [ObservableProperty]
    private ICollection<ControlInfoDataItem> _navigationCards;

    private readonly INavigationService _navigationService;

    public BaseSectionPageViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    public void Navigate(object navCard){
        if (navCard is ControlInfoDataItem dataItem)
        {
            _navigationService.NavigateTo(dataItem);
        }
    }
}