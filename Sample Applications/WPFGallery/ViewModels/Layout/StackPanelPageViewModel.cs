
namespace WPFGallery.ViewModels.Layout;

public partial class StackPanelPageViewModel : ObservableObject 
{
    [ObservableProperty]
    private string _pageTitle = "StackPanel";

    [ObservableProperty]
    private string _pageDescription = "";

    public StackPanelPageViewModel() { }
}
