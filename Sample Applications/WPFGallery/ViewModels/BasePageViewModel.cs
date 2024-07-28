
namespace WPFGallery.ViewModels;

public partial class BasePageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _pageTitle = "";

    [ObservableProperty]
    private string _pageDescription = "";

    public BasePageViewModel()
    {
    }

    public BasePageViewModel(string pageTitle, string pageDescription = "")
    {
        PageTitle = pageTitle;
        PageDescription = pageDescription;
    }
}