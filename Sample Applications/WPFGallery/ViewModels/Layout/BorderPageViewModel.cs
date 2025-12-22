
namespace WPFGallery.ViewModels.Layout;

public partial class BorderPageViewModel : ObservableObject 
{
    [ObservableProperty]
    private string _pageTitle = "Border";

    [ObservableProperty]
    private string _pageDescription = "";

    public BorderPageViewModel() { }
}
