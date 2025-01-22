
namespace WPFGallery.ViewModels
{
    public partial class RichTextEditPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "RichTextEdit";

	[ObservableProperty]
	private string _pageDescription = "";

    }
}
