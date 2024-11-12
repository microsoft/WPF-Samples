
namespace WPFGallery.ViewModels;

public partial class ExpanderPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Expander";

	[ObservableProperty]
	private string _pageDescription = "";

    public ExpanderPageViewModel() { }
}
